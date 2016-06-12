namespace AssemblyName
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal class GlobalVariables
    {
        #region Static Fields

        /// <summary>
        ///     When the assembly got loaded
        /// </summary>
        public static readonly DateTime AssemblyLoadTime = DateTime.Now;

        /// <summary>
        ///     The assembly
        /// </summary>
        public static Assembly Assembly = null;

        /// <summary>
        ///     Debugstate
        /// </summary>
        public static bool Debug = true;

        /// <summary>
        ///     The root menu
        /// </summary>
        public static Menu RootMenu;

        /// <summary>
        ///     Dictionary containing all spells
        /// </summary>
        public static Dictionary<SpellSlot, Spell> Spells = null;

        #endregion

        #region Public Properties

        /// <summary>
        ///     The Author of the assembly
        /// </summary>
        public static string Author => "AuthorName";

        /// <summary>
        ///     String that is mainly used for displaying the full qualified assembly name.
        ///     IE: [BETA] ChampionName
        /// </summary>
        /// <value>
        ///     The display name.
        /// </value>
        public static string DisplayName => $"[{Prefix}] {Author}: {Name}";

        /// <summary>
        ///     The github path of the assembly
        /// </summary>
        public static string GitHubPath => $"{GitHubProfile}/LeagueSharp/tree/master/{Name}";

        /// <summary>
        ///     The profile name of the GitHub account
        /// </summary>
        public static string GitHubProfile => "GithubName";

        /// <summary>
        ///     The name of the assembly
        /// </summary>
        public static string Name => $"AssemblyName";

        /// <summary>
        ///     The Orbwalker
        /// </summary>
        public static Orbwalking.Orbwalker Orbwalker { get; internal set; }

        /// <summary>
        ///     The Player
        /// </summary>
        public static Obj_AI_Hero Player => ObjectManager.Player;

        /// <summary>
        ///     Different prefixes of the assembly. aka: WIP, BETA, ALPHA, TOBEUPDATED, BEST
        /// </summary>
        public static string Prefix => "WIP";

        /// <summary>
        ///     The champion(s) the assembly is for
        /// </summary>
        public static List<string> SupportedChampions => new List<string>() { "championname", "alternatechampionname" };

        #endregion

        #region Properties

        /// <summary>
        ///     If the assembly is limited to a specific champion
        /// </summary>
        internal static bool ChampionDependent => true;

        #endregion
    }
}