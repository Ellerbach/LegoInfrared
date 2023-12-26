// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace Lego.Infrared
{
    /// <summary>
    /// Represents the continuous and timeout modes for a Lego Infrared device.
    /// </summary>
    public enum Function
    {
        /// <summary>
        /// Represents no change in the current state.
        /// </summary>
        NoChange = 0x0,

        /// <summary>
        /// Represents a state where the current function is cleared.
        /// </summary>
        Clear = 0x1,

        /// <summary>
        /// Represents a state where a function is set.
        /// </summary>
        Set = 0x2,

        /// <summary>
        /// Represents a state where the current function is toggled.
        /// </summary>
        Toggle = 0x3
    }
}