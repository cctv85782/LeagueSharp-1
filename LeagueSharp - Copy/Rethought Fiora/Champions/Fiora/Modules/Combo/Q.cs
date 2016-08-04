namespace Rethought_Fiora.Champions.Fiora.Modules.Combo
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Fiora.Champions.Fiora.Modules.Core;
    using Rethought_Fiora.Champions.Fiora.Modules.Core.SpellsModule;
    using Rethought_Fiora.Champions.Fiora.Modules.LogicProvider;
    using Rethought_Fiora.Champions.Fiora.Modules.LogicProvider.PassiveLogicProvider;

    using SharpDX;

    #endregion

    internal class Q : ChildBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Q";

        #endregion

        #region Properties

        private Obj_AI_Hero Target
            => TargetSelector.GetTarget(SpellsModule.Spells[SpellSlot.Q].Range, TargetSelector.DamageType.Physical);

        #endregion

        #region Public Methods and Operators

        public static void Cast(Vector3 position)
        {
            if (position.IsValid())
            {
                SpellsModule.Spells[SpellSlot.Q].Cast(position);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            throw new NotImplementedException();
        }

        private void GameOnOnUpdate(EventArgs args)
        {
            if (!SpellsModule.Spells[SpellSlot.Q].IsReady()
                || OrbwalkerModule.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || ObjectManager.Player.IsWindingUp
                || this.Target == null)
            {
                return;
            }

            var passiveInstance = PassiveLogicProviderModule.GetPassiveInstance(this.Target);

            if (passiveInstance != null)
            {
                Cast(QLogicProviderModule.GetCastPosition(passiveInstance));
            }
        }

        #endregion
    }
}