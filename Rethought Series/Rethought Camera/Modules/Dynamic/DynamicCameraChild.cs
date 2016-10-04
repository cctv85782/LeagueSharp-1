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

namespace Rethought_Camera.Modules.Dynamic
{
    #region Using Directives

    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;

    #endregion

    internal abstract class DynamicCameraChild : Base
    {
        #region Public Properties

        public bool InternalEnabled { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the position.
        /// </summary>
        /// <returns></returns>
        public virtual Vector3 GetPosition()
        {
            if (!this.InternalEnabled) return Vector3.Zero;
            return Vector3.Zero;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Lerps the specified start.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="percent">The percent.</param>
        /// <returns></returns>
        protected Vector3 Lerp(Vector3 start, Vector3 end, float percent)
        {
            return start + percent * (end - start);
        }

        protected void ProcessKeybind(OnValueChangeEventArgs args)
        {
            if (args.GetNewValue<KeyBind>().Active) this.InternalEnabled = true;
            else this.InternalEnabled = false;
        }

        #endregion
    }
}