// ReSharper disable AccessToForEachVariableInClosure

namespace Yasuo.Common.Extensions.MenuExtensions
{
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    public class Whitelist
    {
        #region Fields

        /// <summary>
        ///     The new menu
        /// </summary>
        public Menu AttachedMenu;

        /// <summary>
        ///     The display name
        /// </summary>
        public string DisplayName;

        /// <summary>
        ///     The main menu
        /// </summary>
        public Menu Menu;

        /// <summary>
        ///     The whitelisted heroes
        /// </summary>
        public List<Obj_AI_Base> WhitelistedHeroes = new List<Obj_AI_Base>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DynamicMenu" /> class.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="displayName">The display name.</param>
        public Whitelist(Menu menu, string displayName)
        {
            this.Menu = menu;
            this.DisplayName = displayName;

            this.SetupMenu();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the enemies.
        /// </summary>
        public void AddEnemies()
        {
            if (HeroManager.Enemies.Count == 0)
            {
                AttachedMenu.AddItem(new MenuItem(AttachedMenu.Name + "null", "No enemies found"));
            }
            else
            {
                foreach (var x in HeroManager.Enemies)
                {
                    AttachedMenu.AddItem(
                        new MenuItem(AttachedMenu.Name + x.ChampionName, x.ChampionName).SetValue(true)).ValueChanged +=
                        delegate(object sender, OnValueChangeEventArgs eventArgs)
                            {
                                if (eventArgs.GetNewValue<bool>())
                                {
                                    if (!WhitelistedHeroes.Contains(x))
                                    {
                                        WhitelistedHeroes.Add(x);
                                    }
                                }
                                else
                                {
                                    if (WhitelistedHeroes.Contains(x))
                                    {
                                        WhitelistedHeroes.Remove(x);
                                    }
                                }
                            };
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Setups the menu.
        /// </summary>
        private void SetupMenu()
        {
            if (this.Menu == null)
            {
                return;
            }

            this.AttachedMenu = new Menu(this.DisplayName, this.Menu.Name + this.DisplayName);

            this.Menu.AddSubMenu(this.AttachedMenu);
        }

        #endregion
    }
}