namespace RethoughtLib.Classes.Bootstraps
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using global::RethoughtLib.Classes.Bootstraps.Abstract_Classes;
    using global::RethoughtLib.Classes.Intefaces;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    public class LeagueSharpAutoBootstrap : PlaySharpBootstrapBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LeagueSharpAutoBootstrap" /> class.
        /// </summary>
        public LeagueSharpAutoBootstrap(List<ILoadable> modules, List<string> additionalStrings = null)
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