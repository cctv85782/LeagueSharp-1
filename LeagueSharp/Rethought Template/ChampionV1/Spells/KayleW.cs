namespace Rethought_Kayle.KayleV1.Spells
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.FeatureSystem.Switches;

    using Rethought_Kayle.KayleV1.DamageCalculator;
    using Rethought_Kayle.KayleV1.Drawings;

    using SharpDX;

    #endregion

    internal class KayleW : SpellChild, IDamageCalculatorModule
    {
        #region Public Properties

        public Color Color { get; set; } = Color.Yellow;

        /// <summary>
        ///     Gets the estimated amount in one combo.
        /// </summary>
        /// <value>
        ///     The estimated amount in one combo.
        /// </value>
        public int EstimatedAmountInOneCombo { get; set; } = 0;

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Divine Blessing";

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
        ///     Gets the damage.
        /// </summary>
        /// <param name="target">The get damage.</param>
        /// <returns></returns>
        public float GetDamage(Obj_AI_Base target)
        {
            // Spell deals no dmg
            return 0;
        }

        /// <summary>
        /// Gets the heal.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public float GetHeal(Obj_AI_Base target)
        {
            return this.Spell.IsReady()
                       ? 15 + this.Spell.Level * 45 + ObjectManager.Player.TotalMagicalDamage * 0.45f : 0f;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Spell = new Spell(SpellSlot.W, 900);
            this.Spell.SetTargetted(0, float.MaxValue);
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