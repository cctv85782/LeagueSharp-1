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
        #region Fields

        /// <summary>
        ///     The orbwalking module
        /// </summary>
        private readonly OrbwalkerModule orbwalkingModule;

        /// <summary>
        ///     The passive logic provider
        /// </summary>
        private readonly PassiveLogicProviderModule passiveLogicProvider;

        /// <summary>
        ///     The spells module
        /// </summary>
        private readonly SpellsModule spellsModule;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="E" /> class.
        /// </summary>
        /// <param name="spellsModule">The spells module.</param>
        /// <param name="orbwalkingModule">The orbwalking module.</param>
        /// <param name="passiveLogicProvider">The passive logic provider.</param>
        public E(
            SpellsModule spellsModule,
            OrbwalkerModule orbwalkingModule,
            PassiveLogicProviderModule passiveLogicProvider)
        {
            this.spellsModule = spellsModule;
            this.orbwalkingModule = orbwalkingModule;
            this.passiveLogicProvider = passiveLogicProvider;
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
            Orbwalking.BeforeAttack -= this.OrbwalkingOnBeforeAttack;
        }

        /// <summary>
        ///     Called when [OnEnable]
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
            if (!unit.IsMe || target == null
                || this.orbwalkingModule.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || !this.spellsModule.Spells[SpellSlot.E].IsReady())
            {
                return;
            }

            this.spellsModule.Spells[SpellSlot.E].Cast();
        }

        /// <summary>
        ///     Triggers on BeforeAttack
        /// </summary>
        /// <param name="args">The <see cref="Orbwalking.BeforeAttackEventArgs" /> instance containing the event data.</param>
        private void OrbwalkingOnBeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if (!args.Unit.IsMe || args.Target == null
                || this.orbwalkingModule.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || !this.spellsModule.Spells[SpellSlot.E].IsReady())
            {
                return;
            }

            var unit = args.Target as Obj_AI_Hero;

            if (unit == null) return;

            if (this.passiveLogicProvider.HasFioraUlt(unit) && unit.IsFacing(ObjectManager.Player))
            {
                this.spellsModule.Spells[SpellSlot.E].Cast();
            }
        }

        #endregion
    }
}