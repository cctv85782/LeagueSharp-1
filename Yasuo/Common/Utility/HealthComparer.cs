namespace Yasuo.Common.Utility
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Yasuo.Common.Extensions.MenuExtensions;

    internal class HealthComparer
    {
        #region Fields

        /// <summary>
        ///     The menu
        /// </summary>
        private readonly Menu menu = null;

        /// <summary>
        ///     The menu item color
        /// </summary>
        private Color menuItemColor;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HealthComparer" /> class.
        /// </summary>
        /// <param name="menu">The menu.</param>
        public HealthComparer(Menu menu)
        {
            this.menu = menu;

            this.SetupMenu();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the color of the menu item.
        /// </summary>
        /// <value>
        ///     The color of the menu item.
        /// </value>
        public Color MenuItemColor
        {
            get
            {
                return this.menuItemColor;
            }
            set
            {
                if (value != Color.Black)
                {
                    this.menuItemColor = value;
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Validates the specified unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public bool Valid(Obj_AI_Base unit)
        {
            return Evaluate(unit.HealthPercent);
        }

        /// <summary>
        ///     Validates the specified units.
        /// </summary>
        /// <param name="units">The units.</param>
        /// <returns></returns>
        public bool Valid(List<Obj_AI_Base> units)
        {
            return Evaluate(units.Sum(x => x.HealthPercent) / units.Count);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Evaluates the specified health percentage.
        /// </summary>
        /// <param name="healthPerc">The health perc.</param>
        /// <returns></returns>
        private bool Evaluate(float healthPerc)
        {
            var healthcompareSettings = menu.SubMenu(menu.Name + "Health-Comparer");

            switch (
                healthcompareSettings.Item(healthcompareSettings.Name + "Mode").GetValue<StringList>().SelectedIndex + 1
                )
            {
                case 1:
                    var difference1 = GlobalVariables.Player.HealthPercent - healthPerc;

                    if (difference1 < 0)
                    {
                        difference1 = -difference1;
                    }

                    if (difference1
                        <= healthcompareSettings.Item(healthcompareSettings.Name + "MaxHealthPercDifference")
                               .GetValue<Slider>()
                               .Value)
                    {
                        return true;
                    }
                    return false;

                case 2:

                case 3:
                    return true;
            }
            return true;
        }

        /// <summary>
        ///     Setups the menu.
        /// </summary>
        private void SetupMenu()
        {
            if (this.menu == null)
            {
                return;
            }

            var selecter =
                new MenuItem("Mode", "Mode").SetValue(new StringList(new[] { "Compare", "Custom", "Disabled" }, 0));

            var compare = new List<MenuItem>()
                              {
                                  new MenuItem("MaxHealthPercDifference", "Max % health difference")
                                      .SetValue(new Slider(50, 0, 100)),
                                  new MenuItem("MinPlayerHealth", "Min % Player health").SetValue(
                                      new Slider(0)),
                              };

            var custom = new List<MenuItem>() { };

            var disabled = new List<MenuItem>()
                               {
                                   new MenuItem(
                                       "DisclaimerPathBased",
                                       "[i] Never aim for multiple targets")
                               };

            var menuArray = new[] { compare, custom, disabled };

            var dynamicMenu = new DynamicMenu(menu, "Health-Comparer", selecter, menuArray);
        }

        #endregion
    }
}