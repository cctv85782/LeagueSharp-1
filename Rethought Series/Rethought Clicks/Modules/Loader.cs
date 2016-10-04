//     Copyright (C) 2016 Rethought
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
//     Created: 04.10.2016 1:05 PM
//     Last Edited: 04.10.2016 1:44 PM

namespace Rethought_Clicks.Modules
{
    #region Using Directives

    using System.Collections.Generic;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    #endregion

    /// <summary>
    ///     The loader.
    /// </summary>
    internal class Loader : LoadableBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        public override string DisplayName { get; set; } = "Rethought Clicks";

        /// <summary>
        ///     Gets or sets the name of the internal.
        /// </summary>
        /// <value>
        ///     The name of the internal.
        /// </value>
        public override string InternalName { get; set; } = "Rethought_clicks_V1";

        /// <summary>
        ///     Gets or sets the tags.
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public override IEnumerable<string> Tags { get; set; } = new[] { "Version_1" };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Loads this instance
        /// </summary>
        public override void Load()
        {
            var superParent = new SuperParent(this.DisplayName);

            var clickModule = new ClickObserver();

            superParent.Add(clickModule);

            superParent.Load();
        }

        #endregion
    }
}