namespace RethoughtLibTestPrj.Champions
{
    #region Using Directives

    using System.Collections.Generic;

    using RethoughtLib.Classes.Feature;
    using RethoughtLib.Classes.Intefaces;

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

        public NunuLoader()
        {
            this.features.AddRange(new List<IFeatureChild>() { });
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; } = "Nunu";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public void Load()
        {
        }

        #endregion
    }
}