namespace Rethought_Irelia.IreliaV1.Mixed
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Irelia.IreliaV1.Spells;

    #endregion

    internal class Q : OrbwalkingChild
    {
        #region Constants

        #endregion

        #region Fields


        /// <summary>
        /// The irelia q
        /// </summary>
        private readonly IreliaQ ireliaQ;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Q" /> class.
        /// </summary>
        /// <param name="ireliaQ">The irelia q.</param>

        public Q(IreliaQ ireliaQ)
        {
            this.ireliaQ = ireliaQ;
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

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Orbwalking.AfterAttack -= this.OrbwalkingOnAfterAttack;

            Game.OnUpdate -= this.OnGameUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Orbwalking.AfterAttack += this.OrbwalkingOnAfterAttack;

            Game.OnUpdate += this.OnGameUpdate;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);
        }

        /// <summary>
        ///     Raises the <see cref="E:GameUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnGameUpdate(EventArgs args)
        {

        }

        /// <summary>
        ///     Triggers after an attack has been issued
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        private void OrbwalkingOnAfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (!this.CheckGuardians()) return;
        }

        #endregion
    }
}