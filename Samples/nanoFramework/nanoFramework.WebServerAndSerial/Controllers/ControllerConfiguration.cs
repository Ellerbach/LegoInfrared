using nanoFramework.WebServer;
using nanoFramework.WebServerAndSerial.Models;
using System;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;

namespace nanoFramework.WebServerAndSerial.Controllers
{
    [Authentication("Basic")]
    internal class ControllerConfiguration
    {
        [Route("config")]
        public void Config(WebServerEventArgs e)
        {
            // We need to clean things to get some memory
            Runtime.Native.GC.Run(true);

            // TODO: check the basic authentication
            string route = "<!DOCTYPE html><html><head><title>Configuration</title></head><body><form action=\"process\" method=\"post\">";
            e.Context.Response.ContentType = "text/html";
            // It's the moment to create a new configuration
            var config = Application.AppConfiguration ?? new AppConfiguration();
            var methods = config.GetType().GetMethods();
            foreach (MethodInfo method in methods)
            {
                if (method.Name.StartsWith("get_"))
                {
                    string name = method.Name.Substring(4);
                    var paramType = method.ReturnType;
                    string type;
                    switch (paramType.FullName)
                    {
                        case "System.Int32":
                            type = "number";
                            break;
                        default:
                            type = "string";
                            break;
                    }

                    route += $"<label for=\"{name}\">{name}:</label><input type=\"{type}\" id=\"{name}\" name=\"{name}\" value=\"{method.Invoke(config, null)}\"><br>";
                }
            }

            // We need to clean things to get some memory
            Runtime.Native.GC.Run(true);

            route += "<input type=\"submit\" value=\"Submit\"></form></body></html>";
            WebServer.WebServer.OutPutStream(e.Context.Response, route);
        }

        [Route("process")]
        [Method("POST")]
        public void Process(WebServerEventArgs e)
        {
            byte[] buff = new byte[e.Context.Request.ContentLength64];
            e.Context.Request.InputStream.Read(buff, 0, buff.Length);
            string paramString = Encoding.UTF8.GetString(buff, 0, buff.Length);

            // We're adding back the question mark as it's not present when posting
            var parameters = WebServer.WebServer.DecodeParam($"{WebServer.WebServer.ParamStart}{paramString}");
            // It's the moment to create a new configuration
            var config = Application.AppConfiguration ?? new AppConfiguration();

            foreach (UrlParameter param in parameters)
            {
                var memberPropSetMethod = config.GetType().GetMethod("set_" + param.Name);
                if (memberPropSetMethod != null)
                {
                    var setter = memberPropSetMethod.GetParameters()[0];
                    switch (setter.ParameterType.FullName)
                    {
                        case "System.Int32":
                            int val = int.Parse(param.Value);
                            memberPropSetMethod.Invoke(config, new object[] { val });
                            break;
                        case "System.String":
                            
                            memberPropSetMethod.Invoke(config, new object[] { HttpUtility.UrlDecode(param.Value) });
                            break;
                        default:
                            break;
                    }
                }
            }

            // We need to clean things to get some memory
            Runtime.Native.GC.Run(true);
            config.Save();
            string route = $"<!DOCTYPE html><html><head><title>Configuration Page</title></head><body>Configuration saved and updated. Return to the <a href=\"http://{Improv.GetCurrentIPAddress()}\">home page</a>.</body></html>";
            WebServer.WebServer.OutPutStream(e.Context.Response, route);
        }

        [Route("resetwifi")]
        public void ResetWifi(WebServerEventArgs e)
        {            
            string route = $"<!DOCTYPE html><html><head><title>Configuration Page</title></head><body>Go to the <a href=\"https://www.improv-wifi.com\">Improv-Wifi page</a> and pear your device.</body></html>";
            WebServer.WebServer.OutPutStream(e.Context.Response, route);
            Application.SetImprove();
        }
    }
}
