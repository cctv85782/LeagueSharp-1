using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RethoughtLib.TargetSelector.Implementations
{
    using global::RethoughtLib.TargetSelector.Interfaces;

    using LeagueSharp;
    using LeagueSharp.Common;

    public class LessAttack : ITargetSelectionMode
    {
        public string Name { get; set; } = "Less Attack";

        public Obj_AI_Hero GetTarget(List<Obj_AI_Hero> targets, Obj_AI_Hero requester)
        {
            var results = new Dictionary<Obj_AI_Hero, double>();

            foreach (var target in targets)
            {
                var targetHealth = target.Health;

                while (targetHealth > 0)
                {
                    targetHealth -= (float) requester.GetAutoAttackDamage(target);
                }
            }
            return targets.MinOrDefault(x => x.Health / requester.GetAutoAttackDamage(x, true));
        }
    }
}
