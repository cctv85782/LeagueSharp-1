﻿namespace Rethought_Twitch.TwitchV1.LastHit
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Twitch.TwitchV1.Spells;

    #endregion

    internal class E : OrbwalkingChild
    {
        #region Fields

        /// <summary>
        ///     The irelia e
        /// </summary>
        private readonly TwitchE twitchE;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="E" /> class.
        /// </summary>
        /// <param name="twitchE">The irelia e.</param>
        public E(TwitchE twitchE)
        {
            this.twitchE = twitchE;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "E";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Orbwalking.AfterAttack -= this.OrbwalkingOnAfterAttack;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Orbwalking.AfterAttack += this.OrbwalkingOnAfterAttack;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);
        }

        /// <summary>
        ///     Triggers after attack
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void OrbwalkingOnAfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            var obj = target as Obj_AI_Base;

            if (obj == null) return;

            if (target.Health <= this.twitchE.GetDamage(obj))
            {
                this.twitchE.Spell.Cast(obj);
            }
        }

        #endregion
    }
}