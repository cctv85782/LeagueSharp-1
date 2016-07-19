namespace RethoughtLib.Classes.Bootstraps
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using global::RethoughtLib.Classes.Bootstraps.Abstract_Classes;
    using global::RethoughtLib.Classes.Bootstraps.Interfaces;
    using global::RethoughtLib.Classes.Intefaces;

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

            if (additionalStrings != null)
            {
                this.Strings = additionalStrings;
            }

            CustomEvents.Game.OnGameLoad += delegate(EventArgs args)
                {
                    this.AddString(ObjectManager.Player.ChampionName);
                    this.Initialize();
                };
        }

        #endregion
    }
}