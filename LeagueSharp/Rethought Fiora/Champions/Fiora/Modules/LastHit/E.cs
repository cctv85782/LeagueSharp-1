﻿namespace Rethought_Fiora.Champions.Fiora.Modules.LastHit
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Fiora.Champions.Fiora.Modules.Core;
    using Rethought_Fiora.Champions.Fiora.Modules.Core.SpellsModule;

    #endregion

    internal class E : ChildBase
    {
        #region Fields

        private readonly Orbwalking.OrbwalkingMode requiredOrbwalkerMode;

        private readonly SpellsModule spellsModule;

        private readonly OrbwalkerModule orbwalkerModule;

        #endregion

        #region Constructors and Destructors

        public E(Orbwalking.OrbwalkingMode requiredOrbwalkerMode, SpellsModule spellsModule, OrbwalkerModule orbwalkerModule)
        {
            this.requiredOrbwalkerMode = requiredOrbwalkerMode;
            this.spellsModule = spellsModule;
            this.orbwalkerModule = orbwalkerModule;
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
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Orbwalking.AfterAttack -= this.OrbwalkingOnAfterAttack;
        }

        /// <summary>
        ///     Called when [OnEnable]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="featureBaseEventArgs"></param>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Orbwalking.AfterAttack += this.OrbwalkingOnAfterAttack;
        }

        /// <summary>
        ///     Triggers on AfterAttack
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        private void OrbwalkingOnAfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (!unit.IsMe || target == null || this.orbwalkerModule.Orbwalker.ActiveMode != this.requiredOrbwalkerMode
                || !this.spellsModule.Spells[SpellSlot.E].IsReady())
            {
                return;
            }

            if (Math.Abs(target.Health - ObjectManager.Player.GetAutoAttackDamage(target as Obj_AI_Base)) < 1)
            {
                return;
            }

            this.spellsModule.Spells[SpellSlot.E].Cast();
        }

        #endregion
    }
}