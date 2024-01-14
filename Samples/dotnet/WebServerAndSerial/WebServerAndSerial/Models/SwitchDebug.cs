namespace WebServerAndSerial.Models
{
    public class SwitchDebug : ISwitchManagement
    {
        public const byte MaxNumberSwitches = 16;
        public bool[] _switches = new bool[MaxNumberSwitches];

        public void ChangeSwitch(byte NumSignal, bool value)
        {
            _switches[NumSignal] = value;
        }

        public void Dispose()
        {
            // nothing
        }

        public bool GetSwitch(byte NumSignal)
        {
            return _switches[NumSignal];
        }
    }
}
