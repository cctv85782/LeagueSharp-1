namespace Rethought_Yasuo.Yasuo.Modules.Combo
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.ActionManager.Abstract_Classes;
    using RethoughtLib.Menu;
    using RethoughtLib.Menu.Presets;

    using Rethought_Yasuo.Yasuo.Modules.Core.SpellParent;
    using Rethought_Yasuo.Yasuo.Modules.Core.SpellParent.Implementations;

    using SharpDX;

    using PredictionOutput = SebbyLib.Prediction.PredictionOutput;

    #endregion

    internal class NonDashingQ2Module : OrbwalkingChild
    {
        #region Fields

        /// <summary>
        ///     The spells module
        /// </summary>
        private readonly ISpellIndex spellParent;

        /// <summary>
        ///     The q logic provider module
        /// </summary>
        private readonly YasuoQ yasuoQ;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NonDashingQModule" /> class.
        /// </summary>
        public NonDashingQ2Module(ISpellIndex spellParent, IActionManager actionManager)
            : base(actionManager)
        {
            this.yasuoQ = (YasuoQ)spellParent[SpellSlot.Q];

            this.spellParent = spellParent;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Q (charged & not-dashing)";

        public IEnumerable<Obj_AI_Hero> Targets
        {
            get
            {
                return
                    HeroManager.Enemies.Where(x => x.Distance(ObjectManager.Player) <= this.yasuoQ.Spell.Range).ToList();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the target.
        /// </summary>
        /// <value>
        ///     The target.
        /// </value>
        private Obj_AI_Hero Target
        {
            get
            {
                return TargetSelector.GetTarget(
                    this.spellParent[SpellSlot.Q].Spell.Range,
                    TargetSelector.DamageType.Physical);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Casts the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        public void Cast(Vector3 position)
        {
            if (!position.IsValid())
            {
                return;
            }

            this.spellParent[SpellSlot.Q].Spell.Cast(position);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [OnEnable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            var selecter = new MenuItem("Mode", "Mode").SetValue(new StringList(new[] { "Custom", "Disabled" }, 0));

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
                                        "[Experimental] Assembly will try to decide based on pathing"),
                                    new MenuItem("BuffStatePathBased", "Only if: ").SetValue(
                                        new StringList(
                                        new[] { "Q3 (Stacked)", "Not Stacked", "Always" })),
                                    new MenuItem("MinHitAOEPathBased", "Min HitCount for AOE").SetValue
                                        (new Slider(2, 1, 5)),
                                    new MenuItem("SegmentAmount", "Amount of calculations: ").SetValue(
                                        new Slider(50, 1, 500)),
                                    new MenuItem("PriorityMode", "Priority/Decision Mode: ").SetValue(
                                        new StringList(
                                        new[]
                                            {
                                                "ChampionYasuo Priority (TargetSelector)",
                                                "TODO: Killable"
                                            })),
                                };

            var menuGenerator = new MenuGenerator(
                this.Menu,
                new DynamicMenu("Multi-Knockup Settings", selecter, new[] { custom }));

            menuGenerator.Generate();
        }

        private void GameOnOnUpdate(EventArgs args)
        {
            if (this.CheckGuardians())
            {
                return;
            }

            this.Execute();
        }

        /// <summary>
        ///     Method to Multi-KnockUp.
        /// </summary>
        private void Execute()
        {
            var settings = this.Menu.SubMenu("Multi-Knockup Settings");

            var preds = new List<PredictionOutput>();

            if (this.Targets.Any())
            {
                foreach (var target in
                    this.Targets.Where(x => x.IsValid))
                {
                    var pred = this.yasuoQ.GetPrediction(target, true);

                    preds.Add(pred);
                }
            }

            if (!preds.Any()) return;
            {
                switch (settings.Item("Mode").GetValue<StringList>().SelectedIndex)
                {
                    // Custom
                    case 0:

                        var mostKnockedUp =
                            preds.Where(
                                x =>
                                x.AoeTargetsHitCount
                                >= settings.Item("MinHitAOECustom").GetValue<Slider>().Value
                                && x.Hitchance >= SebbyLib.Prediction.HitChance.VeryHigh)
                                .MaxOrDefault(x => x.AoeTargetsHit.Count);

                        if (mostKnockedUp != null && !mostKnockedUp.CastPosition.IsZero)
                        {
                            this.ActionManager.Queque.Enqueue(2, () => this.yasuoQ.Spell.Cast(mostKnockedUp.CastPosition));
                        }

                        break;

                    //// PathBase Based
                    //// TODO: ADD calculation for arriving to that point on the PathBase
                    //case 1:
                    //    var PathBase = SweepingBlade.PathBaseCopy;

                    //    if (PathBase == null)
                    //    {
                    //        return;
                    //    }

                    //    // TODO
                    //    var vectors =
                    //        PathBase.SplitIntoVectors(multiknockupsettings.Item("SegmentAmount").GetValue<Slider>().Value);

                    //    var predDic = new Dictionary<Vector3, List<PredictionOutput>>();

                    //    if (vectors.Count > 0)
                    //    {
                    //        foreach (var vector in vectors)
                    //        {
                    //            var predsVec =
                    //                this.Targets.ToList()
                    //                    .Select(unit => this.providerQ.GetPrediction(vector, unit, true, 0))
                    //                    .ToList();

                    //            predDic.Add(vector, predsVec);
                    //        }
                    //    }

                    //    var scoreDic = new Dictionary<PredictionOutput, float>();

                    //    foreach (var entry in predDic.ToList())
                    //    {
                    //        var value = entry.Value;

                    //        foreach (var pred in value.ToList())
                    //        {
                    //            if (pred.Hitchance < HitChance.High)
                    //            {
                    //                continue;
                    //            }

                    //            switch (multiknockupsettings.Item("PriorityMode").GetValue<StringList>().SelectedIndex)
                    //            {
                    //                // ChampionYasuo Priority (Target Selector)
                    //                case 0:
                    //                    scoreDic.Add(pred, pred.AoeTargetsHit.Sum(x => TargetSelector.GetPriority(x)));
                    //                    break;
                    //                // Killability
                    //                case 1:
                    //                    scoreDic.Add(pred, pred.AoeTargetsHit.Sum(x => TargetSelector.GetPriority(x)));
                    //                    break;
                    //                // Something smart that takes many things into consideration
                    //                //case 2:
                    //                //    break;
                    //            }
                    //        }
                    //    }

                    //    PredictionOutput finalPred = null;

                    //    if (scoreDic.Any())
                    //    {
                    //        finalPred = scoreDic.MaxOrDefault(x => x.Value).Key;
                    //    }

                    //    if (finalPred != null)
                    //    {
                    //        GlobalVariables.Spells[SpellSlot.Q].Cast(finalPred.CastPosition);
                    //    }
                    //    break;
                }
            }
        }

        #endregion
    }
}