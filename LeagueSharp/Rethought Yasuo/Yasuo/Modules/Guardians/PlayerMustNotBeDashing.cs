namespace Rethought_Yasuo.Yasuo.Modules.Guardians
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal class PlayerMustNotBeDashing : IGuardian
    {
        #region Constructors and Destructors

        public PlayerMustNotBeDashing()
        {
            this.Func = () => ObjectManager.Player.IsDashing();
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
    }
}