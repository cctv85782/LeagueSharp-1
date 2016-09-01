﻿namespace RethoughtLibTest.Champions.Unknown.Parents
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal sealed class OrbwalkingParent : ParentBase
    {
        #region Fields

        /// <summary>
        /// The orbwalker
        /// </summary>
        private readonly Orbwalking.Orbwalker orbwalker;

        /// <summary>
        /// The orbwalking mode
        /// </summary>
        private readonly Orbwalking.OrbwalkingMode orbwalkingMode;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrbwalkingParent"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="orbwalker">The orbwalker.</param>
        /// <param name="orbwalkingMode">The orbwalking mode.</param>
        public OrbwalkingParent(string name, Orbwalking.Orbwalker orbwalker, Orbwalking.OrbwalkingMode orbwalkingMode)
        {
            this.Name = name;
            this.orbwalker = orbwalker;
            this.orbwalkingMode = orbwalkingMode;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnDisable(sender, featureBaseEventArgs);

            Game.OnUpdate -= this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnEnable(sender, featureBaseEventArgs);

            Game.OnUpdate += this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Triggers on GameUpdate
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void GameOnOnUpdate(EventArgs args)
        {
            if (this.orbwalker.ActiveMode != this.orbwalkingMode)
            {
                foreach (var child in this.Children.Where(x => x.Value))
                {
                    child.Key.Switch.OnOnDisableEvent(new FeatureBaseEventArgs(this));
                }
            }
            else
            {
                foreach (var child in this.Children.Where(x => !x.Value))
                {
                    child.Key.Switch.OnOnEnableEvent(new FeatureBaseEventArgs(this));
                }
            }
        }

        #endregion
    }
}