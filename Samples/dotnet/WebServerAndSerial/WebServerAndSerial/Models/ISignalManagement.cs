// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace WebServerAndSerial.Models
{
    public interface ISignalManagement
    {
        void ChangeSignal(byte NumSignal, bool value);
        void Dispose();
        bool GetSignal(byte NumSignal);
    }
}