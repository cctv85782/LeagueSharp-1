namespace Yasuo.Common.Provider
{
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.SDK;

    using SebbyLib.Prediction;

    using SharpDX;

    using PredictionInput = SebbyLib.Prediction.PredictionInput;
    using PredictionOutput = SebbyLib.Prediction.PredictionOutput;

    public class SteelTempestLogicProvider
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Returns the remaining buff time of Q2/23
        /// </summary>
        /// <returns>float</returns>
        public float BuffTime()
        {
            if (this.HasQ3())
            {
                var first = GlobalVariables.Player.Buffs.FirstOrDefault(x => x.Name.Contains("yasuoq3w"));
                if (first != null)
                {
                    return first.EndTime - Game.Time;
                }
            }
            return float.MaxValue;
        }

        /// <summary>
        ///     Returns the correct amount of damage on X unit
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>float</returns>
        public float GetDamage(Obj_AI_Base unit)
        {
            var physicalDmg = 0f;
            var magicDmg = 0f;

            #region Sheen

            if (Items.HasItem((int)ItemId.Sheen)
                && (Items.CanUseItem((int)ItemId.Sheen) || GlobalVariables.Player.HasBuff("Sheen")))
            {
                physicalDmg = GlobalVariables.Player.BaseAttackDamage;
            }

            #endregion Sheen

            #region Trinity Force

            if (Items.HasItem((int)ItemId.Trinity_Force)
                && (Items.CanUseItem((int)ItemId.Trinity_Force) || GlobalVariables.Player.HasBuff("Sheen")))
            {
                physicalDmg = GlobalVariables.Player.BaseAttackDamage * 2;
            }

            #endregion

            #region Statikk Shiv

            if (Items.HasItem((int)ItemId.Statikk_Shiv) && (Items.CanUseItem((int)ItemId.Statikk_Shiv))
                || GlobalVariables.Player.GetBuffCount("StattikShiv") == 100)
            {
                #region unit is Minion

                if (unit is Obj_AI_Minion)
                {
                    magicDmg += 66;
                    switch (GlobalVariables.Player.Level)
                    {
                        case 6:
                            magicDmg += 13.2f;
                            break;
                        case 7:
                            magicDmg += 24.2f;
                            break;
                        case 8:
                            magicDmg += 37.4f;
                            break;
                        case 9:
                            magicDmg += 48.4f;
                            break;
                        case 10:
                            magicDmg += 59.4f;
                            break;
                        case 11:
                            magicDmg += 72.6f;
                            break;
                        case 12:
                            magicDmg += 83.6f;
                            break;
                        case 13:
                            magicDmg += 96.8f;
                            break;
                        case 14:
                            magicDmg += 107.8f;
                            break;
                        case 15:
                            magicDmg += 118.8f;
                            break;
                        case 16:
                            magicDmg += 132f;
                            break;
                        case 17:
                            magicDmg += 143f;
                            break;
                        case 18:
                            magicDmg += 154f;
                            break;
                    }
                }
                    #endregion
                    #region unit is Hero

                else if (unit is Obj_AI_Hero)
                {
                    magicDmg += 30;
                    switch (GlobalVariables.Player.Level)
                    {
                        case 6:
                            magicDmg += 6;
                            break;
                        case 7:
                            magicDmg += 11;
                            break;
                        case 8:
                            magicDmg += 17;
                            break;
                        case 9:
                            magicDmg += 22;
                            break;
                        case 10:
                            magicDmg += 27;
                            break;
                        case 11:
                            magicDmg += 33;
                            break;
                        case 12:
                            magicDmg += 38;
                            break;
                        case 13:
                            magicDmg += 44;
                            break;
                        case 14:
                            magicDmg += 49;
                            break;
                        case 15:
                            magicDmg += 54;
                            break;
                        case 16:
                            magicDmg += 60;
                            break;
                        case 17:
                            magicDmg += 65;
                            break;
                        case 18:
                            magicDmg += 70;
                            break;
                    }
                }

                #endregion
            }

            #endregion

            #region rageblade

            if (Items.HasItem((int)ItemId.Guinsoos_Rageblade) && (Items.CanUseItem((int)ItemId.Guinsoos_Rageblade))
                || GlobalVariables.Player.GetBuffCount("Rageblade") == 8)
            {
                magicDmg += 20
                            + (GlobalVariables.Player.TotalAttackDamage - GlobalVariables.Player.BaseAttackDamage)
                            * 0.15f;
            }

            #endregion

            if (physicalDmg > 0 || magicDmg > 0)
            {
                physicalDmg = (float)GlobalVariables.Player.CalculateDamage(unit, DamageType.Physical, physicalDmg);
                magicDmg = (float)GlobalVariables.Player.CalculateDamage(unit, DamageType.Magical, magicDmg);
            }
            return GlobalVariables.Spells[SpellSlot.Q].GetDamage(unit) + physicalDmg + magicDmg;
        }

        public PredictionOutput GetPrediction(Obj_AI_Base target, bool aoe)
        {
            var predInput = new PredictionInput
                                {
                                    From = GlobalVariables.Player.ServerPosition,
                                    Aoe = aoe,
                                    Collision = GlobalVariables.Spells[SpellSlot.Q].Collision,
                                    Speed = GlobalVariables.Spells[SpellSlot.Q].Speed,
                                    Delay = GlobalVariables.Spells[SpellSlot.Q].Delay,
                                    Radius = GlobalVariables.Spells[SpellSlot.Q].Range,
                                    Unit = target,
                                    Type = SkillshotType.SkillshotLine
                                };

            return Prediction.GetPrediction(predInput);
        }

        public PredictionOutput GetPrediction(Vector3 from, Obj_AI_Base target, bool aoe, float delay)
        {
            var predInput = new PredictionInput
                                {
                                    From = from,
                                    Aoe = aoe,
                                    Collision = GlobalVariables.Spells[SpellSlot.Q].Collision,
                                    Speed = GlobalVariables.Spells[SpellSlot.Q].Speed,
                                    Delay = GlobalVariables.Spells[SpellSlot.Q].Delay + delay,
                                    Radius = GlobalVariables.Spells[SpellSlot.Q].Range,
                                    Unit = target,
                                    Type = SkillshotType.SkillshotLine
                                };

            return Prediction.GetPrediction(predInput);
        }

        public PredictionOutput GetPrediction(Obj_AI_Base target, bool aoe, SkillshotType skillshotType)
        {
            var predInput = new PredictionInput
                                {
                                    From = GlobalVariables.Player.ServerPosition,
                                    Aoe = aoe,
                                    Collision = GlobalVariables.Spells[SpellSlot.Q].Collision,
                                    Speed = GlobalVariables.Spells[SpellSlot.Q].Speed,
                                    Delay = GlobalVariables.Spells[SpellSlot.Q].Delay,
                                    Radius = GlobalVariables.Spells[SpellSlot.Q].Range,
                                    Unit = target,
                                    Type = skillshotType
                                };

            return Prediction.GetPrediction(predInput);
        }

        /// <summary>
        ///     Returns true if Player has Q3
        /// </summary>
        /// <returns>bool</returns>
        public bool HasQ3() => GlobalVariables.Player.HasBuff("yasuoq3w");

        // TODO: PRIORITY MED - HIGH
        /// <summary>
        ///     Returns the static cooldown of Q based on AS
        /// </summary>
        /// <returns>float</returns>
        public float RealCooldown()
        {
            return 0;
        }

        #endregion
    }
}