namespace Rethought_Twitch.TwitchV1.Combo
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Twitch.TwitchV1.Spells;

    #endregion

    internal class W : OrbwalkingChild
    {
        #region Fields

        /// <summary>
        ///     The irelia q
        /// </summary>
        private readonly TwitchQ twitchQ;

        /// <summary>
        ///     The irelia w
        /// </summary>
        private readonly TwitchW twitchW;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="W" /> class.
        /// </summary>
        /// <param name="twitchW">Irelia W.</param>
        /// <param name="twitchQ">Irelia Q</param>
        public W(TwitchW twitchW, TwitchQ twitchQ)
        {
            this.twitchW = twitchW;
            this.twitchQ = twitchQ;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "W";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Obj_AI_Base.OnProcessSpellCast -= this.OnProcessSpellCast;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Obj_AI_Base.OnProcessSpellCast += this.OnProcessSpellCast;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);
        }

        /// <summary>
        ///     Called when [process spell cast].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!this.CheckGuardians()) return;

            if (!sender.IsMe) return;

            var target = args.Target as Obj_AI_Hero;

            if (target == null || this.twitchQ.WillReset(target)) return;

            if (args.Slot == SpellSlot.Q)
            {
                this.twitchW.Spell.Cast();
            }

            if (args.SData.ConsideredAsAutoAttack || args.SData.IsAutoAttack())
            {
                this.twitchW.Spell.Cast();
            }
        }

        #endregion
    }
}