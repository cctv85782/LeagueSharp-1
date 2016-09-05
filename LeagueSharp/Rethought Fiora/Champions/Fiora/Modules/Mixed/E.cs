namespace Rethought_Fiora.Champions.Fiora.Modules.Mixed
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Fiora.Champions.Fiora.Modules.Core;
    using Rethought_Fiora.Champions.Fiora.Modules.Core.SpellsModule;

    #endregion

    internal class E : ChildBase
    {
        #region Fields

        private readonly OrbwalkerModule orbwalkerModule;

        private readonly SpellsModule spellsModule;

        #endregion

        #region Constructors and Destructors

        public E(SpellsModule spellsModule, OrbwalkerModule orbwalkerModule)
        {
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
            if (!unit.IsMe || target == null
                || this.orbwalkerModule.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Mixed
                || !this.spellsModule.Spells[SpellSlot.E].IsReady())
            {
                return;
            }

            if (!(target is Obj_AI_Hero))
            {
                return;
            }

            this.spellsModule.Spells[SpellSlot.E].Cast();
        }

        #endregion
    }
}