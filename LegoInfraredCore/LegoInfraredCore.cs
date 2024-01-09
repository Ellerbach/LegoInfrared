// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;
using System.Device.Gpio;
using System.Device.Spi;
using System.Diagnostics;
using System.Threading;

namespace Lego.Infrared
{
    /// <summary>
    /// Represents a Lego Infrared device.
    /// </summary>
    public partial class LegoInfrared : IDisposable
    {
        // IR = 6 time 1 and 0 = 101010101010 
        // ZE = 2 time 0 = 00 = 0
        // MOSI outpout high bit always first 10 = ushort
        private const ushort High = 0xFE00;
        private const ushort Low = 0x0000;
        private uint[] toggle = new uint[] { 0, 0, 0, 0 };
        private SpiDevice _spi;
        private bool _isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="LegoInfrared"/> class.
        /// </summary>
        /// <param name="spiBusNumber">The SPI bus number to use.</param>
        /// <param name="chipSelect">The data command to use. Important: the chip select will be active high.</param>
        public LegoInfrared(int spiBusNumber, int chipSelect)
        {
            // Frequency is 38KHz in the protocol
            float t_carrier = 1 / 38.0f;
            // Reality is that there is a 2us difference in the output as there is always a 2us bit on on SPI using MOSI
            float t_ushort = t_carrier - 2e-3f;
            // Calulate the outpout frenquency. Here = 16/(1/38 -2^-3) = 658KHz
            int freq = (int)(16.0f * 1000 / t_ushort);

            var settings = new SpiConnectionSettings(spiBusNumber, chipSelect)
            {
                ClockFrequency = freq,
                Mode = SpiMode.Mode3,
                ChipSelectLineActiveState = PinValue.High,
            };

#if NANOFRAMEWORK_1_0
            _spi = new SpiDevice(settings);
#else
            _spi = SpiDevice.Create(settings);
#endif
        }

        private bool SendAllMessage(ushort[] nib2, ushort[] nib3, ushort[] nib4, ushort[] nib1)
        {
            bool isvalid = true;
            ushort[] code = new ushort[4];
            for (int i = 0; i < 4; i++)
            {
                code[i] = (ushort)((nib1[i] << 12) | (nib2[i] << 8) | (nib3[i] << 4) | nib4[i]);
            }

            // send 3 times each channel, starting with number 4
            for (uint j = 0; j < 3; j++)
            {
                for (uint i = 0; i < 4; i++)
                {
                    isvalid = SpiSend(code[3 - i]);
                    if (!isvalid)
                    {
                        return isvalid;
                    }
                }

                Thread.Sleep(16);
            }

            // wait 8 * 16ms, we already wait 16 ms in the past one
            Thread.Sleep(112);
            // send another time all channels
            for (uint i = 0; i < 4; i++)
            {
                isvalid = SpiSend(code[i]);
                if (!isvalid)
                {
                    return isvalid;
                }
            }

            // and finally send the last part of the message
            // wait a bit
            Thread.Sleep(80);
            for (uint i = 0; i < 4; i++)
            {
                isvalid = SpiSend(code[i]);
                if (!isvalid)
                {
                    return isvalid;
                }

                Thread.Sleep(32);
            }

            // Toggle every channel
            for (uint i = 0; i < 4; i++)
            {
                toggle[(int)i] = toggle[(int)i] == 0 ? (uint)8 : 0;
            }

            return isvalid;

        }

        private bool SendMessage(ushort nib1, ushort nib2, ushort nib3, ushort nib4, uint channel)
        {
            bool isvalid = true;
            ushort code = (ushort)((nib1 << 12) | (nib2 << 8) | (nib3 << 4) | nib4);
            for (uint i = 0; i < 6; i++)
            {
                MessagePause(channel, i);

                isvalid = SpiSend(code);
                if (!isvalid)
                {
                    return isvalid;
                }
            }

            toggle[(int)channel] = toggle[(int)channel] == 0 ? (uint)8 : 0;
            return isvalid;
        }

