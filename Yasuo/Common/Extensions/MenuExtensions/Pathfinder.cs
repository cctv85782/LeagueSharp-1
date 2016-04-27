// ReSharper disable AccessToForEachVariableInClosure

namespace Yasuo.Common.Extensions.MenuExtensions
{
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    // TODO: PRIORITY HIGH
    public class Pathfinder
    {
        #region Fields

        /// <summary>
        ///     The new menu
        /// </summary>
        public Menu AttachedMenu;

        /// <summary>
        ///     The blacklist
        /// </summary>
        public Blacklist Blacklist;

        /// <summary>
        ///     The blacklisted heroes
        /// </summary>
        public List<Obj_AI_Base> BlacklistedHeroes = new List<Obj_AI_Base>();

        /// <summary>
        ///     The display name
        /// </summary>
        public string DisplayName;

        /// <summary>
        ///     The main menu
        /// </summary>
        public Menu Menu;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DynamicMenu" /> class.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="displayName">The display name.</param>
        public Pathfinder(Menu menu, string displayName)
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
            Blacklist = new Blacklist(AttachedMenu, "Blacklist");

            var selecter = new MenuItem("ModeTarget", "Dash to: ").SetValue(new StringList(new[] { "Mouse", "Enemy" }));

            var mouse = new List<MenuItem>() { };

            var enemy = new List<MenuItem>()
                            {
                                new MenuItem("Prediction", "Predict enemy position").SetValue(true)
                                    .SetTooltip(
                                        "The assembly will try to E to the enemy predicted position. This will not work if Mode is set to Mouse."),
                            };

            var dynamicMenu = new DynamicMenu(Menu, "Pathfinder", selecter, new[] { mouse, enemy });

            var both = new List<MenuItem>()
                           {
                               new MenuItem("AutoWalkToDash", "[Experimental] Auto-Walk to dash")
                                   .SetValue(true)
                                   .SetTooltip(
                                       "If this is enabled the assembly will auto-walk behind a unit to dash over it."),
                               new MenuItem(
                                   "AutoDashing",
                                   "[Experimental] Auto-Dash dashable path (Dashing-Path)").SetValue(
                                       true)
                                   .SetTooltip(
                                       "If this is enabled the assembly will automatic pathfind and walk to the end of the path. This is a basic feature of pathfinding."),
                               new MenuItem(
                                   "AutoWalking",
                                   "[Experimental] Auto-Walk non-dashable path (Walking-Path)")
                                   .SetValue(false)
                                   .SetTooltip(
                                       "If this is enabled the assembly will automatic pathfind and walk to the end of the path. If you like to have maximum control or your champion disable this."),
                               new MenuItem(
                                   "PathAroundSkillShots",
                                   "[Experimental] Try to Path around Skillshots").SetValue(true)
                                   .SetTooltip(
                                       "if this is enabled, the assembly will path around a skillshot if a path is given."),
                           };

            AttachedMenu.AddItem(new MenuItem("Enabled", "Enabled").SetValue(true));
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