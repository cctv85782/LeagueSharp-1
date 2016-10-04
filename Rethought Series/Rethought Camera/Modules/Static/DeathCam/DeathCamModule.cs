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

namespace Rethought_Camera.Modules.Static.DeathCam
{
    #region Using Directives

    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;

    #endregion

    internal abstract class DeathCamModule : Base
    {
        #region Public Properties

        public int MaxPriority { get; set; } = 1;

        public int Priority { get; set; } = 1;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the position.
        /// </summary>
        /// <returns></returns>
        public abstract Vector3 GetPosition();

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            var priority =
                this.Menu.AddItem(
                    new MenuItem(this.Name + "priority", "Priority").SetValue(new Slider(0, 0, this.MaxPriority)));

            this.Priority = priority.GetValue<Slider>().Value;
        }

        #endregion
    }
}