namespace Rethought_Fiora.Champions.Fiora.Modules.LaneClear
{
    #region Using Directives

    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Fiora.Champions.Fiora.Modules.Core;
    using Rethought_Fiora.Champions.Fiora.Modules.Core.SpellsModule;

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
            Orbwalking.AfterAttack -= OrbwalkingOnAfterAttack;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="featureBaseEventArgs"></param>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Orbwalking.AfterAttack += OrbwalkingOnAfterAttack;
        }

        /// <summary>
        ///     Triggers on AfterAttack
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        private static void OrbwalkingOnAfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (!unit.IsMe || target == null
                || OrbwalkerModule.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear
                || !SpellsModule.Spells[SpellSlot.E].IsReady())
            {
                return;
            }

            var minions = MinionManager.GetMinions(1000, MinionTypes.All, MinionTeam.NotAlly);

            if (!minions.Any())
            {
                return;
            }

            SpellsModule.Spells[SpellSlot.E].Cast();
        }

        #endregion
    }
}