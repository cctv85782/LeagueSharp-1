namespace RethoughtLib.Classes.Bootstraps.Abstract_Classes
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp.Common;

    using RethoughtLib.Classes.Feature;
    using RethoughtLib.Classes.Feature.Presets;
    using RethoughtLib.Classes.Intefaces;
    using RethoughtLib.VersionChecker;

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
                                           new VersionChecker.Version(assembly, this.GithubPath, this.AssemblyName)
                                       });
            throw new NotImplementedException();
        }

        #endregion
    }
}