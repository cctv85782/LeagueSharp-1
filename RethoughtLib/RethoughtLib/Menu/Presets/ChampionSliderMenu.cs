namespace RethoughtLib.Menu.Presets
{
    #region Using Directives

    using LeagueSharp.Common;

    using RethoughtLib.Menu.Interfaces;

    #endregion

    public class ChampionSliderMenu : IMenuPreset
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
        ///     Initializes a new instance of the <see cref="ChampionSliderMenu" /> class.
        /// </summary>
        /// <param name="displayName">
        ///     The menu.
        ///     The display name.
        /// </param>
        public ChampionSliderMenu(string displayName)
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

        public int Modifier { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Generates this instance.
        /// </summary>
        public void Generate()
        {
            this.SetupMenu();
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
                this.attachedMenu.AddItem(new MenuItem(this.attachedMenu.Name + "null", "No enemies found"));
            }
            else
            {
                var maxRange = (int)HeroManager.Enemies.MaxOrDefault(x => x.AttackRange).AttackRange;

                foreach (var hero in HeroManager.Enemies)
                {
                    var range = (int)hero.AttackRange;

                    this.attachedMenu.AddItem(
                        new MenuItem(this.attachedMenu.Name + hero.ChampionName, hero.ChampionName).SetValue(
                            new Slider(range + this.Modifier, 0, maxRange)));
                }
            }
        }

        /// <summary>
        ///     Setups the menu.
        /// </summary>
        private void SetupMenu()
        {
            if (this.Menu == null)
            {
                return;
            }

            this.attachedMenu = new Menu(this.displayName, this.Menu.Name + this.displayName);

            this.Menu.AddSubMenu(this.attachedMenu);

            this.attachedMenu.AddItem(new MenuItem(this.attachedMenu.Name + "Enabled", "Enabled").SetValue(true));

            this.attachedMenu.AddItem(
                new MenuItem(this.attachedMenu.Name + "Modifier", "Modifier").SetValue(new Slider(0, -2000, 2000)))
                .ValueChanged +=
                delegate(object sender, OnValueChangeEventArgs eventArgs)
                    {
                        this.Modifier = eventArgs.GetNewValue<Slider>().Value;
                    };
        }

        #endregion
    }
}