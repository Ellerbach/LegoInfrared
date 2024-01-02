// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using nanoFramework.Json;
using System;
using System.IO;

namespace nanoFramework.WebServerAndSerial.Models
{
    public class AppConfiguration
    {
        private const string FileName = "I:\\config.json";
        private int _spiBus = -1;
        private int _spiChipSelect = -1;
        private int _spiClock = -1;
        private int _spiMosi = -1;
        private int _spiMiso = -1;
        private string _login = string.Empty;
        private string _password = string.Empty;
        private int _serialTx = -1;
        private int _serialRx = -1;
        private int _serialNumber = -1;

        public delegate void ConfigurationUpdated(object sender, ConfigurationEventArgs e);
        public event ConfigurationUpdated OnConfigurationUpdated;

        public static AppConfiguration Load()
        {
            if (!File.Exists(FileName))
            {
                return null;
            }

            string config = File.ReadAllText(FileName);
            var configuration = (AppConfiguration)JsonConvert.DeserializeObject(config, typeof(AppConfiguration));
            return configuration;
        }

        public void Save()
        {
            var config = JsonConvert.SerializeObject(this);
            File.WriteAllText(FileName, config);
        }

        public AppConfiguration()
        {
        }

        public int SpiBus
        {
            get => _spiBus;
            set
            {
                if (value == _spiBus)
                {
                    return;
                }

                _spiBus = value;
                OnConfigurationUpdated?.Invoke(this, new ConfigurationEventArgs(nameof(SpiBus)));
            }
        }

        public int SpiChipSelect
        {
            get => _spiChipSelect;
            set
            {
                if (_spiChipSelect == value)
                {
                    return;
                }

                _spiChipSelect = value;
                OnConfigurationUpdated?.Invoke(this, new ConfigurationEventArgs(nameof(SpiChipSelect)));
            }
        }

        public int SpiClock
        {
            get => _spiClock;
            set
            {
                if (_spiClock == value)
                {
                    return;
                }

                _spiClock = value;
                OnConfigurationUpdated?.Invoke(this, new ConfigurationEventArgs(nameof(SpiClock)));
            }
        }

        public int SpiMosi
        {
            get => _spiMosi;
            set
            {
                if (_spiMosi == value)
                {
                    return;
                }

                _spiMosi = value;
                OnConfigurationUpdated?.Invoke(this, new ConfigurationEventArgs(nameof(SpiMosi)));
            }
        }

        public int SpiMiso
        {
            get => _spiMiso;
            set
            {
                if (_spiMiso == value)
                {
                    return;
                }

                _spiMiso = value;
                OnConfigurationUpdated?.Invoke(this, new ConfigurationEventArgs(nameof(SpiMiso)));
            }
        }

        public int SerialCOMNumber
        {
            get => _serialNumber;
            set
            {
                if (_serialNumber == value)
                {
                    return;
                }

                _serialNumber = value;
                OnConfigurationUpdated?.Invoke(this, new ConfigurationEventArgs(nameof(SerialCOMNumber)));
            }
        }

        public int SerialTx
        {
            get => _serialTx;
            set
            {
                if (_serialTx == value)
                {
                    return;
                }

                _serialTx = value;
                OnConfigurationUpdated?.Invoke(this, new ConfigurationEventArgs(nameof(SerialTx)));
            }
        }   
        
        public int SerialRx
        {
            get => _serialRx;
            set
            {
                if (_serialRx == value)
                {
                    return;
                }

                _serialRx = value;
                OnConfigurationUpdated?.Invoke(this, new ConfigurationEventArgs(nameof(SerialRx)));
            }
        }

        public string Login
        {
            get => _login;
            set
            {
                if (_login == value)
                {
                    return;
                }

                _login = value;
                OnConfigurationUpdated?.Invoke(this, new ConfigurationEventArgs(nameof(Login)));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password == value)
                {
                    return;
                }

                _password = value;
                OnConfigurationUpdated?.Invoke(this, new ConfigurationEventArgs(nameof(Password)));
            }
        }
    }
}
