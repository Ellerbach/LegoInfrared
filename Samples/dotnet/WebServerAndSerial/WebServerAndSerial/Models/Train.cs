// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using Lego.Infrared;

namespace WebServerAndSerial.Models
{
    public class Train
    {
        public const int MaximumNumberOfTrains = 8;

        public int Id { get; set; }

        public Channel Channel { get; set; }

        public PwmOutput Output { get; set; }

        public string Name { get; set; } = string.Empty;

        public PwmSpeed NominalSpeed { get; set; }
    }
}
