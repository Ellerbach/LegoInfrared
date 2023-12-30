using Lego.Infrared;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Networking;
using nanoFramework.WebServer;
using nanoFramework.WebServerAndSerial.Controllers;
using nanoFramework.WebServerAndSerial.Models;
using System;
using System.Net;
using System.Threading;

namespace nanoFramework.WebServerAndSerial
{
    public class Application
    {
        private static Improv _imp;
        private static AppConfiguration _appConfiguration;
        private static LegoInfrared _legoInfrared;
        public static AppConfiguration AppConfiguration { get => _appConfiguration; }
        public static LegoInfrared LegoInfrared { get => _legoInfrared; }
        private static WebServer.WebServer _server;

        public static void Main()
        {
            Console.WriteLine("Lego Infrared REST API and Serial module");

            // Try to read the configuration
            _appConfiguration = AppConfiguration.Load();
            if (AppConfiguration != null)
            {
                SetLegoInfrared();
            }

            _appConfiguration = AppConfiguration ?? new AppConfiguration();

            if (!WifiNetworkHelper.Reconnect(true, token: new CancellationTokenSource(5_000).Token))
            {
                SetImprove();
            }

            Console.WriteLine($"Connected with wifi credentials. IP Address: {Improv.GetCurrentIPAddress()}");

            _server = new WebServer.WebServer(80, HttpProtocol.Http, new Type[] { typeof(ControllerApi), typeof(ControllerConfiguration) });
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
            else if(e.Context.Request.RawUrl.StartsWith("/favicon.ico"))
            {
                var ico = ResourceWeb.GetBytes(ResourceWeb.BinaryResources.favicon);
                e.Context.Response.ContentType = "image/ico";
                e.Context.Response.ContentLength64 = ico.Length;
                e.Context.Response.OutputStream.Write(ico, 0, ico.Length);
                return;
            }

            string toOutput = "<html><head>" +
    $"<title>Hi from nanoFramework Server</title></head><body>Your Lego Infrared configuraiton is: {(LegoInfrared == null ? "Invalid" : "Valid")}<br/>";
            toOutput += "To configure your device please go to <a href=\"config\">configuration</a><br/>";
            toOutput += "Reset your wifi by cliking <a href=\"resetwifi\">here</a>.";
            toOutput += "</body></html>";
            WebServer.WebServer.OutPutStream(e.Context.Response, toOutput);
        }

        private static void SetLegoInfrared()
        {
            if (_legoInfrared != null)
            {
                _legoInfrared.Dispose();
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
        }

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
            string ipAddress = Improv.GetCurrentIPAddress();
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
    }
}
