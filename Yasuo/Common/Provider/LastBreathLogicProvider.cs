namespace Yasuo.Common.Provider
{
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;
    using LeagueSharp.SDK.Core.Utils;

    using SharpDX;

    using Yasuo.Common.Extensions;
    using Yasuo.Common.Objects;
    using Yasuo.Common.Utility;

    class LastBreathLogicProvider
    {
        /// <summary>
        ///     Returns the execution where most damage can get dealt
        /// </summary>
        /// <param name="executions">
        ///     LastBreath execution object
        /// </param>
        /// <returns>
        ///     LastBreath execution object
        /// </returns>
        public Objects.LastBreath MostDamageDealt(List<Objects.LastBreath> executions)
        {
            Dictionary<Objects.LastBreath, float> damageDic = new Dictionary<LastBreath, float>();
            
            if (executions != null)
            {
                foreach (var x in executions)
                {
                    damageDic.Add(x, x.DamageDealt);
                }

                return damageDic.MaxOrDefault(x => x.Value).Key;
            }
            return null;
        }

        /// <summary>
        ///     Gets all executions around a execution that are in range of lastbreath
        /// </summary>
        /// <param name="execution">
        ///     LastBreath execution object
        /// </param>
        /// <returns></returns>
        public List<Objects.LastBreath> GetExecutionsAround(Objects.LastBreath execution)
        {
            var result = new List<Objects.LastBreath>();

            foreach (var target in execution.AffectedEnemies)
            {
                result.Add(new LastBreath(target));
            }

            return result;
        }

        /// <summary>
        ///     Returns the execution that has the lowest remaining airbone time
        /// </summary>
        public Objects.LastBreath LeastRemainingAirboneTime(List<Objects.LastBreath> executions)
        {
            if (executions.Count > 0)
            {
                return executions?.MinOrDefault(x => x.MinRemainingAirboneTime);
            }
            return null;
        }

        /// <summary>
        ///     Returns the most safe enemy to use ultimate on
        /// </summary>
        public Objects.LastBreath MostSafety(List<Objects.LastBreath> executions)
        {
            return executions?.MinOrDefault(x => x.DangerValue);
        }

        // TODO
        /// <summary>
        ///     Returns a bool that determines if it is a good time to use lastbreath or not
        /// </summary>
        /// <param name="execution">The current target</param>
        /// <param name="path">The path to the target</param>
        /// <param name="buffer">a time buffer</param>
        /// <returns></returns>
        public bool ShouldCastNow(Objects.LastBreath execution, Objects.Path path = null, int buffer = 10)
        {
            if (execution == null)
            {
                return false;
            }

            // Last possible second ultimate
            if (execution.MinRemainingAirboneTime <= buffer + Game.Ping)
            {
                return true;
            }

            // if no path is given
            if (path == null)
            {
                var playerpath = Variables.Player.GetPath(execution.Target.ServerPosition);
                var playerpathtime = Helper.GetPathLenght(playerpath) / Variables.Player.MoveSpeed;

                // if walking is requires less time than remaining knockup time
                if (playerpathtime <= execution.MinRemainingAirboneTime + buffer + Game.Ping)
                {
                    return false;
                }

                // Instant Ult in 1 v 1 because armor pen and less time for enemiy to get spells up also you can't gapclose
                if (execution.EndPosition.CountEnemiesInRange(1500) == 0 && !execution.Target.InAutoAttackRange())
                {
                    return true;
                }
            }

            // if a path is given and the path time is shorter than the knockup time
            else
            {
                // TODO: Add CountEnemiesInRange
                if (path.PathTime <= execution.MinRemainingAirboneTime + buffer + Game.Ping)
                {
                    return false;
                }
            }

            return execution.MinRemainingAirboneTime <= (Game.Ping + buffer);
        }
    }
}
