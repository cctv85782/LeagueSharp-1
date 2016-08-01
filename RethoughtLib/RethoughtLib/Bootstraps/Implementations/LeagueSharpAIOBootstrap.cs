namespace RethoughtLib.Bootstraps.Implementations
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using global::RethoughtLib.Bootstraps.Abstract_Classes;
    using global::RethoughtLib.Classes.General_Intefaces;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    public class LeagueSharpAioBootstrap : PlaySharpBootstrapBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LeagueSharpAioBootstrap" /> class.
        /// </summary>
        public LeagueSharpAioBootstrap(List<LoadableBase> modules, List<string> additionalStrings = null)
        {
            this.Modules = modules;

            if (additionalStrings != null)
            {
                this.Strings = additionalStrings;
            }

            CustomEvents.Game.OnGameLoad +=
                delegate(EventArgs args) { this.AddString(ObjectManager.Player.ChampionName); };
        }

        #endregion
    }
}