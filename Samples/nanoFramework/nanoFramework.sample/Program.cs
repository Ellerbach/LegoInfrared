// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using Lego.Infrared;
using nanoFramework.Hardware.Esp32;
using System;
using System.Diagnostics;
using System.Threading;

namespace nanoFramework.sample
{
    public class Program
    {
        private static LegoInfrared _infrared;

        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework!");

            // On an ESP32, setup first the pins for the SPI
            Configuration.SetPinFunction(14, DeviceFunction.SPI1_CLOCK);
            Configuration.SetPinFunction(12, DeviceFunction.SPI1_MISO);
            Configuration.SetPinFunction(13, DeviceFunction.SPI1_MOSI);

            // We will use chip select 17
            _infrared = new LegoInfrared(1, 17);

            _infrared.ComboMode(Speed.BlueForward, Speed.RedForward, Channel.Channel1);

            _infrared.SingleOutputContinuousAll(
                new Function[] { Function.Set, Function.Clear, Function.Toggle, Function.NoChange },
                new Output[] { Output.BluePinC2, Output.RedPinC2, Output.RedPinC1, Output.BluePinC1 });

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
