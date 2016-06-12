namespace AssemblyName
{
    #region Using Directives

    using System.Collections.Generic;

    using AssemblyName.MediaLib.Classes;
    using AssemblyName.MediaLib.Classes.Feature;
    using AssemblyName.MediaLib.Utility;

    using LeagueSharp.Common;

    #endregion

    /// <summary>
    ///     Class representing the base of the assembly
    /// </summary>
    internal class Assembly
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Assembly" /> class.
        /// </summary>
        /// <param name="champion">The champion.</param>
        public Assembly(IChampion champion)
        {
            if (champion == null || GlobalVariables.RootMenu != null)
            {
                return;
            }

            GlobalVariables.RootMenu = new Menu($"[{GlobalVariables.Author}]: " + champion.Name, "Root", true);

            champion.Load();

            Events.Initialize();

            var orbWalkingMenu = new Menu("Orbwalking", "Orbwalking");

            GlobalVariables.RootMenu.AddSubMenu(orbWalkingMenu);

            GlobalVariables.Orbwalker = new Orbwalking.Orbwalker(GlobalVariables.RootMenu.SubMenu("Orbwalking"));

            GlobalVariables.RootMenu.AddToMainMenu();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the features.
        /// </summary>
        /// <value>
        ///     The features.
        /// </value>
        public List<IFeatureChild> Features { get; set; } = new List<IFeatureChild>();

        #endregion
    }
}