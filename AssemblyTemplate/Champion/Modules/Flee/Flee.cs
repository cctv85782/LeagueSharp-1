namespace AssemblyName.Champion.Modules.Flee
{
    #region Using Directives

    using AssemblyName.MediaLib.Classes.Feature;

    #endregion

    /// <summary>
    ///     Flee Parent
    /// </summary>
    /// <seealso cref="AssemblyName.MediaLib.Classes.Feature.FeatureParent" />
    internal class Flee : FeatureParent
    {
        #region Public Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name => "Dash to Mouse";

        #endregion
    }
}