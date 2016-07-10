namespace RethoughtLib
{
    #region Using Directives

    using System.Collections.Generic;

    using global::RethoughtLib.Classes.Feature;
    using global::RethoughtLib.Classes.Intefaces;

    using LeagueSharp.Common;

    #endregion

    public class RethoughtLib : ILoadable
    {
        #region Static Fields

        private static readonly FeatureParent Root = new Root(GlobalVariables.LibraryMenu);

        private readonly List<IFeatureChild> features = new List<IFeatureChild>()
                                                                   {
                                                                       new global::RethoughtLib.VersionChecker.Version(
                                                                           Root,
                                                                           GlobalVariables
                                                                           .GitHubPath,
                                                                           GlobalVariables
                                                                           .DisplayName)
                                                                   };

        private readonly List<ILoadable> loadables = new List<ILoadable>() { new Events.Events() };

        /// <summary>
        ///     The initialized
        /// </summary>
        private bool initialized;

        #endregion

        #region Constructors and Destructors

        private RethoughtLib()
        {
        }

        static RethoughtLib()
        {
        }

        #endregion

        #region Public Properties

        public static RethoughtLib Instance { get; } = new RethoughtLib();

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; } = "RethoughtLib";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public void Load()
        {
            if (this.initialized) return;

            this.initialized = true;

            CustomEvents.Game.OnGameLoad += this.Game_OnGameLoad;
            
        }

        private void Game_OnGameLoad(System.EventArgs args)
        {
            foreach (var loadable in this.loadables)
            {
                loadable.Load();
            }

            foreach (var feature in this.features)
            {
                feature.HandleEvents();
            }
        }

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        void ILoadable.Load()
        {
            this.Load();
        }

        #endregion
    }
}