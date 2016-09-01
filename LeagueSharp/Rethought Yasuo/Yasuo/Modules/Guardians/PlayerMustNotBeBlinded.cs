using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rethought_Yasuo.Yasuo.Modules.Guardians
{
    using LeagueSharp;

    internal class PlayerMustNotBeBlinded : IGuardian
    {
        public PlayerMustNotBeBlinded()
        {
            this.Func = () => ObjectManager.Player.HasBuffOfType(BuffType.Blind);
        }
        /// <summary>
        ///     Gets or sets the function.
        /// </summary>
        /// <value>
        ///     The function.
        /// </value>
        public Func<bool> Func { get; set; }
    }
}
