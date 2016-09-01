namespace Rethought_Yasuo.Yasuo.Modules.Core.SpellParent.Implementations
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Switches;

    using SharpDX;

    using Prediction = SebbyLib.Prediction.Prediction;
    using PredictionInput = SebbyLib.Prediction.PredictionInput;
    using PredictionOutput = SebbyLib.Prediction.PredictionOutput;
    using SkillshotType = SebbyLib.Prediction.SkillshotType;

    #endregion

    internal class YasuoQ : SpellChild
    {
        #region Public Properties

        public string ChargedBuffName { get; set; } = "yasuoq3w";

        public bool IsCharged => ObjectManager.Player.HasBuff(this.ChargedBuffName);

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Steel Tempest";

        /// <summary>
        ///     Gets or sets the spell.
        /// </summary>
        /// <value>
        ///     The spell.
        /// </value>
        public override Spell Spell { get; set; }

        #endregion

        #region Properties

        private Func<float> QDelayFunc { get; set; } =
            () => (float)(1 - Math.Min((ObjectManager.Player.AttackSpeedMod - 1) / 0.172, 0.66f));

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns whether Yasuo has charged Q
        /// </summary>
        /// <returns></returns>
        public bool Charged()
        {
            return ObjectManager.Player.HasBuff("YasuoQ3W");
        }

        /// <summary>
        ///     Returns the correct amount of damage on X unit
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>float</returns>
        public float GetDamage(Obj_AI_Base unit)
        {
            if (unit == null)
            {
                return 0;
            }

            var physicalDmg = 0f;
            var magicDmg = 0f;

            //var data = LeagueSharp.Data.Data.Get<ItemDatabase>();

            //foreach (var item in ObjectManager.Player.InventoryItems)
            //{
            //    if (ObjectManager.Player.Spellbook.CanUseSpell((SpellSlot)item.Slot + (int)SpellSlot.Item1)
            //        == SpellState.Ready)
            //    {
            //        continue;
            //    }

            //    if (data[item.Id].Tags.Contains("OnHit"))
            //    {

            //    }
            //}

            //if (Items.HasItem((int)ItemId.Sheen)
            //    && (Items.CanUseItem((int)ItemId.Sheen) || GlobalVariables.Player.HasBuff("Sheen")))
            //{
            //    physicalDmg = GlobalVariables.Player.BaseAttackDamage;
            //}

            if (Items.HasItem((int)ItemId.Trinity_Force)
                && (Items.CanUseItem((int)ItemId.Trinity_Force) || ObjectManager.Player.HasBuff("Sheen")))
            {
                physicalDmg = ObjectManager.Player.BaseAttackDamage * 2;
            }

            if (Items.HasItem((int)ItemId.Statikk_Shiv) && Items.CanUseItem((int)ItemId.Statikk_Shiv))
            {
                if (unit is Obj_AI_Minion)
                {
                    magicDmg += 66;
                    switch (ObjectManager.Player.Level)
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
                        default:
                            break;
                    }
                }

                else if (unit is Obj_AI_Hero)
                {
                    magicDmg += 30;
                    switch (ObjectManager.Player.Level)
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
                        default:
                            break;
                    }
                }
            }

            if (Items.HasItem((int)ItemId.Guinsoos_Rageblade) && Items.CanUseItem((int)ItemId.Guinsoos_Rageblade)
                || ObjectManager.Player.GetBuffCount("Rageblade") == 8)
            {
                magicDmg += 20
                            + (ObjectManager.Player.TotalAttackDamage - ObjectManager.Player.BaseAttackDamage) * 0.15f;
            }

            if (physicalDmg > 0 || magicDmg > 0)
            {
                physicalDmg = (float)ObjectManager.Player.CalcDamage(unit, Damage.DamageType.Physical, physicalDmg);
                magicDmg = (float)ObjectManager.Player.CalcDamage(unit, Damage.DamageType.Magical, magicDmg);
            }

            return this.Spell.GetDamage(unit) + physicalDmg + magicDmg;
        }

        public PredictionOutput GetPrediction(Obj_AI_Base target, bool aoe = false)
        {
            var predInput = new PredictionInput
                                {
                                    From = ObjectManager.Player.ServerPosition,
                                    Aoe = aoe,
                                    Collision = this.Spell.Collision,
                                    Speed = this.Spell.Speed,
                                    Delay = this.Spell.Delay,
                                    Radius = this.Spell.Range,
                                    Unit = target,
                                    Type = SkillshotType.SkillshotLine
                                };

            Console.WriteLine(this.Spell.Speed);
            Console.WriteLine(this.Spell.Delay);
            Console.WriteLine(this.Spell.Range);
            Console.WriteLine(this.Spell.Collision);

            return Prediction.GetPrediction(predInput);
        }

        public PredictionOutput GetPrediction(Vector3 from, Obj_AI_Base target, bool aoe, float delay)
        {
            var predInput = new PredictionInput
                                {
                                    From = from,
                                    Aoe = aoe,
                                    Collision = this.Spell.Collision,
                                    Speed = this.Spell.Speed,
                                    Delay = this.Spell.Delay + delay,
                                    Radius = this.Spell.Range,
                                    Unit = target,
                                    Type = SkillshotType.SkillshotLine
                                };

            return Prediction.GetPrediction(predInput);
        }

        public PredictionOutput GetPrediction(Obj_AI_Base target, bool aoe, SkillshotType skillshotType)
        {
            var predInput = new PredictionInput
                                {
                                    From = ObjectManager.Player.ServerPosition,
                                    Aoe = aoe,
                                    Collision = this.Spell.Collision,
                                    Speed = this.Spell.Speed,
                                    Delay = this.Spell.Delay,
                                    Radius = this.Spell.Range,
                                    Unit = target,
                                    Type = skillshotType
                                };

            return Prediction.GetPrediction(predInput);
        }

        /// <summary>
        ///     Gets the q stacks.
        /// </summary>
        /// <returns></returns>
        public int GetStacks()
        {
            return 0;
            // TODO:
            //var instance = ObjectManager.Player.GetBuff("YasuoQ3W");

            //return instance?.Count ?? 0;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Game.OnUpdate -= this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Game.OnUpdate += this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [initialize].
        /// </summary>
        protected override void OnInitialize(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnInitialize(sender, featureBaseEventArgs);
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Spell = new Spell(SpellSlot.Q);
        }

        /// <summary>
        ///     Initializes the menu
        /// </summary>
        protected override void SetMenu()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Switch = new BoolSwitch(this.Menu, "Auto-Updating", true, this);
        }

        private void GameOnOnUpdate(EventArgs args)
        {
            if (!ObjectManager.Player.IsDashing())
            {
                if (this.Charged())
                {
                    this.Spell = new Spell(SpellSlot.Q, 950);
                    this.Spell.SetSkillshot(
                        this.QDelayFunc.Invoke(),
                        90,
                        1250,
                        false,
                        LeagueSharp.Common.SkillshotType.SkillshotLine);
                }
                else
                {
                    this.Spell = new Spell(SpellSlot.Q, 475);
                    this.Spell.SetSkillshot(
                        this.QDelayFunc.Invoke(),
                        20,
                        10000,
                        false,
                        LeagueSharp.Common.SkillshotType.SkillshotLine);
                }
            }
            else
            {
                this.Spell = new Spell(SpellSlot.Q, 0);
                this.Spell.SetSkillshot(0, 350, 5000, false, LeagueSharp.Common.SkillshotType.SkillshotCircle);
            }
        }

        #endregion
    }
}