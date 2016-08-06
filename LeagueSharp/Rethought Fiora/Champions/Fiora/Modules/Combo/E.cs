namespace Rethought_Fiora.Champions.Fiora.Modules.Combo
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Fiora.Champions.Fiora.Modules.Core;
    using Rethought_Fiora.Champions.Fiora.Modules.Core.SpellsModule;
    using Rethought_Fiora.Champions.Fiora.Modules.LogicProvider.PassiveLogicProvider;

    #endregion

    internal class E : ChildBase
    {
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
            Orbwalking.BeforeAttack -= this.OrbwalkingOnBeforeAttack;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="featureBaseEventArgs"></param>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Orbwalking.AfterAttack += this.OrbwalkingOnAfterAttack;
            Orbwalking.BeforeAttack += this.OrbwalkingOnBeforeAttack;
        }

        /// <summary>
        ///     Triggers on AfterAttack
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        private void OrbwalkingOnAfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (!unit.IsMe || target == null || OrbwalkerModule.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || !SpellsModule.Spells[SpellSlot.E].IsReady())
            {
                return;
            }

            SpellsModule.Spells[SpellSlot.E].Cast();
        }

        /// <summary>
        ///     Triggers on BeforeAttack
        /// </summary>
        /// <param name="args">The <see cref="Orbwalking.BeforeAttackEventArgs" /> instance containing the event data.</param>
        private void OrbwalkingOnBeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if (!args.Unit.IsMe || args.Target == null
                || OrbwalkerModule.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || !SpellsModule.Spells[SpellSlot.E].IsReady())
            {
                return;
            }

            var unit = args.Target as Obj_AI_Hero;

            if (unit == null) return;

            if (PassiveLogicProviderModule.HasFioraUlt(unit) && unit.IsFacing(ObjectManager.Player))
            {
                SpellsModule.Spells[SpellSlot.E].Cast();
            }
        }

        #endregion
    }
}