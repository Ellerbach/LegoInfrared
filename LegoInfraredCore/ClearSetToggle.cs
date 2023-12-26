// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace Lego.Infrared
{
    /// <summary>
    /// Represents the modes for single output CST on a Lego Infrared device.
    /// </summary>
    public enum ClearSetToggle
    {
        /// <summary>
        /// Clear channel 1 and channel 2.
        /// </summary>
        ClearC1ClearC2 = 0x0,

        /// <summary>
        /// Set channel 1 and clear channel 2.
        /// </summary>
        SetC1ClearC2 = 0x1,

        /// <summary>
        /// Clear channel 1 and set channel 2.
        /// </summary>
        ClearC1SetC2 = 0x2,

        /// <summary>
        /// Set channel 1 and channel 2.
        /// </summary>
        SetC1SetC2 = 0x3,

        /// <summary>
        /// Increment PWM.
        /// </summary>
        IncrementPwm = 0x4,

        /// <summary>
        /// Decrement PWM.
        /// </summary>
        DecrementPwm = 0x5,

        /// <summary>
        /// Full forward.
        /// </summary>
        FullForward = 0x6,

        /// <summary>
        /// Full backward.
        /// </summary>
        FullBackward = 0x7,

        /// <summary>
        /// Toggle between full forward and full backward.
        /// </summary>
        ToggleFullForwardBackward = 0x8,
    }
}