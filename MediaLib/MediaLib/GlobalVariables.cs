namespace RethoughtLib
{
    #region Using Directives

    using LeagueSharp;

    #endregion

    /// <summary>
    ///     Global Variables
    /// </summary>
    public static class GlobalVariables
    {
        #region Static Fields

        /// <summary>
        ///     The debug
        /// </summary>
        public static bool Debug = false;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the author.
        /// </summary>
        /// <value>
        ///     The author.
        /// </value>
        public static string Author => "Rethought";

        /// <summary>
        ///     Gets the display name.
        /// </summary>
        /// <value>
        ///     The display name.
        /// </value>
        public static string DisplayName => "RethoughtLib";

        /// <summary>
        ///     Gets the player.
        /// </summary>
        /// <value>
        ///     The player.
        /// </value>
        public static Obj_AI_Base Player => ObjectManager.Player;

        #endregion
    }
}