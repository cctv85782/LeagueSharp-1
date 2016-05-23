﻿namespace Yasuo
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using global::Yasuo.CommonEx;
    using global::Yasuo.CommonEx.Classes;

    using LeagueSharp.Common;

    #endregion

    /**
 TODO:
    Legend:
    + = Add
    ~ = Change
    # = Rethink
    - = Remove
    * = Bugfix

    Sweeping Blade: (low - med)
    + Detection of Trundle/J4/Anivia Walls while pathfinding
    + Killsteal

    Steel Tempest: (high)
    + Interrupt spells
    + Anti Gapcloser
    + Killsteal
    + (80%) Q while gapclosing
    + 3Q Ultimate Dodge
    
    Flash: (low)
    + (50%) EQ Flash
    + Q Flash

    Windwall: (low - med)
    + Protector
    + WQ Animation canceling
    + Precautional Windwall

    Last Breath: (very high)
    ~ Overkill
    * Fix NRE
    # Do not towerdive option

    TurretLogicProvider: (med)
    + Fix Exception
    + Improve safety check for undertower

    OrbwalkingModes:
    + Add Freeze
    + Add Combo without moving

    General: (high)
    + Add Tower dive logic
    + Fix Prediction
    + Fix Under Tower Checks
    ~ Making Assembly SDK Only

*/

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
            GlobalVariables.RootMenu = new Menu($"[{GlobalVariables.Name}]: XY", string.Format("CY"), true);

            champion?.Load();
            
            Events.Initialize();

            CustomEvents.Game.OnGameLoad += this.OnGameLoad;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when the game loads
        /// </summary>
        /// <param name="args"></param>
        private void OnGameLoad(EventArgs args)
        {
            try
            {
                var orbWalkingMenu = new Menu("Orbwalking", "Orbwalking");
                GlobalVariables.RootMenu.AddSubMenu(orbWalkingMenu);

                GlobalVariables.Orbwalker = new Orbwalking.Orbwalker(GlobalVariables.RootMenu.SubMenu("Orbwalking"));

                GlobalVariables.RootMenu.AddToMainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #endregion
    }
}