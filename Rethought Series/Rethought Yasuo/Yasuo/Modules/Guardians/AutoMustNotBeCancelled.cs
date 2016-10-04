namespace Rethought_Yasuo.Yasuo.Modules.Guardians
{
    #region Using Directives

    using System;

    using LeagueSharp;

    #endregion

    internal class AutoMustNotBeCancelled : IGuardian
    {
        #region Constructors and Destructors

        public AutoMustNotBeCancelled()
        {
            this.Func = () => ObjectManager.Player.IsWindingUp;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the function.
        /// </summary>
        /// <value>
        ///     The function.
        /// </value>
        public Func<bool> Func { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Checks the function.
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            return this.Func.Invoke();
        }

        #endregion
    }
}