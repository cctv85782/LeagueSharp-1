namespace RethoughtLib.Menu.Presets
{
    #region Using Directives

    using LeagueSharp.Common;

    using global::RethoughtLib.Menu.Interfaces;

    #endregion

    public class WhitelistMenu : IMenuPreset
    {
        #region Fields

        /// <summary>
        ///     The display name
        /// </summary>
        private readonly string displayName;

        /// <summary>
        ///     The new menu
        /// </summary>
        private Menu attachedMenu;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WhitelistMenu" /> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        public WhitelistMenu(string displayName)
        {
            this.displayName = displayName;
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

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the enemies.
        /// </summary>
        public void AddEnemies()
        {
            if (HeroManager.Enemies.Count == 0)
            {
                this.attachedMenu.AddItem(new MenuItem(this.attachedMenu.Name + "null", "No enemies found"));
            }
            else
            {
                foreach (var x in HeroManager.Enemies)
                {
                    this.attachedMenu.AddItem(
                        new MenuItem(this.attachedMenu.Name + x.ChampionName, x.ChampionName).SetValue(true));
                }
            }
        }

        /// <summary>
        ///     Generates this instance.
        /// </summary>
        public void Generate()
        {
            if (this.Menu == null)
            {
                return;
            }

            this.attachedMenu = new Menu(this.displayName, this.Menu.Name + this.displayName);

            this.Menu.AddSubMenu(this.attachedMenu);

            this.AddEnemies();
        }

        #endregion
    }
}