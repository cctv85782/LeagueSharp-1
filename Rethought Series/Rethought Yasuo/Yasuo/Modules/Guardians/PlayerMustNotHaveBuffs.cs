namespace Rethought_Yasuo.Yasuo.Modules.Guardians
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal class PlayerMustNotHaveBuffs : IGuardian
    {
        #region Constructors and Destructors

        public PlayerMustNotHaveBuffs(IEnumerable<string> buffs)
        {
            this.Func = () => buffs.Any(x => ObjectManager.Player.HasBuff(x));
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