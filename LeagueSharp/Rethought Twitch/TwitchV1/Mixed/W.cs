﻿namespace Rethought_Twitch.TwitchV1.Mixed
{
    #region Using Directives

    using System;

    using LeagueSharp;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Twitch.TwitchV1.Spells;

    #endregion

    internal class W : OrbwalkingChild
    {
        #region Fields

        /// <summary>
        ///     The irelia w
        /// </summary>
        private readonly TwitchW twitchW;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="W" /> class.
        /// </summary>
        /// <param name="twitchW">The irelia w.</param>
        public W(TwitchW twitchW)
        {
            this.twitchW = twitchW;
        }

        #endregion

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
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Game.OnUpdate -= this.OnGameUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Game.OnUpdate += this.OnGameUpdate;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);
        }

        /// <summary>
        ///     Raises the <see cref="E:GameUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnGameUpdate(EventArgs args)
        {
        }

        #endregion
    }
}