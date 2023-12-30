using System;

namespace nanoFramework.WebServerAndSerial.Models
{
    public class ConfigurationEventArgs : EventArgs
    {
        public ConfigurationEventArgs(string paramName)
        {
            ParamName = paramName;
        }

        public string ParamName { get; set; }
    }
}
