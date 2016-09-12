namespace Rethought_Yorick.YorickV1.Modules
{
    #region Using Directives

    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    /// <summary>
    ///     A simple FeatureSystem implementation containing an instance of an Orbwalker.
    /// </summary>
    /// <seealso cref="RethoughtLib.FeatureSystem.Abstract_Classes.Base" />
    internal class OrbwalkingModule : Base
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Orbwalker";

        /// <summary>
        ///     Gets or sets the orbwalker instance.
        /// </summary>
        /// <value>
        ///     The orbwalker instance.
        /// </value>
        public Orbwalking.Orbwalker OrbwalkerInstance { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            this.OrbwalkerInstance.SetAttack(false);
            this.OrbwalkerInstance.SetMovement(false);
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            this.OrbwalkerInstance.SetAttack(true);
            this.OrbwalkerInstance.SetMovement(true);
        }

        /// <summary>
        ///     Sets the menu
        /// </summary>
        protected override void SetMenu()
        {
            this.OrbwalkerInstance = new Orbwalking.Orbwalker(this.Menu.Parent);

            this.Menu = this.Menu.Parent.SubMenu("Orbwalker");

            this.Menu.DisplayName = this.Name;
        }

        #endregion
    }
}