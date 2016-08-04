namespace RethoughtLib.Bootstraps.Implementations
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using global::RethoughtLib.Bootstraps.Abstract_Classes;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    public class LeagueSharpMultiBootstrap : PlaySharpBootstrapBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LeagueSharpMultiBootstrap" /> class.
        /// </summary>
        public LeagueSharpMultiBootstrap(List<LoadableBase> modules = null, List<string> additionalStrings = null)
        {
            if (modules != null)
            {
                this.Modules = modules;
            }

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