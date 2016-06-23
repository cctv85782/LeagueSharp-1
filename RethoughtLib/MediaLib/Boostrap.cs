namespace RethoughtLib
{
    #region Using Directives

    using System.Collections.Generic;

    using RethoughtLib.CastManager;
    using RethoughtLib.Classes.Feature;

    #endregion

    internal class Boostrap
    {
        #region Static Fields

        /// <summary>
        ///     The initialized
        /// </summary>
        private static bool initialized;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Initializes the specified arguments.
        /// </summary>
        public static void Initialize()
        {
            if (initialized) return;

            initialized = true;

            GlobalVariables.LibraryMenu = new LeagueSharp.Common.Menu("RethoughtLib", "RethoughtLib", true);

            var root = new Root(GlobalVariables.LibraryMenu);

            var features = new List<IFeatureChild>() { new CastManagerMenu(root) };

            foreach (var feature in features)
            {
                feature.HandleEvents();
            }
        }

        #endregion
    }
}