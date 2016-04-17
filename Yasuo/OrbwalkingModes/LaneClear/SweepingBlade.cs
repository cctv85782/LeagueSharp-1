// TODO: Add new Dash Object to make things easier

namespace Yasuo.OrbwalkingModes.LaneClear
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Extensions;
    using Yasuo.Common.Objects;
    using Yasuo.Common.Provider;
    using Yasuo.Common.Utility;

    using Dash = Yasuo.Common.Objects.Dash;

    internal class SweepingBlade : Child<LaneClear>
    {
        #region Fields

        public List<Obj_AI_Base> BlacklistUnits;

        public SweepingBladeLogicProvider ProviderE;

        public TurretLogicProvider ProviderTurret;

        #endregion

        #region Constructors and Destructors

        public SweepingBlade(LaneClear parent)
            : base(parent)
        {
            this.OnLoad();
        }

        #endregion

        #region Public Properties

        public override string Name => "Sweeping Blade";

        #endregion

        #region Public Methods and Operators

        

        #endregion

        #region Methods

        protected override void OnDisable()
        {
            Game.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            Game.OnUpdate += this.OnUpdate;
            base.OnEnable();
        }

        protected override void OnInitialize()
        {
            this.ProviderE = new SweepingBladeLogicProvider();
            this.ProviderTurret = new TurretLogicProvider();

            base.OnInitialize();
        }

        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            // Mode
            this.Menu.AddItem(
                new MenuItem(this.Name + "ModeTarget", "Dash to: ").SetValue(new StringList(new[] { "Mouse", "Auto" })));

            // EQ

            #region EQ

            this.Menu.AddItem(
                new MenuItem(this.Name + "EQ", "Try to E for EQ").SetValue(true)
                    .SetTooltip("The assembly will try to E on a minion in order to Q"));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MinHitAOE", "Min HitCount for AOE").SetValue(new Slider(1, 1, 15)));

            #endregion

            #region E LastHit

            this.Menu.AddItem(
                new MenuItem(this.Name + "LastHit", "Smart Lasthit").SetValue(true)
                    .SetTooltip(
                        "The assembly will only Lasthit a minion if Q is not up and the end position of the dash is not too close to the enemy and is not inside a skillshot"));

            #endregion

            #region Misc

            this.Menu.AddItem(new MenuItem(this.Name + "NoSkillshot", "Don't E into Skillshots").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "NoTurret", "Don't E into Turret").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "NoEnemy", "Don't E into Enemies").SetValue(true));

            #endregion

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != LeagueSharp.Common.Orbwalking.OrbwalkingMode.LaneClear || !Variables.Spells[SpellSlot.E].IsReady()
                || Variables.Player.IsWindingUp)
            {
                return;
            }

            var minions = MinionManager.GetMinions(
                Variables.Player.ServerPosition,
                Variables.Spells[SpellSlot.E].Range,
                MinionTypes.All,
                MinionTeam.NotAlly,
                MinionOrderTypes.None);

            var dashes = new List<Dash>();

            if (minions.Any())
            {
                dashes = minions.Where(x => !x.HasBuff("YasuoDashScalar")).Select(minion => new Common.Objects.Dash(minion)).ToList();
            }

            foreach (var dash in dashes.ToList())
            {
                if (Menu.Item(this.Name + "NoSkillshot").GetValue<bool>())
                {
                    if (!dash.InSkillshot)
                    {
                        continue;
                    }
                }

                if (Menu.Item(this.Name + "NoTurret").GetValue<bool>())
                {
                    if (ProviderTurret.IsSafePosition(dash.EndPosition))
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

                dashes.Remove(dash);
            }
            
            switch (this.Menu.Item(this.Name + "ModeTarget").GetValue<StringList>().SelectedIndex)
            {
                // Mouse oriented LaneClearing
                case 0:
                    if (dashes.Any())
                    {
                        foreach (var dash in dashes.Where(dash => dash.EndPosition.Distance(Game.CursorPos) < dash.StartPosition.Distance(Game.CursorPos)))
                        {
                            if (!dash.IsWallDash)
                            {
                                // Minion will die and no other minions are in killable range
                                if (dash.Over.Health < this.ProviderE.GetDamage(dash.Over)
                                    && !minions.Any(
                                        x =>
                                        !x.Equals(dash.Over)
                                        && (x.Distance(Variables.Player) <= Variables.Player.AttackRange)
                                        || (Variables.Spells[SpellSlot.Q].IsReady(100)
                                            && x.Distance(Variables.Player) <= Variables.Spells[SpellSlot.Q].Range)))
                                {
                                    if (Variables.Debug)
                                    {
                                        Console.WriteLine(@"LaneClear: SweepingBlade > Killing killable unit");
                                    }

                                    this.Execute(dash.Over);
                                }
                            }
                        }
                    }
                    break;
            }

            // if EQ will hit more than X units
            if (this.Menu.Item(this.Name + "EQ").GetValue<bool>())
            {
                if (dashes.Any())
                {
                    var dash = dashes.MaxOrDefault(x => x.KnockUpMinions.Count);

                    if (dash.KnockUpMinions.Count >= this.Menu.Item(this.Name + "MinHitAOE").GetValue<Slider>().Value
                        && Variables.Spells[SpellSlot.Q].Cooldown <= 1
                        && !minions.Any(x => x.Distance(Variables.Player) <= Variables.Player.AttackRange && x.HealthPercent <= 25))
                    {
                        Execute(dash.Over);
                    }
                }
            }
        }

        private void Execute(Obj_AI_Base unit)
        {
            var dash = new Dash(unit);

            if (unit.IsValidTarget() && unit != null)
            {
                Variables.Spells[SpellSlot.E].CastOnUnit(unit);
            }
        }

        #endregion
    }
}