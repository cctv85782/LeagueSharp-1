// TODO: Add new Dash Object to make things easier

namespace Yasuo.OrbwalkingModes.LaneClear
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Extensions.MenuExtensions;
    using Yasuo.Common.LogicProvider;

    using Dash = Yasuo.Common.Objects.Dash;

    internal class SweepingBlade : Child<LaneClear>
    {
        #region Fields

        /// <summary>
        ///     The blacklist
        /// </summary>
        public BlacklistMenu BlacklistMenu;

        /// <summary>
        ///     The provider e
        /// </summary>
        private SweepingBladeLogicProvider providerE;

        /// <summary>
        ///     The provider turret
        /// </summary>
        private TurretLogicProvider providerTurret;

        /// <summary>
        /// The units
        /// </summary>
        protected List<Obj_AI_Base> Units;

        /// <summary>
        /// The dashes
        /// </summary>
        protected List<Dash> Dashes; 

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SweepingBlade" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public SweepingBlade(LaneClear parent)
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
        public override string Name => "(E) Sweeping Blade";

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
            this.providerE = new SweepingBladeLogicProvider();
            this.providerTurret = new TurretLogicProvider();

            this.Units = new List<Obj_AI_Base>();
            this.Dashes = new List<Dash>();

            base.OnInitialize();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);


            this.BlacklistMenu = new BlacklistMenu(this.Menu, "Don't dash into");

            this.SetupGeneralMenu();

            this.SetupLastHitMenu();

            this.AddMiscMenu();


            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        /// <summary>
        ///     Adds the general settings.
        /// </summary>
        private void SetupGeneralMenu()
        {
            this.Menu.AddItem(
                new MenuItem(this.Name + "ModeTarget", "Dash to: ").SetValue(new StringList(new[] { "Mouse", "Auto" })));
        }

        /// <summary>
        ///     Adds the lasthit menu.
        /// </summary>
        private void SetupLastHitMenu()
        {
            this.Menu.AddItem(
                new MenuItem(this.Name + "LastHit", "Smart Lasthit").SetValue(true)
                    .SetTooltip(
                        "The assembly will only Lasthit a minion if Q is not up and the end position of the dash is not too close to the enemy and is not inside a skillshot"));
        }

        /// <summary>
        ///     Adds the misc menu.
        /// </summary>
        private void AddMiscMenu()
        {
            this.Menu.AddItem(new MenuItem(this.Name + "NoSkillshot", "Don't E into Skillshots").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "NoTurret", "Don't E into Turret").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "NoEnemy", "Don't E into Enemies").SetValue(true));
        }

        /// <summary>
        ///     Executes on the specified unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        private static void Execute(Obj_AI_Base unit)
        {
            if (unit.IsValidTarget() && unit != null)
            {
                GlobalVariables.Spells[SpellSlot.E].CastOnUnit(unit);
            }
        }

        // TODO: Decomposite
        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnUpdate(EventArgs args)
        {
            this.SoftReset();

            if (GlobalVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear
                || !GlobalVariables.Spells[SpellSlot.E].IsReady() || GlobalVariables.Player.IsWindingUp)
            {
                return;
            }

            this.GetUnits();

            this.SetDashes();

            this.ValidateDashes();

            this.LogicLaneClear();        
        }

        /// <summary>
        /// Executes LaneClear logic
        /// </summary>
        private void LogicLaneClear()
        {
            switch (this.Menu.Item(this.Name + "ModeTarget").GetValue<StringList>().SelectedIndex)
            {
                // Mouse oriented LaneClearing
                case 0:
                    if (Dashes.Any())
                    {
                        foreach (
                            var dash in
                                Dashes.Where(
                                    dash =>
                                    dash.EndPosition.Distance(Game.CursorPos)
                                    < dash.StartPosition.Distance(Game.CursorPos)))
                        {
                            if (dash.IsWallDash)
                            {
                                continue;
                            }

                            // Minion will die and no other minions are in killable range
                            if (dash.Unit.Health < this.providerE.GetDamage(dash.Unit)
                                && !this.Units.Any(
                                    x =>
                                    !x.Equals(dash.Unit)
                                    && (x.Distance(GlobalVariables.Player) <= GlobalVariables.Player.AttackRange)
                                    || (GlobalVariables.Spells[SpellSlot.Q].IsReady(100)
                                        && x.Distance(GlobalVariables.Player)
                                        <= GlobalVariables.Spells[SpellSlot.Q].Range)))
                            {
                                if (GlobalVariables.Debug)
                                {
                                    Console.WriteLine(@"LaneClear: SweepingBlade > Killing killable unit");
                                }

                                Execute(dash.Unit);
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Gets the units.
        /// </summary>
        private void GetUnits()
        {
            this.Units = MinionManager.GetMinions(
                GlobalVariables.Player.ServerPosition,
                GlobalVariables.Spells[SpellSlot.E].Range,
                MinionTypes.All,
                MinionTeam.NotAlly,
                MinionOrderTypes.None);
        }

        /// <summary>
        /// Sets the dashes.
        /// </summary>
        private void SetDashes()
        {
            if (!this.Units.Any())
            {
                return;
            }

            this.Dashes =
                this.Units.Where(
                    x =>
                    !x.HasBuff("YasuoDashScalar")
                    && x.Distance(GlobalVariables.Player) <= GlobalVariables.Spells[SpellSlot.E].Range)
                    .Select(minion => new Dash(minion))
                    .ToList();
        }

        /// <summary>
        /// Resets some properties/fields
        /// </summary>
        private void SoftReset()
        {
            this.Units = new List<Obj_AI_Base>();
            this.Dashes = new List<Dash>();
        }

        /// <summary>
        /// Validates the dashes.
        /// </summary>
        private void ValidateDashes()
        {
            foreach (var dash in Dashes.ToList())
            {
                if (Menu.Item(this.Name + "NoTurret").GetValue<bool>())
                {
                    if (this.providerTurret.IsSafePosition(dash.EndPosition))
                    {
                        continue;
                    }
                }

                if (Menu.Item(this.Name + "NoSkillshot").GetValue<bool>())
                {
                    if (!dash.InSkillshot)
                    {
                        continue;
                    }
                }

                if (Menu.Item(this.Name + "NoEnemy").GetValue<bool>())
                {
                    var range = 500;

                    if (dash.EndPosition.CountEnemiesInRange(range) == 0)
                    {
                        continue;
                    }
                }

                Dashes.Remove(dash);
            }
        }
        #endregion
    }
}