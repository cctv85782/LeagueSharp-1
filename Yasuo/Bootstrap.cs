namespace Yasuo
{
    using System;
    using System.Collections.Generic;
    using System.Net.Configuration;
    using System.Reflection;

    using LeagueSharp;
    using LeagueSharp.Common;


    using SharpDX;
    using SharpDX.Direct3D9;

    using Yasuo.Common.Classes;
    using Yasuo.Modules.Auto;
    using Yasuo.Modules.Protector;
    using Yasuo.Modules.WallDash;
    using Yasuo.OrbwalkingModes.Combo;
    using Yasuo.OrbwalkingModes.LaneClear;
    using Yasuo.OrbwalkingModes.LastHit;
    using Yasuo.OrbwalkingModes.Mixed;
    using Yasuo.Properties;

    /// <summary>
    ///     Class that loads every component of the assembly. Should only get called once.
    /// </summary>
    internal class Bootstrap
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public Bootstrap()
        {
            try
            {
                Variables.Assembly = new Assembly(Variables.ChampionName);

                if (Variables.Stop) return;

                #region parents

                // Core
                var assembly = new Modules.Assembly.Assembly();

                // Orbwalking Modes
                var combo = new Combo();
                var laneclear = new LaneClear();
                var lasthit = new LastHit();
                var mixed = new Mixed();

                // Extra Features
                var module = new Modules.Modules();
                var protector = new Protector();

                #endregion
                
                #region features

                CustomEvents.Game.OnGameLoad += delegate
                {
                    Variables.Assembly.Features.AddRange(
                        new List<IChild>
                            {
                                // Core
                                new Modules.Assembly.Version(assembly),
                                new Modules.Assembly.Debug(assembly),

                                // Orbwalking Modes
                                new OrbwalkingModes.Combo.SteelTempest(combo),
                                new OrbwalkingModes.Combo.SweepingBlade(combo),
                                new OrbwalkingModes.Combo.LastBreath(combo),
                                new OrbwalkingModes.Combo.Flash(combo),

                                new OrbwalkingModes.LaneClear.SteelTempest(laneclear),
                                new OrbwalkingModes.LaneClear.SweepingBlade(laneclear),
                                new OrbwalkingModes.LastHit.SteelTempest(lasthit),
                                new OrbwalkingModes.LastHit.SweepingBlade(lasthit),
                                new OrbwalkingModes.Mixed.SteelTempest(mixed),
                                new OrbwalkingModes.Mixed.SweepingBlade(mixed),

                                new Modules.Auto.Potions(module),
                                new Modules.Auto.KillSteal(module),
                                new Modules.WallDash.WallDash(module),

                                new Modules.Flee.SweepingBlade(module),

                                // Extra Features - Disabled due to SDK/Core problems
                                //new WindWallProtector(protector)
                            });

                    foreach (var feature in Variables.Assembly.Features)
                    {
                        feature.HandleEvents();
                    }

                    DrawBanner(Variables.Prefix + " " + Variables.Name, 1337, 6000);
                };

                #endregion
            }

            catch (Exception ex)
            {
                Console.WriteLine(@"Failed to load the assembly: " + ex);
            }
        }

        // TODO: Add Version & Name
        // TODO: Add Fade in & Fade out - probably SDK/Common can't offer that and I have to do it by myself
        /// <summary>
        ///     Method to display a banner and a text
        /// </summary>
        /// <param name="name">Name of the assembly</param>
        /// <param name="version">Version of the assembly</param>
        /// <param name="displayTime">Time of displaying</param>
        private void DrawBanner(String name, int version, int displayTime)
        {
            Notifications.AddNotification(string.Format("[{0}] {1} - loaded successfully!", name, version), displayTime, true);

            if (Game.Time <= 60)
            {
                var banner = new Render.Sprite(Resources.BannerLoading, new Vector2());

                // centered but a little above the screens center
                var position = new Vector2((Drawing.Width / 2) - banner.Width / 2, (Drawing.Height / 2) - banner.Height / 2 - 50);

                banner.Position = position;
                
                banner.Add(0);

                banner.OnDraw();

                Utility.DelayAction.Add(displayTime, () => banner.Remove());
            }
        }
    }
}
