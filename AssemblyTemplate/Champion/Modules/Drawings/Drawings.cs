namespace AssemblyName.Champion.Modules.Drawings
{
    #region Using Directives

    using AssemblyName.MediaLib.Classes.Feature;

    #endregion

    /// <summary>
    ///     Drawings Parent
    /// </summary>
    /// <seealso cref="AssemblyName.MediaLib.Classes.Feature.FeatureParent" />
    internal class Drawings : FeatureParent
    {
        #region Public Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name => "Drawings";

        #endregion
    }
}