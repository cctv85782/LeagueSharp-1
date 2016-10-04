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

namespace Rethought_Camera
{
    #region Using Directives

    using RethoughtLib.Bootstraps.Implementations;

    #endregion

    /// <summary>
    ///     The entry point
    /// </summary>
    internal class Program
    {
        #region Methods

        /* TODO: GLOBAL LIST >
            - DeathCam
            - Ease In and Ease Out separated instead of only having "Transition: Quadric Ease In and Out" for example. More transitions.
            - QuickSwitch for objectives. And waypoints.
            - Waypoint System. Set some waypoints and let the camera transits to all waypoints in a given order on hotkey.
        */

        /// <summary>
        ///     Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            // TODO: Think about adding multi-versioning, so you can change the version ingame by using a different LoadableBase
            var bootstrap = new LeagueSharpMultiBootstrap();

            bootstrap.AddModule(new RethoughtCameraV1());

            bootstrap.AddString("Version_" + bootstrap.Modules.Count);

            bootstrap.Run();
        }

        #endregion
    }
}