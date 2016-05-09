﻿namespace Yasuo.OrbwalkingModes.LastHit
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
    using Yasuo.Common.Utility;

    using Dash = Yasuo.Common.Objects.Dash;

    #endregion

    internal class SteelTempest : Child<LastHit>
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
        protected List<Obj_AI_Base> Minions;

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
        public SteelTempest(LastHit parent)
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
                || !GlobalVariables.Spells[SpellSlot.Q].IsReady() || GlobalVariables.Player.Spellbook.IsCharging
                || GlobalVariables.Player.Spellbook.IsChanneling)
            {
                return;
            }

            this.GetMinions();

            this.CheckCircumstances();

            if (!goodCircumstances)
            {
                return;
            }

            this.LogicStacking();

            this.LogicLastHit();
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
                goodCircumstances = false;
                return;
            }

            var settingsAdv = this.Menu.SubMenu(this.Name + "Advanced");

            if (!this.Minions.Any())
            {
                goodCircumstances = false;
                return;
            }

            if (settingsAdv.Item(settingsAdv.Name + "TurretCheck").GetValue<bool>())
            {
                if (!this.providerTurret.IsSafePosition(GlobalVariables.Player.ServerPosition))
                {
                    goodCircumstances = false;
                    return;
                }
            }
        }

        /// <summary>
        ///     Gets the minions and targets.
        /// </summary>
        private void GetMinions()
        {
            this.Minions = MinionManager.GetMinions(
                GlobalVariables.Player.ServerPosition,
                GlobalVariables.Spells[SpellSlot.Q].Range,
                MinionTypes.All,
                MinionTeam.NotAlly,
                MinionOrderTypes.None);
        }

        /// <summary>
        /// Resets some fields
        /// </summary>
        private void SoftReset()
        {
            goodCircumstances = true;

            Minions = new List<Obj_AI_Base>();
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

            base.OnInitialize();
        }

        // TODO: Decomposite
        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);


            this.SetupEqMenu();

            this.SetupAdvancedMenu();

            this.SetupGeneralMenu();


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
        ///     LastHits with ranged/stacked Q
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
        ///     Setups the advanced menu.
        /// </summary>
        private void SetupAdvancedMenu()
        {
            var settingsAdv = new Menu("Advanced", this.Name + "Advanced");

            settingsAdv.AddItem(
                new MenuItem(settingsAdv.Name + "TurretCheck", "Check for enemy turret").SetValue(true)
                    .SetTooltip(
                        "if this is enabled, the assembly will try to not use stacked/charged Q inside the enemy turret. But it will use them if the turret is focusing something else!"));

            this.Menu.AddSubMenu(settingsAdv);
        }

        /// <summary>
        ///     Setups the eq menu.
        /// </summary>
        private void SetupEqMenu()
        {
            this.Menu.AddItem(new MenuItem(this.Name + "EQ", "Use while dashing (EQ)").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "EQ.MinHitAOE", "Min Hit Count").SetValue(new Slider(1, 1, 15)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "EQ.OnlyNotStacked", "(EQ) Only Cast if Q not charged").SetValue(true)
                    .SetTooltip("If this is enabled, the assembly won't do EQ if you have stacked/charged Q"));
        }

        /// <summary>
        ///     Setups the general menu.
        /// </summary>
        private void SetupGeneralMenu()
        {
            this.Menu.AddItem(
                new MenuItem(this.Name + "NoQ3Count", "Don't use Q3 if >= Enemies around").SetValue(
                    new Slider(2, 0, 5)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "NoQ3Range", "Check for enemies in range of").SetValue(
                    new Slider(1000, 0, 5000)));
        }

        #endregion
    }
}