namespace RethoughtLib.Menu.Presets
{
    #region Using Directives

    using LeagueSharp.Common;

    using global::RethoughtLib.Menu.Interfaces;

    #endregion

    public class BlacklistMenu : IMenuPreset
    {
        #region Fields

        /// <summary>
        ///     The display name
        /// </summary>
        private readonly string displayName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BlacklistMenu" /> class.
        /// </summary>
        /// <param name="displayName">
        ///     The display name.
        /// </param>
        public BlacklistMenu(string displayName)
        {
            this.displayName = displayName;

            this.Generate();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the menu.
        /// </summary>
        /// <value>
        ///     The menu.
        /// </value>
        public Menu Menu { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the attached menu.
        /// </summary>
        /// <value>
        ///     The attached menu.
        /// </value>
        private Menu AttachedMenu { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Setups the menu.
        /// </summary>
        public void Generate()
        {
            if (this.Menu == null)
            {
                return;
            }

            this.AttachedMenu = new Menu(this.displayName, this.Menu.Name + this.displayName);

            this.Menu.AddSubMenu(this.AttachedMenu);

            this.AddEnemies();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the enemies.
        /// </summary>
        private void AddEnemies()
        {
            if (HeroManager.Enemies.Count == 0)
            {
                this.AttachedMenu.AddItem(new MenuItem(this.AttachedMenu.Name + "null", "No enemies found"));
            }
            else
            {
                foreach (var x in HeroManager.Enemies)
                {
                    this.AttachedMenu.AddItem(
                        new MenuItem(this.AttachedMenu.Name + x.ChampionName, x.ChampionName).SetValue(false));
                }
            }
        }

        #endregion
    }
}