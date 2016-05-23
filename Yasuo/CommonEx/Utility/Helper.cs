namespace Yasuo.CommonEx.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Yasuo.Yasuo.LogicProvider;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using SpellDatabase = LeagueSharp.SDK.SpellDatabase;

    public class Helper
    {
        #region Fields

        /// <summary>
        ///     The E logicprovicer
        /// </summary>
        public SweepingBladeLogicProvider ProviderE = new SweepingBladeLogicProvider();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns the center from a given list of units
        /// </summary>
        /// <param name="units"></param>
        /// <returns>Vector2</returns>
        public static Vector2 GetMeanVector2(List<Obj_AI_Base> units)
        {
            if (units.Count == 0)
            {
                return Vector2.Zero;
            }
            float x = 0, y = 0;

            foreach (var unit in units)
            {
                x += unit.ServerPosition.X;
                y += unit.ServerPosition.Y;
            }

            return new Vector2(x / units.Count, y / units.Count);
        }

        /// <summary>
        ///     Returns the center from a given list of vectors
        /// </summary>
        /// <param name="vectors"></param>
        /// <returns>Vector2</returns>
        public static Vector2 GetMeanVector2(List<Vector2> vectors)
        {
            if (vectors.Count == 0)
            {
                return Vector2.Zero;
            }

            float x = 0, y = 0;

            foreach (var vector in vectors)
            {
                x += vector.X;
                y += vector.Y;
            }

            return new Vector2(x / vectors.Count, y / vectors.Count);
        }

        /// <summary>
        ///     Returns the center from a given list of units
        /// </summary>
        /// <param name="units"></param>
        /// <returns>Vector3</returns>
        public static Vector3 GetMeanVector3(List<Obj_AI_Base> units)
        {
            if (units.Count == 0)
            {
                return Vector3.Zero;
            }
            float x = 0, y = 0, z = 0;

            foreach (var unit in units)
            {
                x += unit.ServerPosition.X;
                y += unit.ServerPosition.Y;
                z += unit.ServerPosition.Z;
            }

            return new Vector3(x / units.Count, y / units.Count, z / units.Count);
        }

        /// <summary>
        ///     Returns the center from a given list of vectors
        /// </summary>
        /// <param name="vectors"></param>
        /// <returns>Vector2</returns>
        public static Vector3 GetMeanVector3(List<Vector3> vectors)
        {
            if (vectors.Count == 0)
            {
                return Vector3.Zero;
            }

            float x = 0, y = 0, z = 0;

            foreach (var vector in vectors)
            {
                x += vector.X;
                y += vector.Y;
                z += vector.Z;
            }

            return new Vector3(x / vectors.Count, y / vectors.Count, z / vectors.Count);
        }

        public static float GetPathLenght(Vector3[] path)
        {
            var result = 0f;

            for (var i = 0; i < path.Count(); i++)
            {
                if (i + 1 != path.Count())
                {
                    result += path[i].Distance(path[i + 1]);
                }
            }
            return result;
        }

        public float GetMeanDistance(List<Vector3> vectors, Vector3 vector)
        {
            var result = vectors.Sum(v => v.Distance(vector));

            return result / vectors.Count;
        }

        #endregion

        #region Methods

        internal static float DistanceToTarget(Obj_AI_Base unit)
        {
            return unit.Distance(TargetSelector.GetSelectedTarget());
        }

        /// <summary>
        ///     Returns Missile Speed based on Spell Name
        /// </summary>
        /// <param name="spellName"></param>
        /// <returns>float</returns>
        internal static float GetMissileSpeed(string spellName)
        {
            switch (spellName)
            {
                case "YasuoDash":
                    return 1000 + (ObjectManager.Player.MoveSpeed - 345);
                case "YasuoQ":
                    return float.MaxValue;
                case "YasuoQ2":
                    return 1400;
                default:
                    return SpellDatabase.GetByName(spellName).MissileSpeed;
            }
        }

        /// <summary>
        ///     Returns the Spell Delay based on Spell Name
        /// </summary>
        /// <param name="spellName"></param>
        /// <returns>float</returns>
        internal static float GetSpellDelay(string spellName)
        {
            if (spellName != null)
            {
                return SpellDatabase.GetByName(spellName).Delay;
            }
            return 0;
        }

        /// <summary>
        ///     Returns Spell Range by Spell Name
        /// </summary>
        /// <param name="spellName"></param>
        /// <returns>float</returns>
        internal static float GetSpellRange(string spellName)
        {
            if (spellName != null)
            {
                return SpellDatabase.GetByName(spellName).Range;
            }
            return 0;
        }

        /// <summary>
        ///     Returns Spell Range by Missile Name
        /// </summary>
        /// <param name="missileName"></param>
        /// <returns>float</returns>
        internal static float GetSpellRange2(string missileName)
        {
            if (missileName != null)
            {
                return SpellDatabase.GetByMissileName(missileName).Range;
            }
            return 0;
        }

        /// <summary>
        ///     Returns Spell Width based on Spell Name
        /// </summary>
        /// <param name="spellName"></param>
        /// <returns>float</returns>
        internal static float GetSpellWidth(string spellName)
        {
            if (spellName == "YasuoWMovingWall")
            {
                return (250 + (50 * GlobalVariables.Spells[SpellSlot.W].Level));
            }
            if (spellName == "YasuoQ")
            {
                return 20;
            }
            if (spellName == "YasuoQ2")
            {
                return 90;
            }
            return spellName != null ? SpellDatabase.GetByName(spellName).Width : 0;
        }

        internal static float GetTick()
        {
            return (int)DateTime.Now.Subtract(GlobalVariables.AssemblyLoadTime).TotalMilliseconds;
        }

        #endregion
    }
}