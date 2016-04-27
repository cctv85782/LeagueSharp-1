namespace Yasuo.OrbwalkingModes.Combo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Extensions;
    using Yasuo.Common.Extensions.MenuExtensions;
    using Yasuo.Common.Provider;

    using Dash = Yasuo.Common.Objects.Dash;
    using HitChance = SebbyLib.Prediction.HitChance;
    using PredictionOutput = SebbyLib.Prediction.PredictionOutput;

    internal class SteelTempest : Child<Combo>
    {
        #region Fields

        /// <summary>
        ///     The E logicprovider
        /// </summary>
        public SweepingBladeLogicProvider ProviderE;

        /// <summary>
        ///     The Q logicprovider
        /// </summary>
        public SteelTempestLogicProvider ProviderQ;

        /// <summary>
        ///     The blacklist
        /// </summary>
        internal Blacklist Blacklist;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SteelTempest" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public SteelTempest(Combo parent)
            : base(parent)
        {
            this.OnLoad();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name => "(Q) Steel Tempest";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Method to Multi-KnockUp.
        /// </summary>
        /// <param name="units">The units.</param>
        public void MultiKnockup(List<Obj_AI_Hero> units)
        {
            var multiknockupsettings = this.Menu.SubMenu(this.Name + "multiknockupsettings");

            var preds = new List<PredictionOutput>();

            if (units.Any())
            {
                preds = units.Select(unit => this.ProviderQ.GetPrediction(unit, true)).ToList();
            }

            if (preds.Count > 0)
            {
                switch (
                    multiknockupsettings.Item(multiknockupsettings.Name + "Mode").GetValue<StringList>().SelectedIndex)
                {
                    // Custom
                    case 0:
                        var mostKnockedUp = preds.MaxOrDefault(x => x.AoeTargetsHitCount);

                        if (mostKnockedUp != null
                            && mostKnockedUp.AoeTargetsHitCount
                            >= multiknockupsettings.Item(multiknockupsettings.Name + "MinHitAOECustom")
                                   .GetValue<Slider>()
                                   .Value)
                        {
                            GlobalVariables.Spells[SpellSlot.Q].Cast(mostKnockedUp.CastPosition);
                        }

                        break;

                    // Path Based
                    // TODO: ADD calculation for arriving to that point on the path
                    case 1:
                        var path = SweepingBlade.PathCopy;

                        // TODO
                        var vectors =
                            path.SplitIntoVectors(multiknockupsettings.Item("SegmentAmount").GetValue<Slider>().Value);

                        var predDic = new Dictionary<Vector3, List<PredictionOutput>>();

                        if (vectors.Count > 0)
                        {
                            foreach (var vector in vectors)
                            {
                                var predsVec =
                                    units.ToList()
                                        .Select(unit => this.ProviderQ.GetPrediction(vector, unit, true, 0))
                                        .ToList();

                                if (predsVec != null)
                                {
                                    predDic.Add(vector, predsVec);
                                }
                            }
                        }

                        var scoreDic = new Dictionary<PredictionOutput, float>();

                        foreach (var entry in predDic.ToList())
                        {
                            var value = entry.Value;

                            foreach (var pred in value.ToList())
                            {
                                if (pred.Hitchance < HitChance.High)
                                {
                                    continue;
                                }

                                switch (multiknockupsettings.Item("PriorityMode").GetValue<StringList>().SelectedIndex)
                                {
                                    // Champion Priority (Target Selector)
                                    case 0:
                                        scoreDic.Add(pred, pred.AoeTargetsHit.Sum(x => TargetSelector.GetPriority(x)));
                                        break;
                                    // Killability
                                    case 1:
                                        scoreDic.Add(pred, pred.AoeTargetsHit.Sum(x => TargetSelector.GetPriority(x)));
                                        break;
                                    // Something smart that takes many things into consideration
                                    //case 2:
                                    //    break;
                                }
                            }
                        }

                        PredictionOutput finalPred = null;

                        if (scoreDic.Any())
                        {
                            finalPred = scoreDic.MaxOrDefault(x => x.Value).Key;
                        }

                        if (finalPred != null)
                        {
                            GlobalVariables.Spells[SpellSlot.Q].Cast(finalPred.CastPosition);
                        }
                        break;
                }
            }
        }

        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnUpdate(EventArgs args)
        {
            try
            {
                if (GlobalVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                    || !GlobalVariables.Spells[SpellSlot.Q].IsReady())
                {
                    return;
                }

                var target = TargetSelector.GetTarget(
                    GlobalVariables.Spells[SpellSlot.Q].Range,
                    TargetSelector.DamageType.Physical);

                var targets =
                    HeroManager.Enemies.Where(
                        x => x.Distance(GlobalVariables.Player) <= GlobalVariables.Spells[SpellSlot.Q].Range).ToList();

                this.SingleTarget(target);
                this.MultiKnockup(targets);
                this.Stacking();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        // TODO: Non-Dash
        /// <summary>
        ///     Method to Q single targets.
        /// </summary>
        /// <param name="target">The target.</param>
        public void SingleTarget(Obj_AI_Base target)
        {
            if (target == null || !target.IsValid)
            {
                return;
            }

            if (GlobalVariables.Player.IsDashing())
            {
                var dash = new Dash(GlobalVariables.Player.GetDashInfo().Unit);

                if (dash.EndPosition.Distance(target.ServerPosition) <= 350)
                {
                    if (GlobalVariables.Debug)
                    {
                        Console.WriteLine(@"OrbwalkingModes > Combo > Steel Tempest > EQ Trigger");
                    }
                    this.Execute(target);
                }
            }
        }

        /// <summary>
        ///     Method to stack Q.
        /// </summary>
        public void Stacking()
        {
            var stacksettingsMenu = this.Menu.SubMenu(this.Name + "stacksettings");

            if (stacksettingsMenu.Item(stacksettingsMenu.Name + "Mode").GetValue<StringList>().SelectedValue
                == "Disabled" || GlobalVariables.Player.HasQ3())
            {
                return;
            }

            var units = this.ProviderE.GetUnits(GlobalVariables.Player.ServerPosition);

            var path = SweepingBlade.PathCopy;

            switch (stacksettingsMenu.Item(stacksettingsMenu.Name + "Mode").GetValue<StringList>().SelectedIndex)
            {
                    #region Mode: Custom

                case 0:
                    var nearesthero =
                        HeroManager.Enemies.Where(x => !x.IsDead || !x.IsZombie)
                            .MinOrDefault(x => x.Distance(GlobalVariables.Player));

                    // if we are X further away from the closest enemy
                    if (nearesthero != null
                        && nearesthero.Distance(GlobalVariables.Player.ServerPosition)
                        <= stacksettingsMenu.Item(stacksettingsMenu.Name + "MaxDistance").GetValue<Slider>().Value)
                    {
                        if (GlobalVariables.Player.ServerPosition.Distance(nearesthero.ServerPosition)
                            >= stacksettingsMenu.Item(stacksettingsMenu.Name + "MinDistance").GetValue<Slider>().Value)
                        {
                            if (this.ProviderQ.RealCooldown()
                                <= stacksettingsMenu.Item(stacksettingsMenu.Name + "MinCooldownQ")
                                       .GetValue<Slider>()
                                       .Value)
                            {
                                if (units.Any())
                                {
                                    if (stacksettingsMenu.Item(stacksettingsMenu.Name + "CarePath").GetValue<bool>()
                                        && path.Connections.Any(x => x.Unit != null))
                                    {
                                        foreach (var unit in units.ToList())
                                        {
                                            if (path.Connections.Any(x => x.Unit.Equals(unit) || x.Unit == unit))
                                            {
                                                Game.PrintChat("Test");
                                                units.Remove(unit);
                                            }
                                        }
                                    }

                                    var unitsNotMoving =
                                        units.Where(
                                            x =>
                                            !x.IsMoving
                                            && x.Distance(GlobalVariables.Player)
                                            <= GlobalVariables.Spells[SpellSlot.Q].Range).ToList();

                                    if (unitsNotMoving.Any())
                                    {
                                        var unit = unitsNotMoving.MinOrDefault(x => x.Health);

                                        if (unit != null)
                                        {
                                            if (GlobalVariables.Debug)
                                            {
                                                Console.WriteLine(
                                                    @"OrbwalkingModes > Combo > SteelTempest > Stacking > Case 0: not moving units");
                                            }

                                            this.Execute(unit);
                                        }
                                    }
                                    else
                                    {
                                        var unit =
                                            units.Where(x => GlobalVariables.Spells[SpellSlot.Q].IsInRange(x))
                                                .MinOrDefault(x => x.Health);

                                        if (unit != null)
                                        {
                                            if (GlobalVariables.Debug)
                                            {
                                                Console.WriteLine(
                                                    @"OrbwalkingModes > Combo > SteelTempest > Stacking > Case 0: moving units");
                                            }

                                            this.Execute(unit);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;

                    #endregion

                    #region Mode: Always

                case 1:
                    if (units.Any())
                    {
                        var unitsNotMoving =
                            units.Where(
                                x =>
                                !x.IsMoving
                                && x.Distance(GlobalVariables.Player) <= GlobalVariables.Spells[SpellSlot.Q].Range)
                                .ToList();

                        if (unitsNotMoving.Any())
                        {
                            var unit = unitsNotMoving.MinOrDefault(x => x.Health);

                            if (unit != null)
                            {
                                if (GlobalVariables.Debug)
                                {
                                    Console.WriteLine(
                                        @"OrbwalkingModes > Combo > SteelTempest > Stacking > Case 1: not moving units");
                                }

                                this.Execute(unit);
                            }
                        }
                        else
                        {
                            var unit =
                                units.Where(x => GlobalVariables.Spells[SpellSlot.Q].IsInRange(x))
                                    .MinOrDefault(x => x.Health);

                            if (unit != null)
                            {
                                if (GlobalVariables.Debug)
                                {
                                    Console.WriteLine(
                                        @"OrbwalkingModes > Combo > SteelTempest > Stacking > Case 1: moving units");
                                }

                                this.Execute(unit);
                            }
                        }
                    }
                    break;

                    #endregion

                    #region Mode: Path Based

                case 2:
                    if (path != null && units.Any())
                    {
                        foreach (var unit in units.ToList())
                        {
                            if (path.Connections.Any(x => x.Unit.Equals(unit) || x.Unit == unit))
                            {
                                units.Remove(unit);
                            }
                        }

                        if (GlobalVariables.Player.CountEnemiesInRange(GlobalVariables.Spells[SpellSlot.Q].Range) == 0
                            && path.PathTime + 500 > GlobalVariables.Spells[SpellSlot.Q].Cooldown)
                        {
                            if (
                                path.Connections.Any(
                                    x =>
                                    x.IsDash
                                    && x.Unit.Health
                                    > this.ProviderE.GetDamage(x.Unit) + this.ProviderQ.GetDamage(x.Unit)))
                            {
                                if (GlobalVariables.Player.IsDashing())
                                {
                                    this.Execute(path.Connections.FirstOrDefault(x => x.Unit != null)?.Unit);
                                }
                            }

                            else if (units.Count > 0)
                            {
                                var unitsNotMoving =
                                    units.Where(
                                        x =>
                                        !x.IsMoving
                                        && x.Distance(GlobalVariables.Player)
                                        <= GlobalVariables.Spells[SpellSlot.Q].Range).ToList();

                                if (unitsNotMoving.Any())
                                {
                                    this.Execute(unitsNotMoving.MinOrDefault(x => x.Distance(GlobalVariables.Player)));
                                }
                                else
                                {
                                    var unit = units.MinOrDefault(x => x.Distance(GlobalVariables.Player));

                                    if (unit != null
                                        && unit.Distance(GlobalVariables.Player)
                                        <= GlobalVariables.Spells[SpellSlot.Q].Range)
                                    {
                                        Game.PrintChat("Stacking Case2");
                                        this.Execute(unit);
                                    }
                                }
                            }
                        }
                    }

                    break;

                    #endregion
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable()
        {
            Game.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            Game.OnUpdate += this.OnUpdate;
            base.OnEnable();
        }

        /// <summary>
        ///     Called when [initialize].
        /// </summary>
        protected override void OnInitialize()
        {
            this.ProviderQ = new SteelTempestLogicProvider();
            this.ProviderE = new SweepingBladeLogicProvider();

            base.OnInitialize();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Blacklist = new Blacklist(this.Menu, "Blacklist");

            this.MultiKnockUpMenu();
            this.StackMenu();

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        /// <summary>
        ///     Executes SteelTempest on the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="aoe">if set to <c>true</c> [aoe].</param>
        private void Execute(Obj_AI_Base target, bool aoe = false)
        {
            var predictionOutput = ProviderQ.GetPrediction(target, aoe);

            if (predictionOutput.Hitchance >= HitChance.High)
            {
                GlobalVariables.Spells[SpellSlot.Q].Cast(predictionOutput.UnitPosition);
            }
        }

        /// <summary>
        ///     Setups the Multi-KnockUp Menu
        /// </summary>
        private void MultiKnockUpMenu()
        {
            var selecter =
                new MenuItem("Mode", "Mode").SetValue(new StringList(new[] { "Custom", "Path Based", "Disabled" }, 1));

            var custom = new List<MenuItem>()
                             {
                                 new MenuItem("BuffState", "Only if: ").SetValue(
                                     new StringList(new[] { "Q3 (Stacked)", "Not Stacked", "Always" })),
                                 new MenuItem("MinHitAOECustom", "Min HitCount for AOE").SetValue(
                                     new Slider(2, 1, 5)),
                             };

            var pathBased = new List<MenuItem>()
                                {
                                    new MenuItem(
                                        "DisclaimerPathBased",
                                        "[i] [Experimental] Assembly will try to decide based on pathing"),
                                    new MenuItem("BuffStatePathBased", "Only if: ").SetValue(
                                        new StringList(
                                        new[] { "Q3 (Stacked)", "Not Stacked", "Always" })),
                                    new MenuItem("MinHitAOEPathBased", "Min HitCount for AOE").SetValue
                                        (new Slider(2, 1, 5)),
                                    new MenuItem("SegmentAmount", "Amount of calculations: ").SetValue(
                                        new Slider(50, 1, 500)),
                                    new MenuItem("PriorityMode", "Priority/Decisison MOde: ").SetValue(
                                        new StringList(
                                        new[]
                                            { "Champion Priority (TargetSelector)", "TODO: Killable" })),
                                };

            var disabled = new List<MenuItem>()
                               {
                                   new MenuItem(
                                       "DisclaimerPathBased",
                                       "[i] Never aim for multiple targets")
                               };

            var multiknockupDyn = new DynamicMenu(
                Menu,
                "Multi-Knockup Settings",
                selecter,
                new[] { custom, pathBased, disabled });
        }

        /// <summary>
        ///     Setups the Stack Menu.
        /// </summary>
        private void StackMenu()
        {
            var selecter =
                new MenuItem("Mode", "Mode").SetValue(
                    new StringList(new[] { "Custom", "Always (Brain-Dead)", "Path Based" }));

            var custom = new List<MenuItem>()
                             {
                                 new MenuItem("MinDistance", "Don't Stack if Distance to enemy <= ")
                                     .SetValue(new Slider(600, 0, 4000)),
                                 new MenuItem("MaxDistance", "Don't Stack if Distance to enemy >= ")
                                     .SetValue(new Slider(1500, 0, 4000)),
                                 new MenuItem(
                                     "MinCooldownQ",
                                     "Don't Stack if Q Cooldown is >= (milliseconds)").SetValue(
                                         new Slider(1700, 1333, 5000)),
                                 new MenuItem("CarePath", "Don't kill units in SweepingBlade Path")
                                     .SetValue(true),
                             };

            var pathBased = new List<MenuItem>()
                                {
                                    new MenuItem("Information", "[i] information").SetTooltip(
                                        "If this is enabled, the assembly will stack based on the current gapclose path. Currently here are no options, but if I got enough time and motivation I will add some.")
                                };

            var disabled = new List<MenuItem>() { new MenuItem("DisclaimerPathBased", "[i] Never Stack in Combo mode") };

            var stacksettings = new DynamicMenu(Menu, "Stack-Settings", selecter, new[] { custom, pathBased, disabled });
        }

        #endregion
    }
}