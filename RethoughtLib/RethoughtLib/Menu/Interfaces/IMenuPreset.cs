﻿namespace RethoughtLib.Menu.Interfaces
{
    using LeagueSharp.Common;

    /// <summary>
    ///     Interface that gets used to build new menusets
    /// </summary>
    public interface IMenuPreset
    {
        /// <summary>
        /// Gets or sets the menu.
        /// </summary>
        /// <value>
        /// The menu.
        /// </value>
        Menu Menu { get; set; }

        /// <summary>
        /// Generates this instance.
        /// </summary>
        void Generate();
    }
}
