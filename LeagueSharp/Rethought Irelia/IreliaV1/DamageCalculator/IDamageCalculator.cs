using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rethought_Irelia.IreliaV1.DamageCalculator
{
    using LeagueSharp;

    internal interface IDamageCalculator
    {
        /// <summary>
        /// Gets the damage.
        /// </summary>
        /// <param name="target">The get damage.</param>
        /// <returns></returns>
        float GetDamage(Obj_AI_Base target);
    }
}
