﻿namespace Yasuo.Common.Extensions.MenuExtensions
{
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp.Common;

    public class DynamicMenu
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
        ///     The menu sets
        /// </summary>
        public List<List<MenuItem>> MenuSets;

        /// <summary>
        ///     The selecter
        /// </summary>
        public MenuItem Selecter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DynamicMenu" /> class.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="selecter">The selecter.</param>
        /// <param name="menuItems">The menu items.</param>
        public DynamicMenu(Menu menu, string displayName, MenuItem selecter, List<MenuItem>[] menuItems)
        {
            this.Menu = menu;
            this.DisplayName = displayName;
            this.Selecter = selecter;
            this.MenuSets = new List<List<MenuItem>>();

            foreach (var itemSet in menuItems)
            {
                this.MenuSets.Add(itemSet);
            }

            this.SetupMenu();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Setups the menu.
        /// </summary>
        private void SetupMenu()
        {
            if (this.Menu == null || this.MenuSets == null || !this.MenuSets.Any())
            {
                return;
            }

            this.AttachedMenu = new Menu(this.DisplayName, this.Menu.Name + this.DisplayName);

            this.Menu.AddSubMenu(this.AttachedMenu);

            var value = this.Selecter.GetValue<StringList>();

            this.AttachedMenu.AddItem(
                new MenuItem(this.AttachedMenu.Name + this.Selecter.Name, this.Selecter.DisplayName).SetValue(value))
                .ValueChanged += delegate(object sender, OnValueChangeEventArgs eventArgs)
                    {
                        foreach (var item in AttachedMenu.Items)
                        {
                            if (item.Tag != 0)
                            {
                                item.Hide();
                            }

                            if (item.Tag == eventArgs.GetNewValue<StringList>().SelectedIndex + 1)
                            {
                                item.Show();
                            }
                        }
                    };

            var tag = 1;

            foreach (var itemSet in this.MenuSets)
            {
                foreach (var item in itemSet)
                {
                    item.Name = Menu.Name + item.Name;
                    this.AttachedMenu.AddItem(item).SetTag(tag);
                }
                tag++;
            }

            MenuExtensions.RefreshTagBased(
                this.AttachedMenu,
                this.AttachedMenu.Item(this.AttachedMenu.Name + Selecter.Name).GetValue<StringList>().SelectedIndex + 1);
        }

        #endregion
    }
}