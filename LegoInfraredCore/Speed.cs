// Licensed to the Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

namespace Lego.Infrared
{
    /// <summary>
    /// Represents the different speed states for the red and blue outputs.
    /// </summary>
    public enum Speed
    {
        /// <summary>
        /// Represents the float state for the red output.
        /// </summary>
        RedFloat = 0x0,

        /// <summary>
        /// Represents the forward state for the red output.
        /// </summary>
        RedForward = 0x1,

        /// <summary>
        /// Represents the reverse state for the red output.
        /// </summary>
        RedReverse = 0x2,

        /// <summary>
        /// Represents the break state for the red output.
        /// </summary>
        RedBreak = 0x3,

        /// <summary>
        /// Represents the float state for the blue output.
        /// </summary>
        BlueFloat = 0x0,

        /// <summary>
        /// Represents the forward state for the blue output.
        /// </summary>
        BlueForward = 0x4,

        /// <summary>
        /// Represents the reverse state for the blue output.
        /// </summary>
        BlueReverse = 0x8,

        /// <summary>
        /// Represents the break state for the blue output.
        /// </summary>
        BlueBreak = 0xC
    }
}