        /// <summary>
        /// Sends a combo mode signal to all channels.
        /// </summary>
        /// <param name="blueSpeed">Array of speeds for the blue channel.</param>
        /// <param name="redSpeed">Array of speeds for the red channel.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        public bool ComboModeAll(Speed[] blueSpeed, Speed[] redSpeed)
        {
            if (blueSpeed.Length != 4 || redSpeed.Length != 4)
            {
                throw new ArgumentException("Arrays length must be 4.");
            }

            // prepare all channels
            ushort[] nib1 = new ushort[4];
            ushort[] nib2 = new ushort[4];
            ushort[] nib3 = new ushort[4];
            ushort[] nib4 = new ushort[4];

            // set nibs
            for (int i = 0; i < 4; i++)
            {
                nib1[i] = (ushort)(toggle[i] | (uint)i);
                nib2[i] = (ushort)Mode.ComboDirectMode;
                nib3[i] = (ushort)((uint)blueSpeed[i] | (uint)redSpeed[i]);
                nib4[i] = (ushort)(0xf ^ nib1[i] ^ nib2[i] ^ nib3[i]);
            }

            return SendAllMessage(nib2, nib3, nib4, nib1);
        }

        /// <summary>
        /// Sends a combo mode signal to a specific channel.
        /// </summary>
        /// <param name="blueSpeed">Speed for the blue channel.</param>
        /// <param name="redSpeed">Speed for the red channel.</param>
        /// <param name="channel">The channel to which the signal is sent.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        public bool ComboMode(Speed blueSpeed, Speed redSpeed, Channel channel)
        {
            uint nib1;
            uint nib2;
            uint nib3;
            uint nib4;

            // set nibs
            nib1 = toggle[(uint)channel] | (uint)channel;
            nib2 = (uint)Mode.ComboDirectMode;
            nib3 = (uint)blueSpeed | (uint)redSpeed;
            nib4 = 0xf ^ nib1 ^ nib2 ^ nib3;

            return SendMessage((ushort)nib1, (ushort)nib2, (ushort)nib3, (ushort)nib4, (uint)channel);
        }


        /// <summary>
        /// Sends a PWM signal to all channels.
        /// </summary>
        /// <param name="pwm">Array of PWM speeds.</param>
        /// <param name="output">Array of PWM outputs.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        public bool SingleOutputPwmAll(PwmSpeed[] pwm, PwmOutput[] output)
        {
            if (pwm.Length != 4 || output.Length != 4)
            {
                throw new ArgumentException("Arrays length must be 4.");
            }

            //prepare all channels
            ushort[] nib1 = new ushort[4];
            ushort[] nib2 = new ushort[4];
            ushort[] nib3 = new ushort[4];
            ushort[] nib4 = new ushort[4];

            //set nibs
            for (int i = 0; i < 4; i++)
            {
                nib1[i] = (ushort)(toggle[i] | (uint)i);
                //nib1 = (uint)channel;
                nib2[i] = (ushort)((ushort)Mode.SingleOutputPwm | (ushort)output[i]);
                nib3[i] = (ushort)(pwm[i]);
                nib4[i] = (ushort)(0xf ^ nib1[i] ^ nib2[i] ^ nib3[i]);
            }
            return SendAllMessage(nib2, nib3, nib4, nib1);
        }

        /// <summary>
        /// Sends a PWM signal to a specific channel.
        /// </summary>
        /// <param name="pwm">PWM speed.</param>
        /// <param name="output">PWM output.</param>
        /// <param name="channel">The channel to which the signal is sent.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        public bool SingleOutputPwm(PwmSpeed pwm, PwmOutput output, Channel channel)
        {
            uint nib1;
            uint nib2;
            uint nib3;
            uint nib4;

            //set nibs
            nib1 = toggle[(uint)channel] | (uint)channel;
            nib2 = (uint)Mode.SingleOutputPwm | (uint)output;
            nib3 = (uint)pwm;
            nib4 = 0xf ^ nib1 ^ nib2 ^ nib3;

            return SendMessage((ushort)nib1, (ushort)nib2, (ushort)nib3, (ushort)nib4, (uint)channel);
        }

