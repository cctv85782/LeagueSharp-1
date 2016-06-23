namespace RethoughtLib.Menu
{
    using LeagueSharp.Common;

    public static class MenuExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Adds the tool tip.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="helpText">The help text.</param>
        public static void AddToolTip(Menu menu, string helpText)
        {
            menu.AddItem(new MenuItem(menu.Name + " Helper", "Helper").SetTooltip(helpText));
        }

        /// <summary>
        ///     Hides the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public static void Hide(this MenuItem item)
        {
            if (item != null)
            {
                if (item.ShowItem)
                {
                    item.Show(false);
                }
            }
        }

        #endregion  

        #region Methods

        /// <summary>
        /// Refreshes the menu based on a specified tag.
        /// All menuItems that have another tag will be hidden after the refresh.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="tag">The tag.</param>
        internal static void RefreshTagBased(Menu menu, int tag)
        {
            if (menu == null)
            {
                return;
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