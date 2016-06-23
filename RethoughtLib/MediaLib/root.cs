namespace RethoughtLib
{
    #region Using Directives

    using RethoughtLib.Classes.Feature;

    #endregion

    internal class Root : FeatureParent
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FeatureParent" /> class.
        /// </summary>
        /// <param name="rootMenu">The root menu. (the generated menu will get attached to it)</param>
        public Root(LeagueSharp.Common.Menu rootMenu)
            : base(rootMenu)
        { }

        #endregion

        #region Public Properties

        public override string Name => "Root";

        #endregion
    }
}