        /// Sends a PWM signal to a specific channel in a combo configuration.
        /// </summary>
        /// <param name="pwm1">PWM speed for the first channel.</param>
        /// <param name="pwm2">PWM speed for the second channel.</param>
        /// <param name="channel">The channel to which the signal is sent.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        public bool SingleOutputClearSetToggle(ClearSetToggle pwm, PwmOutput output, Channel channel)
        {
            uint nib1;
            uint nib2;
            uint nib3;
            uint nib4;

            // set nibs
            nib1 = toggle[(uint)channel] | (uint)channel;
            nib2 = (uint)Mode.SingleOutputCst | (uint)output;
            nib3 = (uint)pwm;
            nib4 = 0xf ^ nib1 ^ nib2 ^ nib3;

            return SendMessage((ushort)nib1, (ushort)nib2, (ushort)nib3, (ushort)nib4, (uint)channel);
        }

        /// <summary>
        /// Sends a PWM signal to all channels in a combo configuration.
        /// </summary>
        /// <param name="pwm1">Array of PWM speeds for the first set of channels.</param>
        /// <param name="pwm2">Array of PWM speeds for the second set of channels.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        public bool ComboPwmAll(PwmSpeed[] pwm1, PwmSpeed[] pwm2)
        {
            if (pwm1.Length != 4 || pwm2.Length != 4)
            {
                throw new ArgumentException("Arrays length must be 4.");
            }

            // prepare all channels
            ushort[] nib1 = new ushort[4];
            ushort[] nib2 = new ushort[4];
            ushort[] nib3 = new ushort[4];
            ushort[] nib4 = new ushort[4];

            // set nibs
            for (int i = 0; i < 4; i++)
            {
                // set nibs
                nib1[i] = (ushort)(1 << 2 | (uint)i);
                nib2[i] = (ushort)pwm1[i];
                nib3[i] = (ushort)pwm2[i];
                nib4[i] = (ushort)(0xf ^ nib1[i] ^ nib2[i] ^ nib3[i]);
            }

            return SendAllMessage(nib1, nib2, nib3, nib4);
        }

        /// <summary>
        /// Sends a PWM signal to a specific channel in a combo configuration.
        /// </summary>
        /// <param name="pwm1">PWM speed for the first channel.</param>
        /// <param name="pwm2">PWM speed for the second channel.</param>
        /// <param name="channel">The channel to which the signal is sent.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        public bool ComboPwm(PwmSpeed pwm1, PwmSpeed pwm2, Channel channel)
        {
            uint nib1;
            uint nib2;
            uint nib3;
            uint nib4;

            // set nibs
            nib1 = 1 << 2 | (uint)channel;
            nib2 = (uint)pwm1;
            nib3 = (uint)pwm2;
            nib4 = 0xf ^ nib1 ^ nib2 ^ nib3;

            return SendMessage((ushort)nib1, (ushort)nib2, (ushort)nib3, (ushort)nib4, (uint)channel);
        }

        /// <summary>
        /// Sends a continuous signal to all channels.
        /// </summary>
        /// <param name="lfunction">Array of functions indicating behavior for each channel.</param>
        /// <param name="output">Array of outputs for each channel.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        public bool SingleOutputContinuousAll(Function[] lfunction, Output[] output)
        {
            if (lfunction.Length != 4 || output.Length != 4)
            {
                throw new ArgumentException("Arrays length must be 4.");
            }

            //prepare all channels
            ushort[] nib1 = new ushort[4];
            ushort[] nib2 = new ushort[4];
            ushort[] nib3 = new ushort[4];
            ushort[] nib4 = new ushort[4];

            //set nibs
            for (int i = 0; i < 4; i++)
            {
                nib1[i] = (ushort)(toggle[i] | (uint)i);
                //nib1 = (uint)channel;
                nib2[i] = (ushort)Mode.SinglePinContinuous;
                nib3[i] = (ushort)((uint)output[i] << 2 | (uint)lfunction[i]);
                nib4[i] = (ushort)(0xf ^ nib1[i] ^ nib2[i] ^ nib3[i]);
            }
            return SendAllMessage(nib2, nib3, nib4, nib1);
        }

