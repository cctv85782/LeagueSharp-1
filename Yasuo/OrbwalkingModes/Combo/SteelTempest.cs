namespace Yasuo.OrbwalkingModes.Combo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;
    using SharpDX.Direct3D9;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Extensions;
    using Yasuo.Common.Predictions;
    using Yasuo.Common.Provider;

    using HitChance = Yasuo.Common.Predictions.HitChance;
    using PredictionOutput = SebbyLib.Prediction.PredictionOutput;

    internal class SteelTempest : Child<Combo>
    {
        public SteelTempest(Combo parent)
            : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "Steel Tempest";

        public SteelTempestLogicProvider ProviderQ;

        public SweepingBladeLogicProvider ProviderE;

        protected override void OnEnable()
        {
            Game.OnUpdate += this.OnUpdate;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            Game.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            // Blacklist
            var blacklist = new Menu("Blacklist", this.Name + "Blacklist");

            if (HeroManager.Enemies.Count == 0)
            {
                blacklist.AddItem(new MenuItem(blacklist.Name + "null", "No enemies found"));
            }
            else
            {
                foreach (var x in HeroManager.Enemies)
                {
                    blacklist.AddItem(new MenuItem(blacklist.Name + x.ChampionName, x.ChampionName).SetValue(false));
                }
                MenuExtensions.AddToolTip(
                    blacklist,
                    "Setting a champion to 'on', will make the script not using Q for him anymore");
            }
            this.Menu.AddSubMenu(blacklist);

            // Spell Settings
            // Hit Multiple

            #region MultiKnockup (Dynamic-Menu)

            var multiknockupDyn = new Menu("Multi-Knockup Settings", this.Name + "multiknockupsettings");

            multiknockupDyn.AddItem(new MenuItem(multiknockupDyn.Name + "Enabled", "Enabled").SetValue(true));

            this.Menu.AddSubMenu(multiknockupDyn);

            multiknockupDyn.AddItem(
                new MenuItem(multiknockupDyn.Name + "Mode", "Mode").SetValue(new StringList(new[] { "Custom", "Path Based", "Never" }, 1)))
                .ValueChanged += delegate (object sender, OnValueChangeEventArgs eventArgs)
                {
                    MenuExtensions.RefreshMenu(multiknockupDyn, eventArgs.GetNewValue<StringList>().SelectedIndex + 1);
                };

            // Custom
            multiknockupDyn.AddItem(
                new MenuItem(multiknockupDyn.Name + "BuffState", "Only if: ").SetValue(new StringList(new[] { "Q3 (Stacked)", "Not Stacked", "Always" }, 0))).SetTag(1);

            multiknockupDyn.AddItem(
                new MenuItem(multiknockupDyn.Name + "MinHitAOECustom", "Min HitCount for AOE").SetValue(new Slider(2, 1, 5))).SetTag(1);

            // Path Based
            multiknockupDyn.AddItem(
                new MenuItem(multiknockupDyn.Name + "DisclaimerPathBased", "[i] [Experimental] Assembly will try to decide based on pathing")).SetTag(2);

            multiknockupDyn.AddItem(
                new MenuItem(multiknockupDyn.Name + "BuffStatePathBased", "Only if: ").SetValue(new StringList(new[] { "Q3 (Stacked)", "Not Stacked", "Always" }, 0))).SetTag(2);

            multiknockupDyn.AddItem(
                new MenuItem(multiknockupDyn.Name + "MinHitAOEPathBased", "Min HitCount for AOE").SetValue(new Slider(2, 1, 5))).SetTag(2);

            // Never
            multiknockupDyn.AddItem(
                new MenuItem(multiknockupDyn.Name + "DisclaimerPathBased", "[i] Never aim for multiple targets")).SetTag(3);

            #endregion

            #region Stacking (Dynamic-Menu)

            var stacksettingsDyn = new Menu("Stack Settings", this.Name + "stacksettings");

            stacksettingsDyn.AddItem(new MenuItem(stacksettingsDyn.Name + "Enabled", "Enabled").SetValue(true));

            this.Menu.AddSubMenu(stacksettingsDyn);

            stacksettingsDyn.AddItem(
                new MenuItem(stacksettingsDyn.Name + "Mode", "Mode").SetValue(new StringList(new[] { "Custom", "Always (Brain-Dead)", "Path Based"}, 2)))
                .ValueChanged += delegate (object sender, OnValueChangeEventArgs eventArgs)
                    {
                        MenuExtensions.RefreshMenu(stacksettingsDyn, eventArgs.GetNewValue<StringList>().SelectedIndex + 1);
                    };

            // Custom
            stacksettingsDyn.AddItem(
                new MenuItem(stacksettingsDyn.Name + "MinDistance", "Don't Stack if Distance to enemy <= ").SetValue(
                    new Slider(1000, 0, 4000)).SetTag(1));

            stacksettingsDyn.AddItem(
                new MenuItem(stacksettingsDyn.Name + "MinCooldownQ", "Don't Stack if Q Cooldown is >= (milliseconds)").SetValue(
                    new Slider(1700, 1333, 5000)).SetTag(1));

            // Always
            stacksettingsDyn.AddItem(
                new MenuItem(stacksettingsDyn.Name + "Disclaimer", "[i] Can't recommend using this").SetTag(2));

            // Path Based
            stacksettingsDyn.AddItem(
                new MenuItem(stacksettingsDyn.Name + "Information", "[i] information")
                    .SetTooltip("If this is enabled, the assembly will stack based on the current gapclose path. Currently here are no options, but if I got enough time and motivation I will add some.").SetTag(3));

            MenuExtensions.RefreshMenu(stacksettingsDyn, stacksettingsDyn.Item(stacksettingsDyn.Name + "Mode").GetValue<StringList>().SelectedIndex + 1);

            #endregion

            this.Menu.AddItem(
                 new MenuItem(this.Name + "Disc", "DISCLAIMER")
                     .SetTooltip(
                            "I know that stacking is a little but bugged, as well as EQ some times is. Prediciton seems also a little bit off, will fix it tmrw or sth"));



            // Prediction Mode
            //this.Menu.AddItem(new MenuItem(this.Name + "Prediction", "Prediction").SetValue(new StringList(Variables.Predictions, 0)));
            //Menu.AddItem(new MenuItem(Name + "Prediction Mode", "Prediction Mode").SetValue(new Slider(5, 0, 0)));
            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.ProviderQ = new SteelTempestLogicProvider();
            this.ProviderE = new SweepingBladeLogicProvider();

            base.OnInitialize();
        }

        // ReSharper disable once FunctionComplexityOverflow
        public void OnUpdate(EventArgs args)
        {
            try
            {
                if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !Variables.Spells[SpellSlot.Q].IsReady())
                {
                    return;
                }

                var target = TargetSelector.GetTarget(Variables.Spells[SpellSlot.Q].Range, TargetSelector.DamageType.Physical);

                #region EQ

                if (target != null && target.IsValid)
                {
                    if (Variables.Player.IsDashing() && target.Distance(ObjectManager.Player.ServerPosition) <= 350)
                    {
                        Execute(target);
                    }

                    if (!Variables.Player.IsDashing())
                    {
                        if (this.Menu.Item(this.Name + "AOE").GetValue<bool>()
                            && Variables.Player.CountEnemiesInRange(Variables.Spells[SpellSlot.Q].Range)
                            >= this.Menu.Item(this.Name + "MinHitAOE").GetValue<Slider>().Value)
                        {
                            Execute(target, Variables.Player.HasQ3());
                        }
                        Execute(target, Variables.Player.HasQ3());
                    }
                }


                #endregion

                #region AOE

                var multiknockupsettings = this.Menu.SubMenu(this.Name + "multiknockupsettings");

                var targets = HeroManager.Enemies.Where(x => x.IsValid && x.Distance(Variables.Player) <= Variables.Spells[SpellSlot.Q].Range).ToList();

                var preds = targets.Select(unit => this.ProviderQ.GetPrediction(unit, true)).ToList();

                if (preds.Any())
                {
                    switch (multiknockupsettings.Item(multiknockupsettings.Name + "Mode").GetValue<StringList>().SelectedIndex)
                    {
                        // Custom
                        case 0:
                            var mostKnockedUp = preds.MaxOrDefault(x => x.AoeTargetsHitCount);

                            if (mostKnockedUp != null && mostKnockedUp.AoeTargetsHitCount >= 
                                multiknockupsettings.Item(multiknockupsettings.Name + "MinHitAOECustom").GetValue<Slider>().Value)
                            {
                                Variables.Spells[SpellSlot.Q].Cast(mostKnockedUp.CastPosition);
                            }

                            break;

                        // Path Based
                        case 1:
                            var path = SweepingBlade.PathCopy;
                            
                            // TODO
                            var vectors = new List<Vector3>();

                            var predDic = new Dictionary<Vector3, List<SebbyLib.Prediction.PredictionOutput>>();

                            if (vectors.Count > 0)
                            {
                                foreach (var vector in vectors)
                                {
                                    var predsVec = new List<SebbyLib.Prediction.PredictionOutput>();

                                    predDic.Add(vector, predsVec);
                                }
                            }

                            var best = new SebbyLib.Prediction.PredictionOutput();

                            foreach (var vecPred in predDic)
                            {
                                var value = vecPred.Value;
                                var score = 0;

                                foreach (var pred in value)
                                {
                                    var prioTargets = pred.AoeTargetsHit.Sum(x => TargetSelector.GetPriority(x));

                                    if (best.AoeTargetsHitCount < pred.AoeTargetsHitCount)
                                    {
                                        best = pred;
                                    }
                                }
                            }
                            break;

                        // Never
                        case 2:
                            break;
                    }
                }

                #endregion

                #region Stacking

                // Stacking
                var stacksettingsMenu = this.Menu.SubMenu(this.Name + "stacksettings");

                if (stacksettingsMenu.Item(stacksettingsMenu.Name + "Enabled").GetValue<bool>() && !Variables.Player.HasQ3())
                {
                    var units = this.ProviderE.GetUnits(Variables.Player.ServerPosition);

                    switch (stacksettingsMenu.Item(stacksettingsMenu.Name + "Mode").GetValue<StringList>().SelectedIndex)
                    {
                        // Custom
                        case 0:
                            var nearesthero = HeroManager.Enemies.Where(x => !x.IsDead || !x.IsZombie).MinOrDefault(x => x.Distance(Variables.Player));

                            // if we are X further away from the closest enemy
                            if (nearesthero != null)
                            {
                                if (Variables.Player.ServerPosition.Distance(nearesthero.ServerPosition) >= stacksettingsMenu.Item(stacksettingsMenu.Name + "MinDistance").GetValue<Slider>().Value)
                                {
                                    if (Variables.Spells[SpellSlot.Q].Cooldown <= stacksettingsMenu.Item(stacksettingsMenu.Name + "MinCooldownQ").GetValue<Slider>().Value)
                                    {
                                        if (units.Count > 0)
                                        {
                                            var unitsNotMoving = units.Where(x => !x.IsMoving && x.Distance(Variables.Player) <= Variables.Spells[SpellSlot.Q].Range).ToList();

                                            bool any = unitsNotMoving.Any();

                                            if (any)
                                            {
                                                Execute(unitsNotMoving.MinOrDefault(x => x.Distance(Variables.Player)));
                                            }
                                            else
                                            {
                                                var unit = units.Where(x => x.Distance(Variables.Player) <= Variables.Spells[SpellSlot.Q].Range).MinOrDefault(x => x.Distance(Variables.Player));

                                                if (unit != null)
                                                {
                                                    Game.PrintChat("Stacking Case0");
                                                    Execute(unit);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;

                        // Always
                        case 1:
                            if (units.Count > 0)
                            {
                                var unit = units.MinOrDefault(x => x.Distance(Variables.Player));

                                if (unit != null
                                    && unit.Distance(Variables.Player) <= Variables.Spells[SpellSlot.Q].Range)
                                {
                                    Game.PrintChat("Stacking Case1");
                                    Execute(unit);
                                }
                            }
                            break;

                        // Path Based
                        case 2:
                            var path = SweepingBlade.PathCopy;

                            if (path == null)
                            {
                                return;
                            }


                            foreach (var unit in units.ToList())
                            {
                                if (path.Connections.Any(x => x.Over.Equals(unit) || x.Over == unit))
                                {
                                    units.Remove(unit);
                                }
                            }

                            if (Variables.Player.CountEnemiesInRange(Variables.Spells[SpellSlot.Q].Range) == 0 &&
                                path.PathTime + 500 > Variables.Spells[SpellSlot.Q].Cooldown)
                            {
                                if (path.Connections.Any(x =>
                                        x.IsDash
                                        && x.Over.Health > ProviderE.GetDamage(x.Over) + ProviderQ.GetDamage(x.Over)))
                                {
                                    if (Variables.Player.IsDashing())
                                    {
                                        Execute(path.Connections.FirstOrDefault(x => x.Over != null)?.Over);
                                    }
                                }

                                else if (units.Count > 0)
                                {
                                    var unitsNotMoving = units.Where(x => !x.IsMoving && x.Distance(Variables.Player) <= Variables.Spells[SpellSlot.Q].Range).ToList();

                                    if (unitsNotMoving.Any())
                                    {
                                        Execute(unitsNotMoving.MinOrDefault(x => x.Distance(Variables.Player)));
                                    }
                                    else
                                    {
                                        var unit = units.MinOrDefault(x => x.Distance(Variables.Player));

                                        if (unit != null
                                            && unit.Distance(Variables.Player) <= Variables.Spells[SpellSlot.Q].Range)
                                        {
                                            Game.PrintChat("Stacking Case2");
                                            Execute(unit);
                                        }
                                    }
                                }
                            }

                            break;
                    }

                }

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void Execute(Obj_AI_Base target, bool aoe = false)
        {
            var predictionOutput = ProviderQ.GetPrediction(target, aoe);

            if (predictionOutput.Hitchance >= SebbyLib.Prediction.HitChance.High)
            {
                Variables.Spells[SpellSlot.Q].Cast(predictionOutput.UnitPosition);
            }
        }
    }
}
