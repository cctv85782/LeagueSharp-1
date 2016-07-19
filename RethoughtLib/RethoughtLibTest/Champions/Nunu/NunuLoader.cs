namespace RethoughtLibTest.Champions.Nunu
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp.Common;

    using RethoughtLib.Classes.Feature;
    using RethoughtLib.Classes.FeatureV2;
    using RethoughtLib.Classes.Intefaces;

    using RethoughtLibTest.Champions.Nunu.Modules.OrbwalkingModes;

    #endregion

    internal class NunuLoader : ILoadable
    {
        #region Fields

        /// <summary>
        ///     The features
        /// </summary>
        private readonly List<IFeatureChild> features = new List<IFeatureChild>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; } = "Nunu";

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public void Load()
        {
            var rootMenu = new Menu("Nunu", "Nunu", true);
            rootMenu.AddToMainMenu();

            GlobalVariables.RootMenu = rootMenu;

            var comboParent = new Parent("Combo");



            comboParent.AddChildren(new Q());

            comboParent.Initialize();
        }

        #endregion
    }
}