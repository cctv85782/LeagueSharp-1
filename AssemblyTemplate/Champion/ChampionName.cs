namespace AssemblyName.Champion
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using AssemblyName.Base.Modules.Assembly;
    using AssemblyName.Champion.Modules.Auto;
    using AssemblyName.Champion.Modules.Protector;
    using AssemblyName.Champion.Modules.WallDash;
    using AssemblyName.Champion.Modules.Hidden;
    using AssemblyName.Champion.OrbwalkingModes.Combo;
    using AssemblyName.Champion.OrbwalkingModes.LaneClear;
    using AssemblyName.Champion.OrbwalkingModes.LastHit;
    using AssemblyName.Champion.OrbwalkingModes.Mixed;
    using AssemblyName.Champion.Spells;
    using AssemblyName.MediaLib.Classes;
    using AssemblyName.MediaLib.Classes.Feature;

    using LeagueSharp.Common;

    using Version = AssemblyName.Base.Modules.Assembly.Version;

    #endregion

    internal class ChampionName : IChampion
    {
        #region Public Properties

        public string Name { get; } = "Yasuo";

        #endregion

        #region Public Methods and Operators

        [SuppressMessage("ReSharper", "RedundantNameQualifier")]
        public void Load()
        {
            #region parents

            // Core
            var assembly = new Assembly();

            // Orbwalking Modes
            var combo = new Combo();
            var laneclear = new LaneClear();
            var lasthit = new LastHit();
            var mixed = new Mixed();

            // Additional Features
            var module = new Modules.Modules();
            var protector = new Protector();

            #endregion

            #region features

            CustomEvents.Game.OnGameLoad += delegate
                {
                    GlobalVariables.Assembly.Features.AddRange(
                        new List<IFeatureChild>
                            {
                                new Version(assembly),
                                new Debug(assembly),
                                new CastManager(assembly),
                                new SpellInitializer(assembly),
                                new SpellManager(assembly),
                                
                                new Flash(combo),

                                new Potions(module),
                                new KillSteal(module),
                                new WallDash(module),
                            });

                    foreach (var feature in GlobalVariables.Assembly.Features.Where(feature => !feature.Handled))
                    {
                        if (GlobalVariables.Debug)
                        {
                            Console.WriteLine(@"Loading Feature: {0}, Enabled: {1}", feature.Name, feature.Enabled);
                        }

                        feature.HandleEvents();
                    }
                };

            #endregion
        }

        #endregion
    }
}