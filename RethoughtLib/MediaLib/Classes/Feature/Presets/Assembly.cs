namespace RethoughtLib.Classes.Feature.Presets
{
    #region Using Directives

    using LeagueSharp.Common;

    #endregion

    public class Assembly : FeatureParent
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FeatureParent" /> class.
        /// </summary>
        /// <param name="rootMenu">The root menu. (the generated menu will get attached to it)</param>
        public Assembly(Menu rootMenu)
            : base(rootMenu)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Assembly";

        #endregion
    }
}