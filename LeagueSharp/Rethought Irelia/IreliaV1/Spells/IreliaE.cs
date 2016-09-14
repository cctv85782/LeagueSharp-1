namespace Rethought_Irelia.IreliaV1.Spells
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.FeatureSystem.Switches;

    using Rethought_Irelia.IreliaV1.DamageCalculator;

    #endregion

    internal class IreliaE : SpellChild, IDamageCalculatorModule
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the estimated amount in one combo.
        /// </summary>
        /// <value>
        ///     The estimated amount in one combo.
        /// </value>
        public int EstimatedAmountInOneCombo { get; } = 1;

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Equilibrium Strike";

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
        ///     Determines whether this instance can stun the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public bool CanStun(Obj_AI_Base target)
        {
            if (this.Menu.Item("usehealthprediction").GetValue<bool>())
            {
                var predictedEnemyHealth = HealthPrediction.GetHealthPrediction(
                    target,
                    (int)
                    (this.Spell.Delay
                     + ObjectManager.Player.ServerPosition.Distance(target.ServerPosition) / this.Spell.Speed) / 1000,
                    0);

                var playerpredictedHealth = HealthPrediction.GetHealthPrediction(
                    ObjectManager.Player,
                    (int)
                    (this.Spell.Delay
                     + ObjectManager.Player.ServerPosition.Distance(target.ServerPosition) / this.Spell.Speed) / 1000,
                    0);

                return predictedEnemyHealth / target.MaxHealth <= playerpredictedHealth / ObjectManager.Player.MaxHealth;
            }
            else
            {
                return target.HealthPercent <= ObjectManager.Player.HealthPercent;
            }
        }

        /// <summary>
        ///     Gets the damage.
        /// </summary>
        /// <param name="target">The get damage.</param>
        /// <returns></returns>
        public float GetDamage(Obj_AI_Base target)
        {
            return this.Spell.GetDamage(target);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Spell = new Spell(SpellSlot.E, 425);
            this.Spell.SetTargetted(0.2f, 200);

            this.Menu.AddItem(new MenuItem("usehealthprediction", "Use HealthPrediction").SetValue(true));
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