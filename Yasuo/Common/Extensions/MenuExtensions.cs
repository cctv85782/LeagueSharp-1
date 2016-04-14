using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp.Common;

namespace Yasuo.Common.Extensions
{
    public static class MenuExtensions
    {
        public static void AddToolTip(Menu menu, string helpText)
        {
            menu.AddItem(new MenuItem(menu.Name +" Helper", "Helper").SetTooltip(helpText));
        }

        internal static void RefreshMenu(Menu menu, int tag)
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
    }
}
