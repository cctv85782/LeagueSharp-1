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
        #region Fields

        /// <summary>
        /// The q logic provider module
        /// </summary>
        private readonly QLogicProviderModule qLogicProviderModule;

        /// <summary>
        /// The spells module
        /// </summary>
        private readonly SpellsModule spellsModule;

        /// <summary>
        /// The orbwalker module
        /// </summary>
        private readonly OrbwalkerModule orbwalkerModule;

        private readonly PassiveLogicProviderModule passiveLogicProvider;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Q"/> class.
        /// </summary>
        /// <param name="qLogicProviderModule">The q logic provider module.</param>
        /// <param name="spellsModule">The spells module.</param>
        /// <param name="orbwalkerModule">The orbwalker module.</param>
        public Q(SpellsModule spellsModule, OrbwalkerModule orbwalkerModule, PassiveLogicProviderModule passiveLogicProvider, QLogicProviderModule qLogicProviderModule)
        {
            this.qLogicProviderModule = qLogicProviderModule;
            this.spellsModule = spellsModule;
            this.orbwalkerModule = orbwalkerModule;
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
        public override string Name { get; set; } = "Q";

        #endregion

        #region Properties

        private Obj_AI_Hero Target
            => TargetSelector.GetTarget(this.spellsModule.Spells[SpellSlot.Q].Range, TargetSelector.DamageType.Physical);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Casts the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        public void Cast(Vector3 position)
        {
            if (position.IsValid())
            {
                this.spellsModule.Spells[SpellSlot.Q].Cast(position);
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
            if (!this.spellsModule.Spells[SpellSlot.Q].IsReady()
                || this.orbwalkerModule.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || ObjectManager.Player.IsWindingUp || this.Target == null
                || ObjectManager.Player.HasBuffOfType(BuffType.Blind))
            {
                return;
            }

            var passiveInstance = this.passiveLogicProvider.GetPassiveInstance(this.Target);

            if (passiveInstance != null)
            {
                this.Cast(this.qLogicProviderModule.GetCastPosition(passiveInstance));
            }

            var targetPred = this.spellsModule.Spells[SpellSlot.Q].GetPrediction(this.Target);

            this.Cast(targetPred.UnitPosition);
        }

        #endregion
    }
}