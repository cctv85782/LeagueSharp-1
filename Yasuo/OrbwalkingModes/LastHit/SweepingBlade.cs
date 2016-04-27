namespace Yasuo.OrbwalkingModes.LastHit
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

    using Dash = Common.Objects.Dash;

    internal class SweepingBlade : Child<LastHit>
    {
        public SweepingBlade(LastHit parent)
            : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "(E) Sweeping Blade";

        public SweepingBladeLogicProvider ProviderE;

        public TurretLogicProvider ProviderTurret;

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

            #region Champion Specific Range Settings

            var settingsChampDepen = new Menu("Champion Dependent", this.Name + "ChampionDependent");

            if (HeroManager.Enemies.Count == 0)
            {
                settingsChampDepen.AddItem(new MenuItem(settingsChampDepen.Name + "null", "No enemies found"));
            }
            else
            {
                foreach (var x in HeroManager.Enemies)
                {
                    settingsChampDepen.AddItem(
                        new MenuItem(settingsChampDepen.Name + x.ChampionName, x.ChampionName).SetValue(
                            new Slider((int) x.AttackRange + (int)(GlobalVariables.Player.BoundingRadius / 2) + 100)));
                }

                settingsChampDepen.AddItem(
                    new MenuItem(settingsChampDepen.Name + "information", "[i] informations").SetTooltip(
                        "Changing values will change how close you dash to a champion while lasthitting"));
            }

            this.Menu.AddSubMenu(settingsChampDepen);

            #endregion

            // Mode
            this.Menu.AddItem(
                new MenuItem(this.Name + "ModeTarget", "Dash to: ").SetValue(new StringList(new[] { "Mouse", "Auto" })));

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
                    .SetTooltip("The assembly will only Lasthit a minion if Q is not up and the end position of the dash is not too close to the enemy and is not inside a skillshot"));

            #endregion

            this.Menu.AddItem(new MenuItem(this.Name + "NoSkillshot", "Don't E into Skillshots").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "NoTurret", "Don't E into Turret").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "NoEnemy", "Don't E into Enemies").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.ProviderE = new SweepingBladeLogicProvider();
            this.ProviderTurret = new TurretLogicProvider();

            base.OnInitialize();
        }

        public void OnUpdate(EventArgs args)
        {
            if (GlobalVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit
                || !GlobalVariables.Spells[SpellSlot.E].IsReady())
            {
                return;
            }

            var minions = MinionManager.GetMinions(
                            GlobalVariables.Player.ServerPosition,
                            GlobalVariables.Spells[SpellSlot.E].Range,
                            MinionTypes.All,
                            MinionTeam.NotAlly,
                            MinionOrderTypes.None);

            var dashes = new List<Dash>();

            if (minions.Any())
            {
                dashes = minions.Where(x => !x.HasBuff("YasuoDashScalar") && x.Distance(GlobalVariables.Player) <= GlobalVariables.Spells[SpellSlot.E].Range).Select(minion => new Common.Objects.Dash(minion)).ToList();
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
                case 0:
                    if (dashes.Any())
                    {
                        foreach (var dash in dashes.Where(dash => dash.EndPosition.Distance(Game.CursorPos) < dash.StartPosition.Distance(Game.CursorPos)))
                        {
                            if (!dash.IsWallDash)
                            {
                                // Minion will die and no other minions are in killable range
                                if (dash.Unit.Health < this.ProviderE.GetDamage(dash.Unit)
                                    && !minions.Any(
                                        x =>
                                        !x.Equals(dash.Unit)
                                        && (x.Distance(GlobalVariables.Player) <= GlobalVariables.Player.AttackRange)
                                        || (GlobalVariables.Spells[SpellSlot.Q].IsReady(100)
                                            && x.Distance(GlobalVariables.Player) <= GlobalVariables.Spells[SpellSlot.Q].Range)))
                                {
                                    if (GlobalVariables.Debug)
                                    {
                                        Console.WriteLine(@"LaneClear: SweepingBlade > Killing killable unit");
                                    }

                                    this.Execute(dash.Unit);
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    if (dashes.Any())
                    {
                        foreach (var dash in dashes)
                        {
                            if (!dash.IsWallDash)
                            {
                                // Minion will die and no other minions are in killable range
                                if (dash.Unit.Health < this.ProviderE.GetDamage(dash.Unit)
                                    && !minions.Any(
                                        x =>
                                        !x.Equals(dash.Unit)
                                        && (x.Distance(GlobalVariables.Player) <= GlobalVariables.Player.AttackRange)
                                        || (GlobalVariables.Spells[SpellSlot.Q].IsReady(100)
                                            && x.Distance(GlobalVariables.Player) <= GlobalVariables.Spells[SpellSlot.Q].Range)))
                                {
                                    if (GlobalVariables.Debug)
                                    {
                                        Console.WriteLine(@"LaneClear: SweepingBlade > Killing killable unit");
                                    }

                                    this.Execute(dash.Unit);
                                }
                            }
                        }
                    }
                    break;
            }

            // if EQ will hit more than X units and X units die
            if (this.Menu.Item(this.Name + "EQ").GetValue<bool>())
            {
                if (GlobalVariables.Spells[SpellSlot.Q].IsReady(50))
                {
                    var possibleDashes = minions.Select(unit => new Common.Objects.Dash(GlobalVariables.Player.ServerPosition, unit)).ToList();

                    var mostHits = possibleDashes.MaxOrDefault(x => x.KnockUpMinions.Count);

                    if (mostHits.KnockUpMinions.Count >= this.Menu.Item(this.Name + "MinHitAOE").GetValue<Slider>().Value
                        && GlobalVariables.Spells[SpellSlot.Q].Cooldown <= 1
                        && !minions.Any(x => x.Distance(GlobalVariables.Player) <= GlobalVariables.Player.AttackRange && x.HealthPercent <= 25))
                    {
                        Execute(mostHits.Unit);
                    }
                }
            }

            // Smart Last Hit
            if (this.Menu.Item(this.Name + "LastHit").GetValue<bool>())
            {
                if (minions == null)
                {
                    return;
                }

                var enemies = HeroManager.Enemies.Where(x => x.Health > 0).ToList();
                List<Obj_AI_Base> possibleExecutions = new List<Obj_AI_Base>();

                foreach (var x in minions)
                {
                    foreach (var y in enemies.Where(z => z.HealthPercent > 10))
                    {
                        var newPos = GlobalVariables.Player.ServerPosition.Extend(x.ServerPosition, GlobalVariables.Spells[SpellSlot.E].Range);
                        if (newPos.Distance(y.ServerPosition) < y.AttackRange)
                        {
                            possibleExecutions.Add(x);
                        }
                    }
                        
                }
            }
        }

        private void Execute(Obj_AI_Base target)
        {
            var dash = new Common.Objects.Dash(GlobalVariables.Player.ServerPosition, target);

            if (target.IsValidTarget() && ProviderTurret.IsSafePosition(dash.EndPosition))
            {
                GlobalVariables.Spells[SpellSlot.E].CastOnUnit(target);
            }
        }
    }
}

