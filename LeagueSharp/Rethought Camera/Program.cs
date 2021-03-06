﻿namespace Rethought_Camera
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