        /// <summary>
        /// Sends a timeout signal to a specific channel.
        /// </summary>
        /// <param name="lfunction">Function.</param>
        /// <param name="output">Output.</param>
        /// <param name="channel">The channel to which the signal is sent.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        public bool SingleOutputContinuous(Function lfunction, Output output, Channel channel)
        {
            uint nib1;
            uint nib2;
            uint nib3;
            uint nib4;

            // set nibs
            nib1 = toggle[(uint)channel] | (uint)channel;
            nib2 = (uint)Mode.SinglePinContinuous;
            nib3 = (uint)output << 2 | (uint)lfunction;
            nib4 = 0xf ^ nib1 ^ nib2 ^ nib3;

            return SendMessage((ushort)nib1, (ushort)nib2, (ushort)nib3, (ushort)nib4, (uint)channel);
        }

        /// <summary>
        /// Sends a timeout signal to a specific channel.
        /// </summary>
        /// <param name="lfunction">Function indicating the behavior during timeout.</param>
        /// <param name="output">Output where the timeout signal is sent.</param>
        /// <param name="channel">The channel to which the signal is sent.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        public bool SingleOutputTimeout(Function lfunction, Output output, Channel channel)
        {
            uint nib1, nib2, nib3, nib4;

            // set nibs
            nib1 = toggle[(uint)channel] | (uint)channel;
            nib2 = (uint)Mode.SinglePinTimeout;
            nib3 = (uint)output << 2 | (uint)lfunction;
            nib4 = 0xf ^ nib1 ^ nib2 ^ nib3;

            return SendMessage((ushort)nib1, (ushort)nib2, (ushort)nib3, (ushort)nib4, (uint)channel);
        }


        private void MessagePause(uint channel, uint count)
        {

            int a = 0;
            if (count == 0)
            {
                // delay for first message
                // (4 - Ch) * Tm
                a = 4 - (int)channel + 1;
            }
            else if (count == 1 || count == 2)
            {
                // next 2 messages
                // 5 * Tm
                a = 5;
            }
            else if (count == 3 || count == 4)
            {
                // last 2 messages
                // (6+2*Ch) * Tm
                a = 5 + ((int)channel + 1) * 2;
            }

            // Tm = 16 ms (in theory 13.7 ms)
            Thread.Sleep(a * 16);
        }

        private int FillStartStop(ushort[] uBuff, int iStart)
        {
            // Bit Start/stop = 6 x IR + 39 x ZE
            return FillBit(uBuff, iStart, 6, 39);
        }

        private int FillHigh(ushort[] uBuff, int iStart)
        {
            // Bit high = 6 x IR + 21 x ZE
            return FillBit(uBuff, iStart, 6, 21);
        }

        private int FillLow(ushort[] uBuff, int iStart)
        {
            // Bit low = 6 x IR + 10 x ZE
            return FillBit(uBuff, iStart, 6, 10);
        }

        private int FillBit(ushort[] uBuff, int iStart, int lBit, int sBit)
        {
            int inc;
            int i = iStart;

            // startstop bit
            for (inc = 0; inc < lBit; inc++)
            {
                uBuff[i] = High;
                i++;
            }
            for (inc = 0; inc < sBit; inc++)
            {
                uBuff[i] = Low;
                i++;
            }
            return i;
        }

        private bool SpiSend(ushort code)
        {
            try
            {
                ushort[] tosend = new ushort[522]; // 522 is the max size of the message to be send
                ushort x = 0x8000;
                int i = 0;

                // Start bit
                i = FillStartStop(tosend, i);

                // encoding the 2 codes
                while (x != 0)
                {
                    if ((code & x) != 0)
                    {
                        i = FillHigh(tosend, i);
                    }
                    else
                    {
                        i = FillLow(tosend, i);
                    }

                    // next bit
                    x >>= 1;
                }

                // stop bit
                i = FillStartStop(tosend, i);

                if (_spi != null)
                {
#if NANOFRAMEWORK_1_0
                    _spi.Write(tosend);
#else
                    // We need a byte buffer
                    byte[] buff = new byte[tosend.Length * 2];
                    Buffer.BlockCopy(tosend, 0, buff, 0, buff.Length);
                    _spi.Write(buff);
#endif
                }
                else
                {
                    Debug.WriteLine("No SPI or PC mode");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("error spi send: " + e.Message);
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            _spi.Dispose();
            _spi = null!;
        }
    }
}
