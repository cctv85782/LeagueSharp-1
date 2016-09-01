namespace Rethought_Yasuo.Yasuo.Modules.Guardians
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal class PlayerMustNotHaveBuff : IGuardian
    {
        #region Constructors and Destructors

        public PlayerMustNotHaveBuff(string buff)
        {
            this.Func = () => ObjectManager.Player.HasBuff(buff);
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