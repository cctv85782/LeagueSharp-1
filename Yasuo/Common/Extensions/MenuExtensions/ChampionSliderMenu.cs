﻿// ReSharper disable AccessToForEachVariableInClosure

namespace Yasuo.Common.Extensions.MenuExtensions
{
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    public class ChampionSliderMenu
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
        ///     The blacklisted heroes
        /// </summary>
        public Dictionary<Obj_AI_Base, float> Values = new Dictionary<Obj_AI_Base, float>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DynamicMenu" /> class.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="displayName">The display name.</param>
        public ChampionSliderMenu(Menu menu, string displayName)
        {
            this.Menu = menu;
            this.DisplayName = displayName;

            this.SetupMenu();
            this.AddEnemies();
        }

        #endregion

        #region Public Properties

        public int Modifier { get; set; }

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
                var maxRange = (int)HeroManager.Enemies.MaxOrDefault(x => x.AttackRange).AttackRange;

                foreach (var hero in HeroManager.Enemies)
                {
                    var range = (int)hero.AttackRange;

                    AttachedMenu.AddItem(
                        new MenuItem(AttachedMenu.Name + hero.ChampionName, hero.ChampionName).SetValue(
                            new Slider(range + Modifier, 0, maxRange))).ValueChanged +=
                        delegate(object sender, OnValueChangeEventArgs eventArgs)
                            {
                                if (!Values.ContainsKey(hero))
                                {
                                    Values.Add(hero, eventArgs.GetNewValue<Slider>().Value);
                                }
                                else
                                {
                                    Values[hero] = eventArgs.GetNewValue<Slider>().Value;
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

            this.AttachedMenu.AddItem(
                new MenuItem(this.AttachedMenu.Name + "Modifier", "Modifier").SetValue(new Slider(0, -2000, 2000)))
                .ValueChanged +=
                delegate(object sender, OnValueChangeEventArgs eventArgs)
                    {
                        Modifier = eventArgs.GetNewValue<Slider>().Value;
                    };
        }

        #endregion
    }
}