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

namespace Rethought_Camera.Modules.Force
{
    internal class Force
    {
        #region Constructors and Destructors

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Force" /> class.
        /// </summary>
        /// <param name="mass">The mass.</param>
        /// <param name="acceleration">The acceleration.</param>
        public Force(double mass, double acceleration)
        {
            this.Mass = mass;
            this.Acceleration = acceleration;
        }

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the acceleration.
        /// </summary>
        /// <value>
        ///     The acceleration.
        /// </value>
        public double Acceleration { get; set; }

        /// <summary>
        ///     Gets or sets the mass.
        /// </summary>
        /// <value>
        ///     The mass.
        /// </value>
        public double Mass { get; set; }

        /// <summary>
        ///     Gets the result.
        /// </summary>
        /// <value>
        ///     The result.
        /// </value>
        public double Result => this.Mass * this.Acceleration;

        #endregion
    }
}