// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using Iot.Device.ServoMotor;
using System.Device.Gpio;
using System.Device.Pwm;
using System.Diagnostics;

namespace WebServerAndSerial.Models
{
    public class SwitchManagement : ISwitchManagement
    {
        public const byte MaximumNumberSwotches = 16;

        private bool[] _SwitchStatus;
        private uint _maxAngle;
        private GpioPin[] _outputMiltiplex;
        private GpioController _gpio;
        private bool _shouldDispose = false;
        // create a new servo
        // Rotational Range: 203° 
        // Pulse Cycle: 20 ms 
        // Pulse Width: 540-2470 µs 
        private ServoMotor _switch;

        public SwitchManagement(uint minDur, uint maxDur, int[] multiplexPins, int pwmChip, int pwmChannel, GpioController? gpio = null, bool shouldDispose = true)
        {
            _maxAngle = 180;
            _switch = new ServoMotor(PwmChannel.Create(pwmChip, pwmChannel, 50), _maxAngle, minDur, maxDur);
            _switch.Start();
           
            _SwitchStatus = new bool[MaximumNumberSwotches];

            if (multiplexPins.Length != 4)
            {
                throw new ArgumentException("You need an array of 4 GPIO pins, even is all are -1 (not used).");
            }

            // initialise the outputs based on the number of signals
            _gpio = gpio ?? new GpioController();
            _shouldDispose = gpio == null | shouldDispose;

            // initialize all multiplexer output to Low
            _outputMiltiplex = new GpioPin[multiplexPins.Length];
            for (int i = 0; i < _outputMiltiplex.Length; i++)
            {
                if (multiplexPins[i] >= 0)
                {
                    _outputMiltiplex[i] = _gpio.OpenPin(multiplexPins[i], PinMode.Output);
                    _outputMiltiplex[i].Write(PinValue.Low);
                }
            }

            Debug.Write("All GPIO initialised in Switch");
            // initialise all signals to "false"
            for (byte i = 0; i < MaximumNumberSwotches; i++)
            {
                ChangeSwitch(i, false);
            }

        }

        public void ChangeSwitch(byte NumSignal, bool value)
        {
            if ((NumSignal <= 0) && (NumSignal > MaximumNumberSwotches))
            {
                throw new Exception("Not correct number of Signals");
            }

            // need to convert to select the right Signal
            _SwitchStatus[NumSignal] = value;
            for (int j = 0; j < 4; j++)
            {
                if (((NumSignal >> j) & 1) == 1)
                {
                    if (_outputMiltiplex != null)
                    {
                        _outputMiltiplex[j].Write(PinValue.High);
                    }

                    Debug.WriteLine("Selecting switch: {0} to value {1}", j, PinValue.High);
                }
                else
                {
                    if (_outputMiltiplex != null)
                    {
                        _outputMiltiplex[j].Write(PinValue.Low);
                    }

                    Debug.WriteLine("Selecting switch: {0} to value {1}", j, PinValue.Low);
                }
            }

            // Write the value to the output
            if (value)
            {
                _switch.WriteAngle(_maxAngle);
                Debug.WriteLine($"Angle {_maxAngle}");
            }
            else
            {
                _switch.WriteAngle(0);
                Debug.WriteLine($"Angle 0");
            }
        }

        public bool GetSwitch(byte NumSignal)
        {
            if ((NumSignal <= 0) && (NumSignal > MaximumNumberSwotches))
            {
                throw new Exception("Not correct number of Signals");
            }

            return _SwitchStatus[NumSignal];
        }

        public void Dispose()
        {
            _switch?.Stop();
            _switch?.Dispose();
            _switch = null!;

            for (int i = 0; i < _SwitchStatus.Length; i++)
            {
                if (_gpio != null && _gpio.IsPinOpen(_outputMiltiplex[i].PinNumber))
                {
                    _gpio.ClosePin(_outputMiltiplex[i].PinNumber);
                }
            }

            if (_shouldDispose)
            {
                _gpio?.Dispose();
                _gpio = null!;
            }
        }
    }
}
