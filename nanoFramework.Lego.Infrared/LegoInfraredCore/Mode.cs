// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace Lego.Infrared
{
    /// <summary>
    /// Represents the different modes of operation.
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Represents the extended mode.
        /// </summary>
        Extended = 0x0,

        /// <summary>
        /// Represents the combo direct mode.
        /// </summary>
        ComboDirectMode = 0x1,

        /// <summary>
        /// Represents the single pin continuous mode.
        /// </summary>
        SinglePinContinuous = 0x2,

        /// <summary>
        /// Represents the single pin timeout mode.
        /// </summary>
        SinglePinTimeout = 0x3,

        /// <summary>
        /// Represents the single output PWM mode.
        /// </summary>
        SingleOutputPwm = 0x4,

        /// <summary>
        /// Represents the PWM mode.
        /// </summary>
        Pwm = 0x05,

        /// <summary>
        /// Represents the single output CST mode.
        /// </summary>
        SingleOutputCst = 0x6,
    }
}