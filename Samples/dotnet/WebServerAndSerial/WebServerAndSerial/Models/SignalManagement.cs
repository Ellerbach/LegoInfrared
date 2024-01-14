// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System.Device.Spi;

namespace WebServerAndSerial.Models
{
    public class SignalManagement : ISignalManagement, IDisposable
    {
        public const byte MaximunNumberSignals = 16;
        private bool[] _signalStatus;
        private SpiDevice _spi;

        public SignalManagement(int spiBusNumber, int chipSelect)
        {
            _signalStatus = new bool[MaximunNumberSignals];
            var settings = new SpiConnectionSettings(spiBusNumber, chipSelect)
            {
                Mode = SpiMode.Mode2,
            };

            _spi = SpiDevice.Create(settings);

            // initialise all signals to "false"
            for (byte i = 0; i < MaximunNumberSignals; i++)
            {
                ChangeSignal(i, false);
            }
        }

        public void ChangeSignal(byte NumSignal, bool value)
        {
            if ((NumSignal <= 0) && (NumSignal > MaximunNumberSignals))
            {
                new ArgumentException("Not correct number of Signals");
            }

            // need to convert to select the right Signal
            _signalStatus[NumSignal] = value;
            // fill the buffer to be sent
            ushort mySign = 0;
            for (ushort i = 0; i < MaximunNumberSignals; i++)
            {
                if (_signalStatus[i])
                {
                    mySign = (ushort)(mySign | (ushort)(1 << i * 2));
                }
                else
                {
                    mySign = (ushort)(mySign | (ushort)(1 << (i * 2 + 1)));
                }
            }

            // send the bytes
            if (_spi != null)
            {
                byte[] buff = new byte[2] { (byte)(mySign >> 8), (byte)(mySign & 0xFF) };
                _spi.Write(buff);
            }
        }

        public bool GetSignal(byte NumSignal)
        {
            if ((NumSignal <= 0) && (NumSignal > MaximunNumberSignals))
            {
                new ArgumentException("Not correct number of Signals");
            }

            return _signalStatus[NumSignal];
        }

        public void Dispose()
        {
            _spi?.Dispose();
            _spi = null!;
        }
    }
}
