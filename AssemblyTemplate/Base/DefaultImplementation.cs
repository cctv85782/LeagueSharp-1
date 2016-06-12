namespace AssemblyName.Base
{
    #region Using Directives

    using System;

    using AssemblyName.MediaLib.Classes;

    #endregion

    /// <summary>
    ///     Default Implementation of IChampion
    /// </summary>
    /// <seealso cref="IChampion" />
    internal class DefaultImplementation : IChampion
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; } = "DefaultImplementation";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public void Load()
        {
            Console.WriteLine(
                string.Format(
                    GlobalVariables.Name
                    + " does not support this champion. No Modules, except base Modules, will be loaded."));
        }

        #endregion
    }
}