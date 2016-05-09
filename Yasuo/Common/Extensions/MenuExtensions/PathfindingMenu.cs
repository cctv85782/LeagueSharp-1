// ReSharper disable AccessToForEachVariableInClosure

namespace Yasuo.Common.Extensions.MenuExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    // TODO: PRIORITY HIGH
    public class PathfindingMenu
    {
        #region Fields

        /// <summary>
        ///     The blacklist
        /// </summary>
        public Menu Blacklist;

        /// <summary>
        ///     The blacklisted heroes
        /// </summary>
        public List<Obj_AI_Base> BlacklistedHeroes;

        /// <summary>
        ///     The display name
        /// </summary>
        public string DisplayName;

        /// <summary>
        ///     The main menu
        /// </summary>
        public Menu Menu;

        /// <summary>
        ///     The settings
        /// </summary>
        public Menu Settings;

        /// <summary>
        ///     The dynamic menu
        /// </summary>
        private DynamicMenu attachedMenu;

        /// <summary>
        ///     The blacklist
        /// </summary>
        private BlacklistMenu blacklistMenu;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="attachedMenu" /> class.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="displayName">The display name.</param>
        public PathfindingMenu(Menu menu, string displayName)
        {
            this.Menu = menu;
            this.DisplayName = displayName;

            this.Setup();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds all menu items.
        /// </summary>
        private void Setup()
        {
            var selecter = new MenuItem("Mode", "Dash to: ").SetValue(new StringList(new[] { "Mouse", "Enemy" }));

            var mouse = new List<MenuItem>() { };

            var enemy = new List<MenuItem>()
                            {
                                new MenuItem("Prediction", "Use Prediction").SetValue(true)
                                    .SetTooltip(
                                        "The assembly will try to E to the enemy predicted position."),

                                new MenuItem("PredictionEnhanced", "Prediction -> Two Path System").SetValue(true)
                                    .SetTooltip(
                                        "The assembly will try to E to the enemy predicted position."),

                                new MenuItem("MinCursorDistance", "Min Cursor Distance to target").SetValue(new Slider(600, 50, 2000)),
                            };

            this.attachedMenu = new DynamicMenu(this.Menu, DisplayName, selecter, new[] { mouse, enemy });

            var both = new List<MenuItem>()
                           {
                new MenuItem("DontDashUnderTurret", "Don't dash under turret")
                .SetValue(true),
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

                               new MenuItem("Enabled", "Enabled").SetValue(true),
                           };

            this.blacklistMenu = new BlacklistMenu(this.attachedMenu.AttachedMenu, "Blacklist");

            foreach (var item in both)
            {
                item.Name = attachedMenu.AttachedMenu.Name + item.Name;

                this.attachedMenu.AttachedMenu.AddItem(item);
            }

            foreach (var item in this.attachedMenu.AttachedMenu.Items)
            {
                if (item.Name == "Mode")
                {
                    var stringarray = item.GetValue<StringList>().SList;

                    var id = 0;

                    for (var i = 0; i < stringarray.Count(); i++)
                    {
                        if (stringarray[i] == "Enemy")
                        {
                            id = i + 1;
                        }
                    }

                    foreach (var item2 in this.blacklistMenu.AttachedMenu.Items)
                    {
                        item2.SetTag(id);
                    }
                }
            }

            Settings = this.attachedMenu.AttachedMenu;
            Blacklist = this.blacklistMenu.AttachedMenu;
            BlacklistedHeroes = this.blacklistMenu.BlacklistedHeroes;
        }

        #endregion
    }
}