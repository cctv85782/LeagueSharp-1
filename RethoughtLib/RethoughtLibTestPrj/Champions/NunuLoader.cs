namespace RethoughtLibTestPrj.Champions
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp.Common;

    using RethoughtLib.Classes.Feature;
    using RethoughtLib.Classes.Intefaces;

    using RethoughtLibTestPrj.Champions.Modules;

    #endregion

    internal class NunuLoader : ILoadable
    {
        #region Fields

        /// <summary>
        ///     The features
        /// </summary>
        private readonly List<IFeatureChild> features = new List<IFeatureChild>();

        #endregion

        #region Constructors and Destructors

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
            
            var combo = new Combo(rootMenu);

            this.features.AddRange(new List<IFeatureChild>() { new Q(combo) });

            foreach (var feature in this.features)
            {
                Console.WriteLine("Handling " + feature.Name + " feature");
                feature.HandleEvents();
            }
        }

        #endregion

        #region Public Methods and Operators

        #endregion
    }
}