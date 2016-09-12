namespace Rethought_Yorick.YorickV1.Mixed
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Yorick.YorickV1.Spells;

    #endregion

    internal class E : OrbwalkingChild
    {
        #region Fields

        /// <summary>
        ///     The mourning mist
        /// </summary>
        private readonly MourningMist mourningMist;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="E" /> class.
        /// </summary>
        /// <param name="mourningMist">The mourning mist.</param>
        public E(MourningMist mourningMist)
        {
            this.mourningMist = mourningMist;
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
            if (!this.CheckGuardians()) return;

            var target = TargetSelector.GetTarget(
                this.mourningMist.Spell.Range,
                TargetSelector.DamageType.Magical,
                false);

            this.mourningMist.Spell.GetPrediction(target, true, collisionable: new[] { CollisionableObjects.Walls });
        }

        #endregion
    }
}