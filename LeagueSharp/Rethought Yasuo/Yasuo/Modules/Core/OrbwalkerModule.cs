namespace Rethought_Yasuo.Yasuo.Modules.Core
{
    #region Using Directives

    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class OrbwalkerModule : ChildBase
    {
        #region Fields

        /// <summary>
        ///     The Orbwalker
        /// </summary>
        public Orbwalking.Orbwalker Orbwalker;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrbwalkerModule" /> class.
        /// </summary>
        /// <param name="mainMenu">The main menu.</param>
        public OrbwalkerModule(Menu mainMenu)
        {
            this.Orbwalker = new Orbwalking.Orbwalker(mainMenu);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Unknown";

        #endregion

        #region Methods

        /// <summary>
        ///     Initializes the menu, overwrite this method to change the menu.
        /// </summary>
        protected override void SetMenu()
        {
            return;
        }

        #endregion
    }
}