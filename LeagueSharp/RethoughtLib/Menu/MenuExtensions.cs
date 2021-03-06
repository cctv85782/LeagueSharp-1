﻿namespace RethoughtLib.Menu
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp.Common;

    #endregion

    public static class MenuExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Adds the tool tip.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="helpText">The help text.</param>
        public static void AddHelper(Menu menu, string helpText)
        {
            if (menu == null)
            {
                throw new NullReferenceException("Menu is null");
            }

            if (string.IsNullOrWhiteSpace(helpText))
            {
                throw new NullReferenceException("String is null or empty or whitespace.");
            }

            var index = 0;
            var displayName = "Helper";

            for (var i = 0; i < menu.Items.Count(x => x.Name.Contains("Helper")); i++)
            {
                index = i;
            }

            if (index > 0)
            {
                displayName = displayName += $" No. {index}";
            }

            menu.AddItem(new MenuItem(menu.Name + "Helper" + index, displayName).SetTooltip(helpText));
        }

        /// <summary>
        ///     Hides the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public static void Hide(this MenuItem item)
        {
            if (item == null)
            {
                throw new NullReferenceException("MenuItem is null");
            }

            if (item.ShowItem)
            {
                item.Show(false);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Refreshes the menu based on a specified tag.
        ///     All menuItems that have another tag will be hidden after the refresh.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="tag">The tag.</param>
        internal static void RefreshTagBased(Menu menu, int tag)
        {
            if (menu == null)
            {
                throw new NullReferenceException("Menu is null");
            }

            foreach (var item in menu.Items)
            {
                if (item.Tag != 0)
                {
                    item.Hide();
                }

                if (item.Tag == tag)
                {
                    item.Show();
                }
            }
        }

        #endregion
    }
}