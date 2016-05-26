namespace Yasuo.Yasuo
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using global::Yasuo.CommonEx.Classes;
    using global::Yasuo.Yasuo.Modules.Assembly;
    using global::Yasuo.Yasuo.Modules.Auto;
    using global::Yasuo.Yasuo.Modules.Protector;
    using global::Yasuo.Yasuo.Modules.WallDash;
    using global::Yasuo.Yasuo.OrbwalkingModes.Combo;
    using global::Yasuo.Yasuo.OrbwalkingModes.LaneClear;
    using global::Yasuo.Yasuo.OrbwalkingModes.LastHit;
    using global::Yasuo.Yasuo.OrbwalkingModes.Mixed;

    using LeagueSharp.Common;

    #endregion

    class ChampionYasuo : IChampion
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
                        new List<IChild>
                            {
                                // Core
                                new Modules.Assembly.Version(assembly),
                                new Debug(assembly), new CastManager(assembly), new SpellManager(assembly),

                                // Orbwalking Modes
                                new OrbwalkingModes.Combo.SteelTempest(combo),
                                new OrbwalkingModes.Combo.SweepingBlade(combo),
                                
                                new OrbwalkingModes.Combo.LastBreath(combo),
                                //new Flash(combo),
                                new OrbwalkingModes.LaneClear.SteelTempest(laneclear),
                                new OrbwalkingModes.LaneClear.SweepingBlade(laneclear),
                                new OrbwalkingModes.LaneClear.Eq(laneclear),
                                new OrbwalkingModes.LastHit.SteelTempest(lasthit),
                                new OrbwalkingModes.LastHit.SweepingBlade(lasthit),
                                new OrbwalkingModes.LastHit.Eq(lasthit),
                                new OrbwalkingModes.Mixed.SteelTempest(mixed),
                                new OrbwalkingModes.Mixed.SweepingBlade(mixed),
                                // new Potions(module),
                                //new KillSteal(module),
                            new Modules.WallDash.WallDash(module),
                                new Modules.Flee.SweepingBlade(module),

                                // Extra Features - Disabled due to SDK/Core problems
                                //new WindWallProtector(protector)
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