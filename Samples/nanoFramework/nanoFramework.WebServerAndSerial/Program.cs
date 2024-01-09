// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

#if NO_BLUETOOTH
using Iot.Device.DhcpServer;
using nanoFramework.WebServerAndSerial.WirelessSetup;
#endif
using Lego.Infrared;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Networking;
using nanoFramework.Runtime.Native;
using nanoFramework.WebServer;
using nanoFramework.WebServerAndSerial.Controllers;
using nanoFramework.WebServerAndSerial.Models;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Web;
using System.IO.Ports;

namespace nanoFramework.WebServerAndSerial
{
    public class Application
    {
#if !NO_BLUETOOTH
        private static Improv _imp;
#endif
        private static AppConfiguration _appConfiguration;
        private static SerialPort _port;
        private static LegoInfrared _legoInfrared;
        public static AppConfiguration AppConfiguration { get => _appConfiguration; }
        public static LegoInfrared LegoInfrared { get => _legoInfrared; }
        private static WebServer.WebServer _server;
        private static bool _wifiApMode = false;

        public static void Main()
        {
            Console.WriteLine("Lego Infrared REST API and Serial module");

            // Try to read the configuration
            _appConfiguration = AppConfiguration.Load();
            if (AppConfiguration != null)
            {
                SetLegoInfrared();
                SetSerial();
            }

            _appConfiguration = AppConfiguration ?? new AppConfiguration();

#if NO_BLUETOOTH
            if (Wireless80211.IsEnabled())
            {
                Console.WriteLine("Wireless client activated");
                if (!WifiNetworkHelper.Reconnect(true, token: new CancellationTokenSource(10_000).Token))
                {
                    SetWifiAp();
                }
            }
            else
            {
                SetWifiAp();
            }
#else
            if (!WifiNetworkHelper.Reconnect(true, token: new CancellationTokenSource(5_000).Token))
            {
                SetImprove();
            }
#endif

#if NO_BLUETOOTH
            Console.WriteLine($"Connected with wifi credentials. IP Address: {(_wifiApMode ? WirelessAP.GetIP() : GetCurrentIPAddress())}");
#else
            Console.WriteLine($"Connected with wifi credentials. IP Address: {GetCurrentIPAddress()}");
#endif
            _server = new WebServer.WebServer(80, HttpProtocol.Http, new Type[] { typeof(ApiController), typeof(ConfigurationController) });
            // Add a handler for commands that are received by the server.
            _server.CommandReceived += ServerCommandReceived;

            _server.Credential = new NetworkCredential(AppConfiguration.Login, AppConfiguration.Password);

            // Start the server.
            _server.Start();

            AppConfiguration.OnConfigurationUpdated += OnConfigurationUpdated;

            Thread.Sleep(Timeout.Infinite);
        }

        private static void OnConfigurationUpdated(object sender, ConfigurationEventArgs e)
        {
            Console.WriteLine($"Parameter updated: {e.ParamName}");
            if (e.ParamName.StartsWith("Spi"))
            {
                SetLegoInfrared();
            }

            if (e.ParamName.StartsWith("Serial"))
            {
                SetSerial();
            }

            if ((e.ParamName == nameof(AppConfiguration.Login)) || (e.ParamName == nameof(AppConfiguration.Password)))
            {
                _server.Credential = new NetworkCredential(AppConfiguration.Login, AppConfiguration.Password);
            }
        }

