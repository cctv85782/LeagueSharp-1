namespace Yasuo
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using global::Yasuo.CommonEx;
    using global::Yasuo.CommonEx.Classes;

    using LeagueSharp.Common;

    #endregion

    class Assembly
    {
        #region Fields

        /// <summary>
        ///     The features of the assembly
        /// </summary>
        public List<IChild> Features = new List<IChild>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Assembly" /> class.
        /// </summary>
        public Assembly(IChampion champion)
        {
            if (champion == null || GlobalVariables.RootMenu != null)
            {
                return;
            }

            GlobalVariables.RootMenu = new Menu($"[{GlobalVariables.Name}]: " + champion.Name, string.Format("Root"), true);

            champion.Load();

            Events.Initialize();

            var orbWalkingMenu = new Menu("Orbwalking", "Orbwalking");

            GlobalVariables.RootMenu.AddSubMenu(orbWalkingMenu);

            GlobalVariables.Orbwalker = new Orbwalking.Orbwalker(GlobalVariables.RootMenu.SubMenu("Orbwalking"));

            GlobalVariables.RootMenu.AddToMainMenu();
        }

        #endregion
    }
}