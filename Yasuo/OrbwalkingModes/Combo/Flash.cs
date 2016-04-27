namespace Yasuo.OrbwalkingModes.Combo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Yasuo.Common.Algorithm.Media;
    using Yasuo.Common.Classes;
    using Yasuo.Common.Extensions;
    using Yasuo.Common.Extensions.MenuExtensions;
    using Yasuo.Common.Provider;

    using Dash = Yasuo.Common.Objects.Dash;
    using Prediction = SebbyLib.Prediction.Prediction;
    using PredictionInput = SebbyLib.Prediction.PredictionInput;
    using PredictionOutput = SebbyLib.Prediction.PredictionOutput;

    internal class Flash : Child<Combo>
    {
        #region Fields

        /// <summary>
        ///     The Flash logicprovider
        /// </summary>
        public FlashLogicProvider Provider;

        /// <summary>
        ///     The E logicprovider
        /// </summary>
        public SweepingBladeLogicProvider ProviderE;

        /// <summary>
        ///     The Q logicprovider
        /// </summary>
        public SteelTempestLogicProvider ProviderQ;

        /// <summary>
        ///     The flash slot
        /// </summary>
        private SpellSlot flashSlot;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Flash" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public Flash(Combo parent)
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
        public override string Name => "Flash";

        #endregion

        #region Public Methods and Operators

        // TODO: Priority Medium
        /// <summary>
        ///     Called when [play animation].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectPlayAnimationEventArgs" /> instance containing the event data.</param>
        public void OnPlayAnimation(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            var flash = GlobalVariables.Player.Spellbook.Spells.FirstOrDefault(x => x.Name == "SummonerFlash");

            if (flash != null)
            {
                var eqFlash = new Spell(SpellSlot.Q, flash.SData.CastRange);

                eqFlash.SetSkillshot(0, 350, float.MaxValue, false, SkillshotType.SkillshotCircle);

                if (args.Animation == "Spell1_Dash" && sender.IsMe && GlobalVariables.Player.HasQ3()
                    && GlobalVariables.Player.Spellbook.CanUseSpell(this.flashSlot) == SpellState.Ready)
                {
                    var targets =
                        HeroManager.Enemies.Where(
                            x =>
                            x.IsValid && x.Distance(GlobalVariables.Player) <= eqFlash.Width / 2 + flash.SData.CastRange)
                            .ToList();

                    var preds = new List<PredictionOutput>();

                    foreach (var target in targets)
                    {
                        var predInput = new PredictionInput
                                            {
                                                From = GlobalVariables.Player.ServerPosition,
                                                Aoe = true,
                                                Collision = false,
                                                Speed = eqFlash.Speed,
                                                Delay = eqFlash.Delay,
                                                Radius = eqFlash.Range,
                                                Unit = target,
                                                Type = SebbyLib.Prediction.SkillshotType.SkillshotCircle,
                                            };

                        var pred = Prediction.GetPrediction(predInput);

                        if (pred != null && !preds.Contains(pred))
                        {
                            preds.Add(pred);
                        }
                    }

                    var execution = preds.MaxOrDefault(x => x.AoeTargetsHitCount);

                    if (execution != null)
                    {
                        Execute(execution.CastPosition);
                    }
                }
            }
        }

        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnUpdate(EventArgs args)
        {
            var flash = GlobalVariables.Player.Spellbook.Spells.FirstOrDefault(x => x.Name == "SummonerFlash");

            if (flash != null)
            {
                var eqFlash = new Spell(SpellSlot.Q, flash.SData.CastRange);

                eqFlash.SetSkillshot(0, 350, float.MaxValue, false, SkillshotType.SkillshotCircle);

                var heroes =
                    HeroManager.Enemies.Where(
                        x => x.Distance(GlobalVariables.Player) <= flash.SData.CastRange + eqFlash.Range).ToList();

                var units = ProviderE.GetUnits(GlobalVariables.Player.ServerPosition);
                {
                }

                var gridGenerator = new GridGenerator(units, new Vector3())
                                        {
                                            MaxConnections = units.Count,
                                            PathDeepness = 1
                                        };

                gridGenerator.Generate();

                var dashes = new List<Dash>();

                if (gridGenerator.Grid != null)
                {
                    foreach (var connection in gridGenerator.Grid.Connections.Where(x => x.IsDash))
                    {
                        dashes.Add(new Dash(connection.Unit));
                    }
                }

                #region Evaluation

                Dash dash = null;

                switch (flashSlot)
                {
                }

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
            Obj_AI_Base.OnPlayAnimation -= this.OnPlayAnimation;
            Game.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            Obj_AI_Base.OnPlayAnimation += this.OnPlayAnimation;
            Game.OnUpdate += this.OnUpdate;
            base.OnEnable();
        }

        /// <summary>
        ///     Called when [initialize].
        /// </summary>
        protected override void OnInitialize()
        {
            this.Provider = new FlashLogicProvider();
            this.ProviderQ = new SteelTempestLogicProvider();
            this.ProviderE = new SweepingBladeLogicProvider(450);

            foreach (var spell in GlobalVariables.Player.Spellbook.Spells)
            {
                if (spell.Name == "SummonerFlash")
                {
                    this.flashSlot = spell.Slot;
                }
            }

            base.OnInitialize();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            //#region Blacklist

            //var blacklist = new Menu("Blacklist", this.Name + "Blacklist");

            //if (HeroManager.Enemies.Count == 0)
            //{
            //    blacklist.AddItem(new MenuItem(blacklist.Name + "null", "No enemies found"));
            //}
            //else
            //{
            //    foreach (var x in HeroManager.Enemies)
            //    {
            //        blacklist.AddItem(new MenuItem(blacklist.Name + x.ChampionName, x.ChampionName).SetValue(false));
            //    }
            //    MenuExtensions.AddToolTip(
            //        blacklist,
            //        "Setting a champion to 'on', will make the script not using Flash for him anymore");
            //}

            //this.Menu.AddSubMenu(blacklist);

            //#endregion

            #region Stacking (Dynamic-Menu)

            var advancedDyn = new Menu("Advanced", this.Name + "Advanced");

            this.Menu.AddSubMenu(advancedDyn);

            advancedDyn.AddItem(
                new MenuItem(advancedDyn.Name + "EvaluationMode", "Evaluation Mode").SetValue(
                    new StringList(new[] { "Priority", "Count", "Damage", "Custom" }, 0))).ValueChanged +=
                delegate(object sender, OnValueChangeEventArgs eventArgs)
                    {
                        MenuExtensions.RefreshTagBased(
                            advancedDyn,
                            eventArgs.GetNewValue<StringList>().SelectedIndex + 1);
                    };

            // Priority
            advancedDyn.AddItem(
                new MenuItem(advancedDyn.Name + "MinPriority", "Only if Priority sum >=").SetValue(new Slider(8, 1, 25))
                    .SetTag(1));

            // Count
            advancedDyn.AddItem(
                new MenuItem(advancedDyn.Name + "Disclaimer", "[i] Can't recommend using this").SetTag(2));

            // Damage
            advancedDyn.AddItem(
                new MenuItem(advancedDyn.Name + "Information", "[i] information").SetTooltip(
                    "If this is enabled, the assembly will stack based on the current gapclose path. Currently here are no options, but if I got enough time and motivation I will add some.")
                    .SetTag(3));

            // Custom
            advancedDyn.AddItem(new MenuItem(advancedDyn.Name + "WIP", "[i] Work in progress!"));

            // Always available
            advancedDyn.AddItem(new MenuItem(advancedDyn.Name + "", " "));

            advancedDyn.AddItem(
                new MenuItem(advancedDyn.Name + "MinPlayerHealth", "Min Player Health").SetValue(new Slider(0)));

            advancedDyn.AddItem(new MenuItem(advancedDyn.Name + "MinHealthDifference", "Min Health Difference"));

            MenuExtensions.RefreshTagBased(
                advancedDyn,
                advancedDyn.Item(advancedDyn.Name + "EvaluationMode").GetValue<StringList>().SelectedIndex + 1);

            #endregion

            this.Menu.AddItem(
                new MenuItem(this.Name + "MinHitAOE", "Min HitCount EQ Flash").SetValue(new Slider(3, 1, 5)));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        /// <summary>
        ///     Executes to the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        private void Execute(Vector3 position)
        {
            if (!position.IsWall() && position.IsValid())
            {
                GlobalVariables.Player.Spellbook.CastSpell(this.flashSlot, position);
            }
        }

        #endregion
    }
}