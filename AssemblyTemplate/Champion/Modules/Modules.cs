namespace AssemblyName.Champion.Modules
{
    #region Using Directives

    using AssemblyName.MediaLib.Classes.Feature;

    #endregion

    /// <summary>
    ///     General Module Parent
    /// </summary>
    /// <seealso cref="AssemblyName.MediaLib.Classes.Feature.FeatureParent" />
    internal class Modules : FeatureParent
    {
        #region Public Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name => "Modules";

        #endregion
    }
}