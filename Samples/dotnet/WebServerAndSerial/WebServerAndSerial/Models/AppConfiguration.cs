// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using Lego.Infrared;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebServerAndSerial.Models
{
    public class AppConfiguration
    {
        private static readonly string ConfigName = $".{Path.VolumeSeparatorChar}config{Path.VolumeSeparatorChar}config.json";
        public static AppConfiguration Load()
        {
            AppConfiguration? config = null;
            try
            {
                var str = File.ReadAllText(ConfigName);
                config = JsonSerializer.Deserialize<AppConfiguration>(str);
            }
            catch
            {
                // We swallow it, most likely not configured
            }

            config = config ?? new AppConfiguration();
            config.UpdateConfiguration();
            return config;
        }

        public AppConfiguration()
        {
            SwitchMultiplexPins = new int[4] { -1, -1, -1, -1 };
        }

        public void Save()
        {
            try
            {
                var str = JsonSerializer.Serialize(this);
                File.WriteAllText(ConfigName, str);
            }
            catch
            {
                // We swallow it, most likely not configured
            }
        }

        public void UpdateConfiguration()
        {
            try
            {
                LegoInfrared = new LegoInfrared(InfraredSpiBusNumber, InfraredSpiBusNumber);
            }
            catch
            {
                // Nothing
            }

            try
            {
#if DEBUG
                SignalManagement = new SignalDebug();
#else
                SignalManagement = new SignalManagement(SignalSpiBusNumber, SignalSpiChipSelect);
#endif
            }
            catch
            {
                // Nothing
            }

            try
            {
#if DEBUG
                SwitchManagement = new SwitchDebug();
#else
                SwitchManagement = new SwitchManagement(SwitchMinimumDuration, SwitchMaximumDuration,
                    SwitchMultiplexPins, SwitchPwmChip, SwitchPwmChannel);
#endif
            }
            catch
            {
                // Nothing
            }
        }

        [JsonIgnore]
        public LegoInfrared? LegoInfrared { get; internal set; }

        [JsonIgnore]
        public ISwitchManagement? SwitchManagement { get; internal set; }

        [JsonIgnore]
        public ISignalManagement? SignalManagement { get; internal set; }

        public List<Train> Trains { get; set; } = new List<Train>();

        public List<Signal> Signals { get; set; } = new List<Signal>();

        public List<Switch> Switches { get; set; } = new List<Switch>();

        [Display(Name = "Infrared SPI Bus Number")]
        [Range(0, int.MaxValue, ErrorMessage = "SPI bus needs to be a valid positive number.")]
        public int InfraredSpiBusNumber { get; set; }

        [Display(Name = "Infrared SPI Chip Select")]
        [Range(-1, int.MaxValue, ErrorMessage = "SPI bus needs to be a valid positive number. Set -1 not to use.")]
        public int InfraredSpiChipSelect { get; set; }

        [Display(Name = "Signal SPI Bus Number")]
        [Range(0, int.MaxValue, ErrorMessage = "SPI bus needs to be a valid positive number.")]
        public int SignalSpiBusNumber { get; set; }

        [Display(Name = "Signal SPI Chip Select")]
        public int SignalSpiChipSelect { get; set; }

        [Display(Name = "Servo Maximum Duration in microseconds")]
        [Range(0, uint.MaxValue, ErrorMessage = "The value must be positive.")]
        public uint SwitchMaximumDuration { get; set; }

        [Display(Name = "Servo Minimum Durationin microseconds")]
        [Range(0, uint.MaxValue, ErrorMessage = "The value must be positive.")]
        public uint SwitchMinimumDuration { get; set; }

        [Display(Name = "Pins for the servo multiplexer")]
        public int[] SwitchMultiplexPins { get; set; }

        [Display(Name = "Servo PWM Chip")]
        [Range(0, int.MaxValue, ErrorMessage = "PWM Chip must be a valid positive number.")]
        public int SwitchPwmChip { get; set; }

        [Display(Name = "Servo PWM Channel")]
        [Range(0, int.MaxValue, ErrorMessage = "PWM Channel must be a valid positive number.")]
        public int SwitchPwmChannel { get; set; }
    }
}
