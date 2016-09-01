namespace Rethought_Yasuo.Yasuo.Modules.Combo
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.CastManager.Abstract_Classes;

    using Rethought_Yasuo.Yasuo.Modules.Core.SpellParent;
    using Rethought_Yasuo.Yasuo.Modules.Core.SpellParent.Implementations;

    using SharpDX;

    #endregion

    internal class NonDashingQ1Module : OrbwalkingChild
    {
        #region Fields

        /// <summary>
        ///     The spells module
        /// </summary>
        private readonly ISpellIndex spellParent;

        /// <summary>
        ///     The q logic provider module
        /// </summary>
        private readonly YasuoQ yasuoQ;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NonDashingQ1Module" /> class.
        /// </summary>
        public NonDashingQ1Module(ISpellIndex spellParent, ICastManager castManager)
            : base(castManager)
        {
            this.yasuoQ = (YasuoQ)spellParent[SpellSlot.Q];

            this.spellParent = spellParent;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Q (while not dashing)";

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the target.
        /// </summary>
        /// <value>
        ///     The target.
        /// </value>
        private Obj_AI_Hero Target
        {
            get
            {
                return TargetSelector.GetTarget(
                    this.spellParent[SpellSlot.Q].Spell.Range,
                    TargetSelector.DamageType.Physical);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Casts the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        public void Cast(Vector3 position)
        {
            if (!position.IsValid())
            {
                return;
            }

            this.spellParent[SpellSlot.Q].Spell.Cast(position);
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
            base.OnLoad(sender, featureBaseEventArgs);
        }

        /// <summary>
        ///     Executes SteelTempest on the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="aoe">if set to <c>true</c> [aoe].</param>
        private void Execute(Obj_AI_Base target, bool aoe = false)
        {
            var predictionOutput = this.spellParent[SpellSlot.Q].Spell.GetPrediction(target, aoe);

            // TODO Menu Entry
            if (predictionOutput.Hitchance < HitChance.High) return;

            this.CastManager.Queque.Enqueue(
                2,
                () => this.spellParent[SpellSlot.Q].Spell.Cast(predictionOutput.CastPosition));
        }

        private void GameOnOnUpdate(EventArgs args)
        {
            if (this.CheckGuardians())
            {
                return;
            }

            this.Execute(this.Target);
        }

        #endregion
    }
}