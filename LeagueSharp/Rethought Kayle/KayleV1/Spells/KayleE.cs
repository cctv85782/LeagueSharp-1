﻿namespace Rethought_Kayle.KayleV1.Spells
{
    #region Using Directives

    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.FeatureSystem.Switches;

    using Rethought_Kayle.KayleV1.DamageCalculator;

    #endregion

    internal class KayleE : SpellChild, IDamageCalculatorModule
    {
        #region Constants

        /// <summary>
        ///     The splash damage range
        /// </summary>
        public const int SplashDamageRange = 150;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the estimated amount in one combo.
        /// </summary>
        /// <value>
        ///     The estimated amount in one combo.
        /// </value>
        public int EstimatedAmountInOneCombo { get; set; } = 1;

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Righteous Fury";

        /// <summary>
        ///     Gets or sets the spell.
        /// </summary>
        /// <value>
        ///     The spell.
        /// </value>
        public override Spell Spell { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the aoe damage.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public float GetAoeDamage(Obj_AI_Base target)
        {
            if (!this.Spell.IsReady()) return 0;

            var units =
                ObjectManager.Get<Obj_AI_Base>()
                    .Where(
                        x =>
                        (x.IsMinion || x.IsChampion() || x.PlayerControlled) && !x.IsDead
                        && x.Distance(target) <= SplashDamageRange && target != x);

            return this.GetDamage(target)
                   + units.Sum(
                       unit =>
                       (float)
                       ObjectManager.Player.CalcDamage(unit, Damage.DamageType.Magical, this.GetSplashDamage(unit)));
        }

        /// <summary>
        ///     Gets the damage.
        /// </summary>
        /// <param name="target">The get damage.</param>
        /// <returns></returns>
        public float GetDamage(Obj_AI_Base target)
        {
            // TODO: should return the bonus magic damage (?)
            return !this.Spell.IsReady()
                       ? 0
                       : 5
                         + (float)
                           ObjectManager.Player.CalcDamage(
                               target,
                               Damage.DamageType.Magical,
                               this.Spell.Level * 5 + ObjectManager.Player.TotalMagicalDamage * 0.15);
        }

        /// <summary>
        ///     Gets the splash damage to a unit
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public float GetSplashDamage(Obj_AI_Base target)
        {
            if (!this.Spell.IsReady()) return 0;

            return 10 + this.Spell.Level * 10 + ObjectManager.Player.TotalMagicalDamage * 0.3f
                   + ObjectManager.Player.TotalAttackDamage * ((15 + this.Spell.Level * 5) / 100f);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Spell = new Spell(SpellSlot.E, 525, TargetSelector.DamageType.Magical);
            this.Spell.SetTargetted(0.1f, 2500);
        }

        /// <summary>
        ///     Sets the switch.
        /// </summary>
        protected override void SetSwitch()
        {
            this.Switch = new UnreversibleSwitch(this.Menu);
        }

        #endregion
    }
}