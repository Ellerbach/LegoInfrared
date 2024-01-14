namespace WebServerAndSerial.Models
{
    public class SignalDebug : ISignalManagement
    {
        public const byte MaximumSignals = 16;
        private bool[] _signals = new bool[MaximumSignals];

        public void ChangeSignal(byte NumSignal, bool value)
        {
            _signals[NumSignal] = value;
        }

        public void Dispose()
        {
            // nothing
        }

        public bool GetSignal(byte NumSignal)
        {
            return _signals[NumSignal];
        }
    }
}
