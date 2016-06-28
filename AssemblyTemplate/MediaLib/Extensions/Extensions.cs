﻿namespace AssemblyName.MediaLib.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;
    using LeagueSharp.SDK;

    using SharpDX;

    using SpellType = LeagueSharp.SDK.SpellType;

    internal static class Extensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Returns all units in Range
        /// </summary>
        /// <param name="position"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static int CountMinionsInRange(this Vector3 position, float range)
        {
            var minionList = MinionManager.GetMinions(position, range);

            return minionList?.Count ?? 0;
        }

        /// <summary>
        ///     returns true if unit is in AttackRange
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static bool InAutoAttackRange(this Obj_AI_Base unit)
        {
            return unit.Distance(GlobalVariables.Player) <= GlobalVariables.Player.AttackRange;
        }

        /// <summary>
        ///     Returns true if unit is airbone
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool IsAirbone(this Obj_AI_Base unit)
            => unit.HasBuffOfType(BuffType.Knockup) || unit.HasBuffOfType(BuffType.Knockback);

        // TODO: REWORK > Wrong value
        /// <summary>
        ///     Returns the missile position after time time.
        /// </summary>
        public static Vector2 GetMissilePosition(this Skillshot skillshot, bool allowNegative = false, float delay = 0)
        {
            if (!skillshot.HasMissile)
            {
                return Vector2.Zero;
            }

            if (skillshot.SData.SpellType == SpellType.SkillshotLine
                || skillshot.SData.SpellType == SpellType.SkillshotMissileLine)
            {
                var t = Math.Max(0, Utils.TickCount + delay - skillshot.StartTime - skillshot.SData.Delay);
                t =
                    (int)
                    Math.Max(
                        0,
                        Math.Min(
                            skillshot.EndPosition.Distance(skillshot.StartPosition),
                            t * skillshot.SData.MissileSpeed / 1000));
                return skillshot.StartPosition + skillshot.Direction * t;
            }
            return Vector2.Zero;
        }

        /// <exception cref="Exception">A delegate callback throws an exception. </exception>
        public static void RaiseEvent(this EventHandler @event, object sender, EventArgs e)
        {
            @event?.Invoke(sender, e);
        }

        /// <exception cref="Exception">A delegate callback throws an exception. </exception>
        public static void RaiseEvent<T>(this EventHandler<T> @event, object sender, T e) where T : EventArgs
        {
            @event?.Invoke(sender, e);
        }

        /// <summary>
        ///     Returns the remaining airbone time from unit
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static float RemainingAirboneTime(this Obj_AI_Base unit)
        {
            float result = 0;

            foreach (
                var buff in unit.Buffs.Where(buff => buff.Type == BuffType.Knockback || buff.Type == BuffType.Knockup))
            {
                result = buff.EndTime - Game.Time;
            }
            return result * 1000;
        }

        /// <summary>
        ///     Converts a list of Obj_Ai_Base's to Vector3's.
        /// </summary>
        /// <param name="units">The units.</param>
        /// <returns></returns>
        public static List<Vector3> ToVector3S(this List<Obj_AI_Base> units)
        {
            return (from unit in units where unit.IsValid select unit.ServerPosition).ToList();
        }

        #endregion
    }
}