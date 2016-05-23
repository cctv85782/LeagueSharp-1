namespace Yasuo.Yasuo.OrbwalkingModes.Mixed
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Yasuo.CommonEx;
    using global::Yasuo.CommonEx.Classes;
    using global::Yasuo.CommonEx.Extensions;
    using global::Yasuo.CommonEx.Menu;
    using global::Yasuo.Yasuo.LogicProvider;
    using global::Yasuo.Yasuo.Menu.MenuSets.OrbwalkingModes.Combo;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    #endregion

    internal class SteelTempest : Child<Mixed>
    {
        #region Fields

        /// <summary>
        ///     The E logic provider
        /// </summary>
        private SweepingBladeLogicProvider providerE;

        /// <summary>
        ///     The Q logic provider
        /// </summary>
        private SteelTempestLogicProvider providerQ;

        /// <summary>
        ///     The Turret logic provider
        /// </summary>
        private TurretLogicProvider providerTurret;

        /// <summary>
        /// The minions
        /// </summary>
        protected List<Obj_AI_Base> Units;

        /// <summary>
        /// The good circumstances
        /// </summary>
        private bool goodCircumstances;

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

            if (GlobalVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit
                || !GlobalVariables.Spells[SpellSlot.Q].IsReady()
                || GlobalVariables.Player.IsDashing()
                || GlobalVariables.Player.IsWindingUp)
            {
                return;
            }

            this.GetMinions();

            this.ValidateMinions();

            this.CheckCircumstances();

            if (!this.goodCircumstances)
            {
                return;
            }

            this.LogicLastHit();
        }

        private void ValidateMinions()
        {
            if (!this.Units.Any())
            {
                return;
            }

            foreach (var unit in this.Units.ToList())
            {
                var remove = false;

                if (unit.Distance(GlobalVariables.Player) > GlobalVariables.Spells[SpellSlot.Q].Range)
                {
                    remove = true;
                }

                if (this.providerQ.GetDamage(unit) < SebbyLib.HealthPrediction.GetHealthPrediction(unit, (int)this.providerQ.TravelTime(unit)))
                {
                    remove = true;
                }

                if (remove)
                {
                    this.Units.Remove(unit);
                }
            }
        }

        /// <summary>
        ///     Checks the circumstances.
        /// </summary>
        private void CheckCircumstances()
        {
            if (this.providerQ.HasQ3()
                && GlobalVariables.Player.CountEnemiesInRange(
                    this.Menu.Item(this.Name + "NoQ3Range").GetValue<Slider>().Value)
                >= this.Menu.Item(this.Name + "NoQ3Count").GetValue<Slider>().Value)
            {
                this.goodCircumstances = false;
                return;
            }

            var settingsAdv = this.Menu.SubMenu(this.Name + "Advanced");

            if (!this.Units.Any())
            {
                this.goodCircumstances = false;
                return;
            }

            if (settingsAdv.Item(settingsAdv.Name + "TurretCheck").GetValue<bool>())
            {
                if (!this.providerTurret.IsSafePosition(GlobalVariables.Player.ServerPosition))
                {
                    this.goodCircumstances = false;
                    return;
                }
            }
        }

        /// <summary>
        ///     Gets the minions and targets.
        /// </summary>
        private void GetMinions()
        {
            this.Units = SebbyLib.Cache.GetMinions(GlobalVariables.Player.ServerPosition, GlobalVariables.Spells[SpellSlot.Q].Range, MinionTeam.NotAlly);
        }

        /// <summary>
        /// Resets some fields
        /// </summary>
        private void SoftReset()
        {
            this.goodCircumstances = true;

            this.Units = new List<Obj_AI_Base>();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable()
        {
            Events.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            Events.OnUpdate += this.OnUpdate;
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

            this.Units = new List<Obj_AI_Base>();

            base.OnInitialize();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);


            var menuGenerator = new MenuGenerator(new SteelTempestMenu(this.Menu));

            menuGenerator.Generate();


            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

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
        private static void Execute(Vector2 position)
        {
            if (position.IsValid())
            {
                GlobalVariables.Spells[SpellSlot.Q].Cast(position);
            }
        }

        /// <summary>
        ///     LastHits with ranged/stacked Q
        /// </summary>
        private void LogicLastHit()
        {
            var validunits =
                this.Units.Where(
                    x =>
                    x.IsValidTarget(GlobalVariables.Spells[SpellSlot.Q].Range - x.BoundingRadius / 2)).ToList();

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

        #endregion
    }
}