        private static void ServerCommandReceived(object obj, WebServerEventArgs e)
        {
            if (e.Context.Request.RawUrl.StartsWith("/style.css"))
            {
                e.Context.Response.ContentType = "text/css";
                WebServer.WebServer.OutPutStream(e.Context.Response, ResourceWeb.GetString(ResourceWeb.StringResources.style));
                return;
            }
            else if (e.Context.Request.RawUrl.StartsWith("/favicon.ico"))
            {
                var ico = ResourceWeb.GetBytes(ResourceWeb.BinaryResources.favicon);
                e.Context.Response.ContentType = "image/ico";
                e.Context.Response.ContentLength64 = ico.Length;
                e.Context.Response.OutputStream.Write(ico, 0, ico.Length);
                return;
            }

#if NO_BLUETOOTH
            if (_wifiApMode)
            {
                if (e.Context.Request.HttpMethod == "GET")
                {
                    string route = $"<!DOCTYPE html><html><body>" +
                        "<h1>NanoFramework</h1>" +
                        "<form method='POST'  action='/'>" +
                        "<fieldset><legend>Wireless configuration</legend>" +
                        "Ssid:</br><input type='input' name='ssid' value='' ></br>" +
                        "Password:</br><input type='password' name='password' value='' >" +
                        "<br><br>" +
                        "<input type='submit' value='Save'>" +
                        "</fieldset>" +
                        "</form></body></html>";
                    WebServer.WebServer.OutPutStream(e.Context.Response, route);
                }
                else
                {
                    byte[] buff = new byte[e.Context.Request.ContentLength64];
                    e.Context.Request.InputStream.Read(buff, 0, buff.Length);
                    string paramString = Encoding.UTF8.GetString(buff, 0, buff.Length);

                    // We're adding back the question mark as it's not present when posting
                    var parameters = WebServer.WebServer.DecodeParam($"{WebServer.WebServer.ParamStart}{paramString}");
                    string ssid = string.Empty;
                    string password = string.Empty;
                    foreach (UrlParameter param in parameters)
                    {
                        if (param.Name == "ssid")
                        {
                            ssid = HttpUtility.UrlDecode(param.Value);
                        }
                        else if (param.Name == "password")
                        {
                            password = HttpUtility.UrlDecode(param.Value);
                        }
                    }

                    Console.WriteLine($"SSID: {ssid}, password: {password}");

                    // Enable the Wireless station interface
                    bool res = Wireless80211.Configure(ssid, password);

                    var route = $"<!DOCTYPE html><html><body>" +
                        "<h1>NanoFramework</h1>" +
                        "<p>New settings saved.</p><p>Rebooting device to put into normal mode.</p>" +
                        "<p>Please allow up to 10 seconds to reconnect to the IP address.</p>";
                    if (res)
                    {
                        route += $"<p>IP Address shoud be <a href='http://{GetCurrentIPAddress()}'>http://{GetCurrentIPAddress()}</a>.</p>";
                    }

                    route += $"<p>If not configured properly, connect again to the SSID {WirelessAP.SoftApSsid} and then to <a href='http://{WirelessAP.SoftApIP}'>http://{WirelessAP.SoftApIP}</a></p>" +
                    "</body></html>";

                    WebServer.WebServer.OutPutStream(e.Context.Response, route);

                    // Needed to make sure all is getting out
                    Thread.Sleep(200);

                    // Disable the Soft AP
                    WirelessAP.Disable();
                    Thread.Sleep(200);
                    Power.RebootDevice();
                }
            }
            else
#endif
            {
                string toOutput = "<html><head>" +
                    $"<title>Lego Infrared</title></head><body>" +
                    $"Your Lego Infrared configuraiton is: {(LegoInfrared == null ? "Invalid" : "Valid")}<br/>" +
                    $"Your Serial port configration is: {(_port == null ? "Invalid" : "Valid")}<br/>";
                toOutput += "<a href='test'>Access</a> the Lego Infrared test page.<br>";
                toOutput += "To configure your device please go to <a href=\"config\">configuration</a><br/>";
                toOutput += "Reset your wifi by cliking <a href=\"resetwifi\">here</a>.";
                toOutput += "</body></html>";
                WebServer.WebServer.OutPutStream(e.Context.Response, toOutput);
            }
        }

        private static void SetLegoInfrared()
        {
            if (_legoInfrared != null)
            {
                _legoInfrared.Dispose();
                _legoInfrared = null;
            }

            // On an ESP32, setup first the pins for the SPI
            if (AppConfiguration.SpiClock >= 0)
            {
                Configuration.SetPinFunction(AppConfiguration.SpiClock, DeviceFunction.SPI1_CLOCK);
            }

            if (AppConfiguration.SpiMiso >= 0)
            {
                Configuration.SetPinFunction(AppConfiguration.SpiMiso, DeviceFunction.SPI1_MISO);
            }

            if (AppConfiguration.SpiMosi >= 0)
            {
                Configuration.SetPinFunction(AppConfiguration.SpiMosi, DeviceFunction.SPI1_MOSI);
            }

            try
            {
                _legoInfrared = new LegoInfrared(AppConfiguration.SpiBus, AppConfiguration.SpiChipSelect);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid LegoInfrared configuration: {e.Message}");
            }

            LegoInfraredExecute.LegoInfrared = _legoInfrared;
        }

