namespace Rethought_Yorick.YorickV1.Spells
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.FeatureSystem.Switches;

    #endregion

    internal class LastRites : SpellChild
    {
        #region Enums

        /// <summary>
        ///     The current spellstate
        /// </summary>
        public enum SpellState
        {
            LastRite = 1,

            Awakening = 2,

            LastRiteAndAwakening = 3
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the state of the Q spell.
        ///     Watch out:
        /// </summary>
        /// <value>
        ///     The state of Q.
        /// </value>
        public SpellState CurrentSpellState
        {
            get
            {
                // TODO BUFFNAME
                if (ObjectManager.Player.HasBuff("YorickOnHitBuffName") && CanCastAwakening) return SpellState.LastRiteAndAwakening;

                return CanCastAwakening ? SpellState.Awakening : SpellState.LastRite;
            }
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Last Rite";

        /// <summary>
        ///     Gets or sets the spell.
        /// </summary>
        /// <value>
        ///     The spell.
        /// </value>
        public override Spell Spell { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a value indicating whether this instance can cast awakening.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can cast awakening; otherwise, <c>false</c>.
        /// </value>
        private static bool CanCastAwakening => ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Name == "YorickQ2";

        #endregion

        #region Public Methods and Operators

        // TODO
        public float GetDamage(Obj_AI_Base target)
        {
            return 0f;
        }

        /// <summary>
        ///     Gets the heal.
        /// </summary>
        /// <returns></returns>
        public int GetHeal()
        {
            var result = 0;

            if (ObjectManager.Player.Level <= 6)
            {
                result = Till6();
            }

            if (ObjectManager.Player.Level <= 12)
            {
                result = Till6() + Till12();
            }

            if (ObjectManager.Player.Level <= 18)
            {
                result = Till6() + Till12() + Till18();
            }

            if (ObjectManager.Player.HealthPercent < 50)
            {
                result *= 2;
            }

            return result;
        }

        /// <summary>
        ///     Determines whether Q will spawn a grave
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public bool SpawnsGrave(Obj_AI_Base target)
        {
            return target.Health <= this.GetDamage(target);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Spell = new Spell(SpellSlot.Q);
        }

        /// <summary>
        ///     Sets the switch.
        /// </summary>
        protected override void SetSwitch()
        {
            this.Switch = new UnreversibleSwitch(this.Menu);
        }

        /// <summary>
        ///     Gets the heal till 18
        /// </summary>
        /// <returns></returns>
        private static int Till12() => (ObjectManager.Player.Level - 6) * 4;

        /// <summary>
        ///     Gets the heal till 12
        /// </summary>
        /// <returns></returns>
        private static int Till18() => 10 + ObjectManager.Player.Level * 6;

        /// <summary>
        ///     Gets the heal till 6
        /// </summary>
        /// <returns></returns>
        private static int Till6() => 10 + ObjectManager.Player.Level * 2;

        #endregion
    }
}