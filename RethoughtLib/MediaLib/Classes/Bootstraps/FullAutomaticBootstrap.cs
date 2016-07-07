namespace RethoughtLib.Classes.Bootstraps
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Classes.Bootstraps.Abstract_Classes;
    using RethoughtLib.Classes.Bootstraps.Interfaces;
    using RethoughtLib.Classes.Intefaces;

    #endregion

    public sealed class LeagueSharpAutoChampionBootstrap : PlaySharpBootstrapBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LeagueSharpAutoChampionBootstrap"/> class.
        /// </summary>
        public LeagueSharpAutoChampionBootstrap(List<ILoadable> modules, List<string> additionalStrings = null)
        {
            this.Modules = modules;
            this.Strings = additionalStrings;

            CustomEvents.Game.OnGameLoad += delegate(EventArgs args)
                {
                    this.AddString(ObjectManager.Player.ChampionName); 
                    this.Initialize();
                };
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the module.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <exception cref="ArgumentException">There can't be multiple similiar modules in the PlaySharpBootstrap.</exception>
        public override void AddModule(ILoadable module)
        {
            base.AddModule(module);
        }

        /// <summary>
        ///     Adds the module.
        /// </summary>
        /// <param name="modules">the modules</param>
        /// <exception cref="ArgumentException">There can't be multiple similiar modules in the PlaySharpBootstrap.</exception>
        public override void AddModules(IEnumerable<ILoadable> modules)
        {
            base.AddModules(modules);
        }

        /// <summary>
        ///     Adds a string with witch the bootstrap is checking for modules.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void AddString(string value)
        {
            base.AddString(value);
        }

        /// <summary>
        ///     Adds strings with witch the bootstrap is checking for modules.
        /// </summary>
        /// <param name="values">the values</param>
        public override void AddStrings(IEnumerable<string> values)
        {
            base.AddStrings(values);
        }

        /// <summary>
        ///     Compares module names with entries in the strings list. If they match it will load the module.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        ///     Removes the module.
        /// </summary>
        /// <param name="module">The module.</param>
        public override void RemoveModule(ILoadable module)
        {
            base.RemoveModule(module);
        }

        /// <summary>
        ///     Removes a string with witch the bootstrap was checking for modules.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void RemoveString(string value)
        {
            base.RemoveString(value);
        }

        /// <summary>
        ///     Removes strings with witch the bootstrap was checking for modules.
        /// </summary>
        /// <param name="values">the values</param>
        public override void RemoveStrings(IEnumerable<string> values)
        {
            base.RemoveStrings(values);
        }

        #endregion
    }
}