        private static void SetSerial()
        {
            if (AppConfiguration.SerialCOMNumber >= 1 && AppConfiguration.SerialCOMNumber <= 3)
            {
                switch (AppConfiguration.SerialCOMNumber)
                {
                    case 1:
                        if (AppConfiguration.SerialRx >= 0)
                        {
                            Configuration.SetPinFunction(AppConfiguration.SerialRx, DeviceFunction.COM1_RX);
                        }

                        if (AppConfiguration.SerialTx >= 0)
                        {
                            Configuration.SetPinFunction(AppConfiguration.SerialTx, DeviceFunction.COM1_TX);
                        }

                        break;
                    case 2:
                        if (AppConfiguration.SerialRx >= 0)
                        {
                            Configuration.SetPinFunction(AppConfiguration.SerialRx, DeviceFunction.COM2_RX);
                        }

                        if (AppConfiguration.SerialTx >= 0)
                        {
                            Configuration.SetPinFunction(AppConfiguration.SerialTx, DeviceFunction.COM2_TX);
                        }

                        break;
                    case 3:
                        if (AppConfiguration.SerialRx >= 0)
                        {
                            Configuration.SetPinFunction(AppConfiguration.SerialRx, DeviceFunction.COM3_RX);
                        }

                        if (AppConfiguration.SerialTx >= 0)
                        {
                            Configuration.SetPinFunction(AppConfiguration.SerialTx, DeviceFunction.COM3_TX);
                        }

                        break;
                }

                try
                {
                    _port = new SerialPort($"COM{AppConfiguration.SerialCOMNumber}", 115_200);
                    _port.ReadTimeout = 15_000;
                    _port.WriteTimeout = 1_000;
                    _port.DataReceived += SerialDataReceived;
                    _port.Open();
                }
                catch
                {
                    // No valid serial communication
                }
            }

        }

        private static void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (LegoInfrared == null)
            {
                return;
            }

            try
            {
                string url;
                url = _port.ReadLine(); //.Trim('\n');
                if (string.IsNullOrEmpty(url))
                {
                    return;
                }

                bool res = false;

                if (url.ToLower().StartsWith(ApiController.PageComboAll))
                {
                    res = LegoInfraredExecute.ComboAll(url);
                }
                else if (url.ToLower().StartsWith(ApiController.PageCombo))
                {
                    res = LegoInfraredExecute.Combo(url);
                }
                else if (url.ToLower().StartsWith(ApiController.PageContinuousAll))
                {
                    res = LegoInfraredExecute.ContinuousAll(url);
                }
                else if (url.ToLower().StartsWith(ApiController.PageContinuous))
                {
                    res = LegoInfraredExecute.Continuous(url);
                }
                else if (url.ToLower().StartsWith(ApiController.PageSingleCst))
                {
                    res = LegoInfraredExecute.SingleCst(url);
                }
                else if (url.ToLower().StartsWith(ApiController.PageComboPwmAll))
                {
                    res = LegoInfraredExecute.ComboPwmAll(url);
                }
                else if (url.ToLower().StartsWith(ApiController.PageComboPwm))
                {
                    res = LegoInfraredExecute.ComboPwm(url);
                }
                else if (url.ToLower().StartsWith(ApiController.PageSinglePwmAll))
                {
                    res = LegoInfraredExecute.SinglePwmAll(url);
                }
                else if (url.ToLower().StartsWith(ApiController.PageSinglePwm))
                {
                    res = LegoInfraredExecute.SinglePwm(url);
                }
                else if (url.ToLower().StartsWith(ApiController.PageTimeout))
                {
                    res = LegoInfraredExecute.SingleTimeout(url);
                }

                string ret = res ? "OK\r\n" : "KO\r\n";
                _port.WriteLine(ret);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in serial: {ex}");
            }
        }

