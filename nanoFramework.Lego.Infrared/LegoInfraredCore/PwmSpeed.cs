// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace Lego.Infrared
{
    /// <summary>
    /// Represents the different PWM speed steps.
    /// </summary>
    public enum PwmSpeed
    {
        /// <summary>
        /// Represents the float speed.
        /// </summary>
        Float = 0x0,

        /// <summary>
        /// Represents the first forward speed step.
        /// </summary>
        Forward1 = 0x1,

        /// <summary>
        /// Represents the second forward speed step.
        /// </summary>
        Forward2 = 0x2,

        /// <summary>
        /// Represents the third forward speed step.
        /// </summary>
        Forward3 = 0x3,

        /// <summary>
        /// Represents the fourth forward speed step.
        /// </summary>
        Forward4 = 0x4,

        /// <summary>
        /// Represents the fifth forward speed step.
        /// </summary>
        Forward5 = 0x5,

        /// <summary>
        /// Represents the sixth forward speed step.
        /// </summary>
        Forward6 = 0x6,

        /// <summary>
        /// Represents the seventh forward speed step.
        /// </summary>
        Forward7 = 0x7,

        /// <summary>
        /// Represents the break speed.
        /// </summary>
        Break = 0x8,

        /// <summary>
        /// Represents the seventh reverse speed step.
        /// </summary>
        Reverse7 = 0x9,

        /// <summary>
        /// Represents the sixth reverse speed step.
        /// </summary>
        Reverse6 = 0xA,

        /// <summary>
        /// Represents the fifth reverse speed step.
        /// </summary>
        Reverse5 = 0xB,

        /// <summary>
        /// Represents the fourth reverse speed step.
        /// </summary>
        Reverse4 = 0xC,

        /// <summary>
        /// Represents the third reverse speed step.
        /// </summary>
        Reverse3 = 0xD,

        /// <summary>
        /// Represents the second reverse speed step.
        /// </summary>
        Reverse2 = 0xE,

        /// <summary>
        /// Represents the first reverse speed step.
        /// </summary>
        Reverse1 = 0xf
    }
}