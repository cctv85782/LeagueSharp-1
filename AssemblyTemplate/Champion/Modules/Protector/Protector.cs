namespace AssemblyName.Champion.Modules.Protector
{
    #region Using Directives

    using AssemblyName.MediaLib.Classes.Feature;

    #endregion

    /// <summary>
    ///     Protector Parent
    /// </summary>
    /// <seealso cref="AssemblyName.MediaLib.Classes.Feature.FeatureParent" />
    internal class Protector : FeatureParent
    {
        #region Public Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name => "Protector";

        #endregion
    }
}