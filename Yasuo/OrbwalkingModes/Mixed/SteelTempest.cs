namespace Yasuo.OrbwalkingModes.Mixed
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Extensions;
    using Yasuo.Common.LogicProvider;

    #endregion

    internal class SteelTempest : Child<Mixed>
    {
        #region Fields

        /// <summary>
        ///     The units
        /// </summary>
        protected List<Obj_AI_Base> Minions;

        /// <summary>
        ///     The targets
        /// </summary>
        protected List<Obj_AI_Hero> Targets;

        /// <summary>
        ///     The provider e
        /// </summary>
        private SweepingBladeLogicProvider providerE;

        /// <summary>
        ///     The provider q
        /// </summary>
        private SteelTempestLogicProvider providerQ;

        /// <summary>
        ///     The provider turret
        /// </summary>
        private TurretLogicProvider providerTurret;

        /// <summary>
        ///     Whether circumstances are good or bad
        /// </summary>
        protected bool GoodCircumstances;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SteelTempest" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public SteelTempest(Mixed parent)
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
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnUpdate(EventArgs args)
        {
            this.SoftReset();

            if (GlobalVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Mixed
                || !GlobalVariables.Spells[SpellSlot.Q].IsReady() || GlobalVariables.Player.Spellbook.IsCharging
                || GlobalVariables.Player.Spellbook.IsChanneling)
            {
                return;
            }

            this.GetMinionsAndTargets();

            this.CheckCircumstances();

            if (!GoodCircumstances)
            {
                return;
            }

            this.LogicEq();

            this.LogicStacking();

            this.LogicLastHit();
            
        }

        // TODO: This won't work.
        /// <summary>
        ///     Checks the circumstances.
        /// </summary>
        private void CheckCircumstances()
        {
            if (this.providerQ.HasQ3()
            && GlobalVariables.Player.CountEnemiesInRange(this.Menu.Item(this.Name + "NoQ3Range").GetValue<Slider>().Value)
            >= this.Menu.Item(this.Name + "NoQ3Count").GetValue<Slider>().Value)
            {
                GoodCircumstances = false;
                return;
            }

            var settingsAdv = this.Menu.SubMenu(this.Name + "Advanced");

            if (!this.Minions.Any() && !this.Targets.Any())
            {
                GoodCircumstances = false;
                return;
            }

            if (settingsAdv.Item(settingsAdv.Name + "TurretCheck").GetValue<bool>())
            {
                if (!this.providerTurret.IsSafePosition(GlobalVariables.Player.ServerPosition))
                {
                    GoodCircumstances = false;
                    return;
                }
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
            this.providerQ = new SteelTempestLogicProvider();
            this.providerE = new SweepingBladeLogicProvider();
            this.providerTurret = new TurretLogicProvider();

            this.Minions = new List<Obj_AI_Base>();
            Targets = new List<Obj_AI_Hero>();

            base.OnInitialize();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.SetupGeneralMenu();

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        /// <summary>
        ///     Executes on the specified unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        private static void Execute(Obj_AI_Base unit)
        {
            if (unit.IsValid)
            {
                GlobalVariables.Spells[SpellSlot.Q].Cast(unit);
            }
        }

        /// <summary>
        ///     Executes on the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        private static void Execute(Vector3 position)
        {
            if (position.IsValid())
            {
                GlobalVariables.Spells[SpellSlot.Q].Cast(position);
            }
        }

        /// <summary>
        ///     Executes on the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        private static void Execute(Vector2 position)
        {
            if (position.IsValid())
            {
                GlobalVariables.Spells[SpellSlot.Q].Cast(position);
            }
        }

        /// <summary>
        ///     Gets the minions and targets.
        /// </summary>
        private void GetMinionsAndTargets()
        {
            this.Minions = MinionManager.GetMinions(
                GlobalVariables.Player.ServerPosition,
                GlobalVariables.Spells[SpellSlot.Q].Range,
                MinionTypes.All,
                MinionTeam.NotAlly,
                MinionOrderTypes.None);

            Targets =
                HeroManager.Enemies.Where(
                    x => x.Distance(GlobalVariables.Player.ServerPosition) <= GlobalVariables.Spells[SpellSlot.Q].Range)
                    .ToList();
        }

        /// <summary>
        /// Resets some fields
        /// </summary>
        private void SoftReset()
        {
            if (Minions.Any())
            {
                Minions = new List<Obj_AI_Base>();
            }

            if (Targets.Any())
            {
                Targets = new List<Obj_AI_Hero>();
            }

            if (!GoodCircumstances)
            {
                GoodCircumstances = true;
            }
        }

        /// <summary>
        ///     Executes Q in Eq combo
        /// </summary>
        private void LogicEq()
        {
            if (!this.Menu.Item(this.Name + "EQ").GetValue<bool>() || !GlobalVariables.Player.IsDashing())
            {
                return;
            }

            if (this.Menu.Item(this.Name + "EQ.OnlyNotStacked").GetValue<bool>() && this.providerQ.HasQ3())
            {
                return;
            }

            var minions =
                Minions.Where(
                    x =>
                    x.Health <= providerQ.GetDamage(x)
                    && x.Distance(providerE.GetCurrentDashEndPosition()) <= GlobalVariables.Spells[SpellSlot.Q].Range)
                    .ToList();

            if (minions.Count >= this.Menu.Item(this.Name + "MinHitAOE").GetValue<Slider>().Value)
            {
                Execute(minions.MaxOrDefault(x => x.Gold).ServerPosition);
            }
        }

        /// <summary>
        ///     LastHits with normal Q
        /// </summary>
        private void LogicLastHit()
        {
            var validunits =
                Minions.Where(
                    x =>
                    x.IsValidTarget(GlobalVariables.Spells[SpellSlot.Q].Range - x.BoundingRadius / 2)
                    && x.Health <= this.providerQ.GetDamage(x));

            var unit = validunits.MaxOrDefault(x => x.FlatGoldRewardMod);

            if (unit != null)
            {
                Execute(unit);
            }
        }

        /// <summary>
        ///     Executes the stacking logic
        /// </summary>
        private void LogicStacking()
        {
            var validunits =
                Minions.Where(
                    x =>
                    x.IsValidTarget(GlobalVariables.Spells[SpellSlot.Q].Range - x.BoundingRadius / 2)
                    && x.Health <= this.providerQ.GetDamage(x)).ToList();

            var farmlocation = MinionManager.GetBestLineFarmLocation(
                validunits.ToVector3S().To2D(),
                GlobalVariables.Spells[SpellSlot.Q].Width,
                GlobalVariables.Spells[SpellSlot.Q].Range);

            var unit = validunits.MaxOrDefault(x => x.FlatGoldRewardMod);

            if (!farmlocation.Position.IsValid())
            {
                if (unit != null)
                {
                    Execute(unit);
                }
            }
            else
            {
                Execute(farmlocation.Position);
            }
        }

        /// <summary>
        ///     Setups the general menu.
        /// </summary>
        private void SetupGeneralMenu()
        {
            this.Menu.AddItem(
                new MenuItem(this.Name + "AOE", "Try to hit multiple").SetValue(true)
                    .SetTooltip(
                        "If predicted hit count > slider, it will try to hit multiple, else it will aim for a single champion"));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MinHitAOE", "Min HitCount for AOE").SetValue(new Slider(1, 1, 5)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "LastHit", "LastHit with Q").SetValue(true)
                    .SetTooltip("If this is enabled, the assembly will lasthit minions with Steel Tempest"));

            this.Menu.AddItem(
                new MenuItem(this.Name + "LastHitNoQ3", "Do not LastHit with charged Q").SetValue(true)
                    .SetTooltip(
                        "if this is enabled, the assembly won't lasthit minions with Steel Tempest when it has 3 Stacks (tornado)"));
        }

        #endregion
    }
}