        /// <summary>
        /// Get current IP address. Only valid if successfully provisioned and connected
        /// </summary>
        /// <returns>IP address string</returns>
        public static string GetCurrentIPAddress()
        {
            NetworkInterface ni = NetworkInterface.GetAllNetworkInterfaces()[0];

            // get first NI ( Wifi on ESP32 )
            return ni.IPv4Address.ToString();
        }

#if NO_BLUETOOTH
        public static void SetWifiAp()
        {
            Wireless80211.Disable();
            if (WirelessAP.Setup() == false)
            {
                // Reboot device to Activate Access Point on restart
                Console.WriteLine($"Setup Soft AP, Rebooting device");
                Power.RebootDevice();
            }

            _wifiApMode = true;
            var dhcpserver = new DhcpServer
            {
                CaptivePortalUrl = $"http://{WirelessAP.SoftApIP}"
            };
            var dhcpInitResult = dhcpserver.Start(IPAddress.Parse(WirelessAP.SoftApIP), new IPAddress(new byte[] { 255, 255, 255, 0 }));
            if (!dhcpInitResult)
            {
                Console.WriteLine($"Error initializing DHCP server.");
                // This happens after a very freshly flashed device
                Power.RebootDevice();
            }

            Console.WriteLine($"Running Soft AP, waiting for client to connect");
            Console.WriteLine($"Soft AP IP address :{WirelessAP.GetIP()}");
        }
#else
        public static void SetImprove()
        {
            // Construct Improv class
            _imp = new Improv();

            // This optional event is called when the provisioning is completed and Wifi is connected but before
            // improv has informed Improv client of result. This allows user to set the provision URL redirect with correct IP address 
            // See event handler
            _imp.OnProvisioningComplete += OnProvisioningComplete;

            // This optional event will be called to do the Wifi provisioning in user program.
            // if not set then improv class will automatically try to connect to Wifi 
            // For this sample we will let iprov do it, uncomment next line to try user event. See event handler
            // imp.OnProvisioned += Imp_OnProvisioned;

            // Start IMPROV service to start advertising using provided device name.
            _imp.Start("Lego Infrared");

            // You may need a physical button to be pressed to authorise the provisioning (security)
            // Wait for button press and call Authorise method
            // For out test we will just Authorise
            _imp.Authorise(true);

            Console.WriteLine("Waiting for device to be provisioned");

            // Now wait for Device to be Provisioned
            // we could also just use the OnProvisioningComplete event
            while (_imp.CurrentState != Improv.ImprovState.provisioned)
            {
                Thread.Sleep(500);
            }

            Console.WriteLine("Device has been provisioned");

            // We are now provisioned and connected to Wifi, so stop bluetooth service to release resources.
            _imp.Stop();
            _imp = null;

            // Start our very simple web page server to pick up the redirect we gave
            Console.WriteLine("Starting simple web server");
        }

        /// <summary>
        /// Event handler for OnProvisioningComplete event
        /// </summary>
        /// <param name="sender">Improv instance</param>
        /// <param name="e">Not used</param>
        private static void OnProvisioningComplete(object sender, EventArgs e)
        {
            SetProvisioningURL();
        }

        /// <summary>
        /// Set URL with current IP address
        /// The Improv client will redirect to this URL if set.
        /// </summary>
        private static void SetProvisioningURL()
        {
            // All good, wifi connected, set up URL for access
            string ipAddress = GetCurrentIPAddress();
            Console.WriteLine($"IP Address: {ipAddress}");
            _imp.RedirectUrl = "http://" + ipAddress;
        }

        private static void Imp_OnProvisioned(object sender, ProvisionedEventArgs e)
        {
            string ssid = e.Ssid;
            string password = e.Password;

            Console.WriteLine("Provisioning device");

            Console.WriteLine("Connecting to Wifi...");

            // Try to connect to Wifi AP
            // use improv internal method
            if (_imp.ConnectWiFi(ssid, password))
            {
                Console.WriteLine("Connected to Wifi");

                SetProvisioningURL();
            }
            else
            {
                Console.WriteLine("Failed to Connect to Wifi!");

                // if not successful set error and return
                _imp.ErrorState = Improv.ImprovError.unableConnect;
            }
        }
#endif
    }
}
