namespace Yasuo.OrbwalkingModes.Mixed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Extensions;
    using Yasuo.Common.Extensions.MenuExtensions;
    using Yasuo.Common.LogicProvider;

    using Dash = Yasuo.Common.Objects.Dash;

    internal class SweepingBlade : Child<Mixed>
    {
        #region Fields

        /// <summary>
        ///     The champion slider menu
        /// </summary>
        public ChampionSliderMenu ChampionSliderMenu;

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
        ///     The possible dashes
        /// </summary>
        protected List<Dash> PossibleDashes = new List<Dash>();

        /// <summary>
        ///     The possible minions
        /// </summary>
        protected List<Obj_AI_Base> NotValidatedMinions = new List<Obj_AI_Base>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SweepingBlade" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public SweepingBlade(Mixed parent)
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

        #region Public Methods and Operators

        /// <summary>
        ///     Executes the eq logic.
        /// </summary>
        public void LogicEq()
        {
            var steelTempestMenu = this.Menu.Parent.SubMenu("(Q) Steel Tempest");

            if (!this.Menu.Item(this.Name + "EQ").GetValue<bool>()
                || !steelTempestMenu.Item("(Q) Steel Tempest" + "Enabled").GetValue<bool>()
                || !steelTempestMenu.Item("(Q) Steel Tempest" + "EQ").GetValue<bool>()
                || !GlobalVariables.Spells[SpellSlot.Q].IsReady((int)this.providerE.Speed()))
            {
                return;
            }

            if (this.providerQ.HasQ3()
                && steelTempestMenu.Item("(Q) Steel Tempest" + "EQ.OnlyNotStacked").GetValue<bool>())
            {
                return;
            }

            var dash = this.PossibleDashes.MaxOrDefault(x => x.MinionsHitCircular.Count(unit => unit.Health <= this.providerQ.GetDamage(unit)));

            if (dash.MinionsHitCircular.Count >= steelTempestMenu.Item("(Q) Steel Tempest" + "EQ.MinHitAOE").GetValue<Slider>().Value
                && !this.NotValidatedMinions.Any(
                    x =>
                    x.Distance(GlobalVariables.Player) <= GlobalVariables.Player.AttackRange && x.HealthPercent <= 25))
            {
                Execute(dash.Unit);
            }
        }

        // TODO: PRIORITY LOW > Some more settings but that should work for now
        /// <summary>
        ///     Executes the LastHit logic.
        /// </summary>
        public void LogicLastHit()
        {
            var dashes = this.PossibleDashes.Where(
                x =>
                x.Unit.Health <= this.providerE.GetDamage(x.Unit)
                && !x.IsWallDash
                && this.providerTurret.IsSafePosition(x.EndPosition)
                ).ToList();

            if (!dashes.Any())
            {
                return;
            }

            foreach (var dash in dashes.ToList())
            {
                if (this.ChampionSliderMenu.Values.Any(
                        entry => dash.EndPosition.Distance(entry.Key.ServerPosition) > entry.Value))
                {
                    dashes.Remove(dash);
                }
            }

            switch (this.Menu.Item(this.Name + "ModeTarget").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    foreach (var dash in
                        dashes.Where(
                            x => x.EndPosition.Distance(Game.CursorPos) >= x.StartPosition.Distance(Game.CursorPos)))
                    {
                        dashes.Remove(dash);
                    }
                    break;
            }

            var range = GlobalVariables.Spells[SpellSlot.Q].IsReady((int)this.providerE.Speed() - 100) ? GlobalVariables.Spells[SpellSlot.Q].Range : GlobalVariables.Player.AttackRange;

            foreach (var dash in dashes.ToList().Where(x => x.StartPosition.CountMinionsInRange(range) > 1))
            {
                dashes.Remove(dash);
            }

            var validDash = dashes.MaxOrDefault(x => x.EndPosition.CountMinionsInRange(GlobalVariables.Spells[SpellSlot.Q].Range));

            if (GlobalVariables.Debug)
            {
                Console.WriteLine(@"LaneClear: SweepingBlade > Killing killable unit: {0}", validDash.Unit);
            }

            Execute(validDash.Unit);
        }

        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnUpdate(EventArgs args)
        {
            this.SoftReset();

            if (GlobalVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit
                || !GlobalVariables.Spells[SpellSlot.E].IsReady())
            {
                return;
            }

            this.GetMinions();

            this.BuildDashes();

            this.ValidateDashes();

            this.LogicEq();

            this.LogicLastHit();
        }

        private void SoftReset()
        {
            this.PossibleDashes = new List<Dash>();
            this.NotValidatedMinions = new List<Obj_AI_Base>();
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
            this.providerE = new SweepingBladeLogicProvider();
            this.providerQ = new SteelTempestLogicProvider();
            this.providerTurret = new TurretLogicProvider();

            base.OnInitialize();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);


            this.ChampionSliderMenu = new ChampionSliderMenu(this.Menu, "Min Distance to Enemy");

            this.SetupGeneralMenu();

            this.SetupEqMenu();

            this.SetupMiscMenu();


            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        /// <summary>
        ///     Adds the eq menu.
        /// </summary>
        private void SetupEqMenu()
        {
            this.Menu.AddItem(
                new MenuItem(this.Name + "EQ", "Try to E for EQ").SetValue(true)
                    .SetTooltip("The assembly will try to E on a minion in order to EQ"));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MinHitAOE", "Min HitCount for AOE").SetValue(new Slider(1, 1, 15)));
        }

        /// <summary>
        ///     Adds the general menu.
        /// </summary>
        private void SetupGeneralMenu()
        {
            this.Menu.AddItem(
                new MenuItem(this.Name + "ModeTarget", "Dash to: ").SetValue(new StringList(new[] { "Mouse", "Auto" })));
        }

        /// <summary>
        ///     Adds the misc menu.
        /// </summary>
        private void SetupMiscMenu()
        {
            this.Menu.AddItem(new MenuItem(this.Name + "NoSkillshot", "Don't E into Skillshots").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "NoTurret", "Don't E into Turret").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "NoEnemy", "Don't E into Enemies").SetValue(true));
        }

        /// <summary>
        ///     Gets the minions.
        /// </summary>
        private void GetMinions()
        {
            this.NotValidatedMinions = MinionManager.GetMinions(
                    GlobalVariables.Player.ServerPosition,
                    GlobalVariables.Spells[SpellSlot.E].Range,
                    MinionTypes.All,
                    MinionTeam.NotAlly,
                    MinionOrderTypes.None);
        }

        /// <summary>
        ///     Builds the dashes.
        /// </summary>
        private void BuildDashes()
        {
            if (this.NotValidatedMinions.Any())
            {
                this.PossibleDashes =
                    this.NotValidatedMinions.Where(
                        x =>
                        !x.HasBuff("YasuoDashScalar")
                        && x.IsValidTarget()
                        && x.Distance(GlobalVariables.Player) <= GlobalVariables.Spells[SpellSlot.E].Range)
                        .Select(minion => new Dash(minion))
                        .ToList();
            }
        }

        /// <summary>
        ///     Executes on the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        private static void Execute(Obj_AI_Base target)
        {
            if (target.IsValidTarget())
            {
                GlobalVariables.Spells[SpellSlot.E].CastOnUnit(target);
            }
        }

        /// <summary>
        ///     Validates the dashes recording to the menu settings.
        /// </summary>
        private void ValidateDashes()
        {
            if (this.PossibleDashes == null || !this.PossibleDashes.Any())
            {
                return;
            }

            foreach (var dash in this.PossibleDashes.ToList())
            {
                if (this.Menu.Item(this.Name + "NoSkillshot").GetValue<bool>())
                {
                    if (!dash.InSkillshot)
                    {
                        continue;
                    }
                }

                if (this.Menu.Item(this.Name + "NoTurret").GetValue<bool>())
                {
                    if (this.providerTurret.IsSafePosition(dash.EndPosition))
                    {
                        continue;
                    }
                }

                if (this.Menu.Item(this.Name + "NoEnemy").GetValue<bool>())
                {
                    const int Range = 500;

                    if (dash.EndPosition.CountEnemiesInRange(Range) == 0)
                    {
                        continue;
                    }
                }

                this.PossibleDashes.Remove(dash);
            }
        }

        #endregion
    }
}