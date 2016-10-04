namespace Rethought_Yasuo.Yasuo.Modules.Guardians
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    ///     Guardian
    /// </summary>
    internal interface IGuardian
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the function.
        /// </summary>
        /// <value>
        ///     The function.
        /// </value>
        Func<bool> Func { get; set; }

        #endregion
    }
}