//TODO: Set collision (Spells Q - YasuoWall)

namespace Yasuo
{
    using System;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Yasuo.Common.Extensions;
    using Yasuo.Common.Objects;
    using Yasuo.Common.Utility;

    internal class Variables : Helper
    {
        /// <summary>
        ///     Method that adjusts the spells to the current circumstances
        /// </summary>
        public static void SetSpells()
        {
            var q = Spells[SpellSlot.Q];
            var w = Spells[SpellSlot.W];
            var e = Spells[SpellSlot.E];
            var r = Spells[SpellSlot.R];

            if (Player.HasQ3())
            {
                q.SetSkillshot(GetQDelay, 90, 1200, false, SkillshotType.SkillshotLine);
                q.Range = 475 * 2;
                q.MinHitChance = HitChance.VeryHigh;
            }
            else
            {
                q.SetSkillshot(GetQDelay, 20, float.MaxValue, false, SkillshotType.SkillshotLine);
                q.Range = 475;
                q.MinHitChance = HitChance.VeryHigh;
            }
            if (Player.IsDashing())
            {
                q.SetSkillshot(GetQDelay, 375, float.MaxValue, false, SkillshotType.SkillshotCircle);
                q.MinHitChance = HitChance.High;
            }

            w = new Spell(SpellSlot.W, 400);
            w.SetSkillshot(0, 400, 400, false, SkillshotType.SkillshotLine);

            e = new Spell(SpellSlot.E, 475);
            e.SetTargetted(0, 1025);
            e.Speed = 1000;

            r = new Spell(SpellSlot.R, 900);
            r.SetTargetted(0, float.MaxValue);
        }

        /// <summary>
        ///     Debugstate
        /// </summary>
        public static bool Debug = false;

        /// <summary>
        ///     If Stop is active the assembly will stop loading
        /// </summary>
        internal static bool Stop = false;

        /// <summary>
        ///     The assembly
        /// </summary>
        public static Assembly Assembly = null;

        /// <summary>
        ///     The Orbwalker
        /// </summary>
        public static Orbwalking.Orbwalker Orbwalker { get; internal set; }

        /// <summary>
        ///     The Player
        /// </summary>
        public static Obj_AI_Hero Player => ObjectManager.Player;

        /// <summary>
        ///     When the assembly got loaded
        /// </summary>
        public static readonly DateTime AssemblyLoadTime = DateTime.Now;

        /// <summary>
        ///     If the assembly supports Plugins (Plugin-Folder)
        /// </summary>
        internal static bool PluginSupport = false;

        /// <summary>
        ///     Directory of where plugins should get loaded from
        /// </summary>
        internal static string PluginDirectory = "";

        /// <summary>
        ///     If the assembly is limited to a specific champion
        /// </summary>
        internal static bool ChampionDependent = true;

        /// <summary>
        ///     The champion(s) the assembly is for
        /// </summary>
        public static List<string> SupportedChampions = new List<string>() { "Yasuo" };

        /// <summary>
        ///     The profile name of the GitHub account
        /// </summary>
        public static string GitHubProfile = "MediaGithub";

        /// <summary>
        ///     The github path of the assembly
        /// </summary>
        public static string GitHubPath = string.Format("{0}/{1}/{2}", GitHubProfile, "LeagueSharp/tree/master", Name);

        /// <summary>
        ///     The name of the assembly
        /// </summary>
        public static string Name => "MediaSuo";

        /// <summary>
        ///     The Author of the assembly
        /// </summary>
        public static string Author => "Media";

        /// <summary>
        ///     Different prefixes of the assembly. aka: WIP, BETA, ALPHA, TOBEUPDATED, BEST
        /// </summary>
        public static string Prefix => "Alpha";

        /// <summary>
        ///     Dictionary containing all spells
        /// </summary>
        public static Dictionary<SpellSlot, Spell> Spells = new Dictionary<SpellSlot, Spell>()
        {
            {
                SpellSlot.Q, new Spell(SpellSlot.Q, 515)
            },
            {
                SpellSlot.W, new Spell(SpellSlot.W, 400)
            },
            {
                SpellSlot.E, new Spell(SpellSlot.E, 475, TargetSelector.DamageType.Magical)
            },
            {
                SpellSlot.R, new Spell(SpellSlot.R, 1200)
            }
        };
    }
}
