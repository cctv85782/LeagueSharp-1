﻿namespace Rethought_Fiora.Champions.Fiora.Modules.Combo
{
    #region Using Directives

    using System;

    using LeagueSharp;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class W : ChildBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "W";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [OnEnable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            throw new NotImplementedException();
        }

        private void GameOnOnUpdate(EventArgs args)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}