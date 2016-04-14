﻿using System;

using LeagueSharp;
using LeagueSharp.Common;

using System.Linq;

using LeagueSharp.SDK;

namespace Yasuo
{
    using System.Collections.Generic;
    using System.Reflection;

    using Yasuo.Common;
    using Yasuo.Common.Classes;
    using Yasuo.Common.Utility;

    class Assembly
    {
        /**
         TODO:
        
            Sweeping Blade: (low - med)
            + Detection of Trundle/J4/Anivia Walls while pathfinding
            + Killsteal

            Steel Tempest: (high)
            + Predictions seems wrong
            + Interrupt spells
            + Anti Gapcloser
            + Killsteal
            + Q while gapclosing
            + 3Q Ultimate Dodge
            
            Flash: (low)
            + EQ Flash
            + Q Flash

            Windwall: (low - med)
            + Protector
            + WQ Animation canceling
            + Precautional Windwall

            Last Breath: (high)
            + Overkill
            + Fix NRE
            + Do not towerdive option

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
        
        */

        /// <summary>
        ///     Initializes a new instance of the <see cref="Assembly"/> class.
        /// </summary>
        public Assembly(string name = null)
        {
            try
            {
                Menu = new Menu(name, name, true);
                
                CustomEvents.Game.OnGameLoad += OnGameLoad;
                CustomEvents.Game.OnGameEnd += OnGameEnd;

                Game.OnUpdate += OnUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public event EventHandler<Base.UnloadEventArgs> OnUnload;

        public Menu Menu { get; }

        public List<IChild> Features = new List<IChild>();

        /// <summary>
        ///     Called when the game loads
        /// </summary>
        /// <param name="args"></param>
        private void OnGameLoad(EventArgs args)
        {
            try
            {
                if (Variables.ChampionDependent)
                {
                    if (!Variables.SupportedChampions.Contains(Variables.Player.ChampionName))
                    {
                        Menu.AddItem(new MenuItem("ChampionNotSupported", "Champion is not supported"));
                        Variables.Stop = true;
                        return;
                    }
                }

                var orbWalkingMenu = new Menu("Orbwalking", "Orbwalking");
                Menu.AddSubMenu(orbWalkingMenu);

                Variables.Orbwalker = new Orbwalking.Orbwalker(Menu.SubMenu("Orbwalking"));

                Menu.AddToMainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        /// <summary>
        ///     Called when the game ends
        /// </summary>
        /// <param name="args"></param>
        private static void OnGameEnd(EventArgs args)
        {
            AppDomain.Unload(AppDomain.CurrentDomain);
        }

        /// <summary>
        ///     Called when the game updates
        /// </summary>
        /// <param name="args"></param>
        private static void OnUpdate(EventArgs args)
        {   
            Variables.SetSpells();
        }
    }
}
