namespace RethoughtLib.Classes.Bootstraps.Abstract_Classes
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp.Common;

    using global::RethoughtLib.Classes.Feature;
    using global::RethoughtLib.Classes.Feature.Presets;
    using global::RethoughtLib.Classes.Intefaces;
    using global::RethoughtLib.VersionChecker;

    #endregion

    internal abstract class LoadableBase : ILoadable
    {
        public Menu RootMenu { get; set; }

        public string GithubPath { get; set; }

        public string AssemblyName { get; set; }

        #region Public Properties

        protected LoadableBase(Menu rootMenu, string githubPath, string assemblyName)
        {
            this.RootMenu = rootMenu;
            this.GithubPath = githubPath;
            this.AssemblyName = assemblyName;
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     The features
        /// </summary>
        private readonly List<IFeatureChild> features = new List<IFeatureChild>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public virtual void Load()
        {
            var assembly = new Assembly(this.RootMenu);

            this.features.AddRange(new List<IFeatureChild>()
                                       {
                                           //new global::RethoughtLib.VersionChecker.VersionChecker(assembly, this.GithubPath, this.AssemblyName)
                                       });

            foreach (var feature in this.features)
            {
                feature.HandleEvents();
            }
        }

        #endregion
    }
}