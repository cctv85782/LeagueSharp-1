namespace Yasuo.Common.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;
    using SDK = LeagueSharp.SDK;

    using SharpDX;

    using Yasuo.Common.Extensions;

    using Variables = Yasuo.Variables;

    class WindWallLogicProvider
    {
        // TODO: Maybe Block spells that aiming an enemy and are blockable i.e: Lux W
        // TODO: E when W not needed (ie. Ally wont get hit)
        // TODO: E behind W when skillshot is targeted (ie. cait ult) will hit you or next AA will kill you or do much dmg
        // TODO: Anti Gragas Insec (more of a fun thing actually.)
        // TODO: Crit in AA
        // TODO: 1v1 SafeZone logic
        // TODO: Clean up code
        // TODO: Annie Stun, Katarina Ult

        public enum WindWallMode
        {
            Protecting, SelfProtecting 
        }

        /// <summary>
        ///     Returns the optimal cast position for multiple skillshots
        /// </summary>
        /// <param name="skillshots"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Vector3 GetCastPosition(SDK.Skillshot[] skillshots, WindWallMode mode)
        {
            var skillshotDict = new Dictionary<SDK.Skillshot, Vector3>();
            var result = Vector3.Zero;

            foreach (var skillshot in skillshots)
            {
                skillshotDict.Add(skillshot, GetCastPosition(skillshot.MissilePosition(), skillshot.Direction));
            }

            switch (mode)
            {
                case WindWallMode.Protecting:
                    // TODO
                    break;
                case WindWallMode.SelfProtecting:
                    // TODO
                    break;
            }

            return result;
        }

        /// <summary>
        ///     Returns the optimal cast position for one missile
        /// </summary>
        /// <returns></returns>
        public Vector3 GetCastPosition(Vector2 missilePosition, Vector2 direction)
        {
            return Vector3.Zero;
        }

        /// <summary>
        ///     Returns a precautionary position that is supposed to soak the most dmg before it happens
        /// </summary>
        /// <param name="units"></param>
        /// <param name="prediction"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public SebbyLib.Prediction.PredictionOutput GetPrecautionaryPosition(List<Obj_AI_Hero> units, bool prediction = true, float range = 1000)
        {
            var predInput = new SebbyLib.Prediction.PredictionInput
            {
                From = Variables.Player.ServerPosition,
                Aoe = true,
                Collision = Variables.Spells[SpellSlot.W].Collision,
                Speed = 4000,
                Delay = float.MaxValue,
                Radius = range,
                Unit = units.MaxOrDefault(TargetSelector.GetPriority),
                Type = SebbyLib.Prediction.SkillshotType.SkillshotCone
            };

            return SebbyLib.Prediction.Prediction.GetPrediction(predInput); 
        }
    }
}