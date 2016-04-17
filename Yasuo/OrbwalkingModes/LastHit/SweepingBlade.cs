namespace Yasuo.OrbwalkingModes.LastHit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Objects;
    using Yasuo.Common.Provider;
    using Yasuo.Common.Utility;

    internal class SweepingBlade : Child<LastHit>
    {
        public SweepingBlade(LastHit parent)
            : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "Sweeping Blade";

        public SweepingBladeLogicProvider Provider;

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

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.Provider = new SweepingBladeLogicProvider();
            this.ProviderTurret = new TurretLogicProvider();

            base.OnInitialize();
        }

        public void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LastHit
                || !Variables.Spells[SpellSlot.E].IsReady())
            {
                return;
            }

            Obj_AI_Base minion = null;
            List<Obj_AI_Base> minions = MinionManager.GetMinions(
                Variables.Player.ServerPosition,
                Variables.Spells[SpellSlot.E].Range,
                MinionTypes.All,
                MinionTeam.Enemy,
                MinionOrderTypes.None);

            switch (this.Menu.Item(this.Name + "ModeTarget").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    minion = MinionManager.GetMinions(Game.CursorPos, 475).Where(x => !x.HasBuff("YasuoDashWrapper") && x.Distance(Variables.Player) <= Variables.Spells[SpellSlot.E].Range)
                        .OrderByDescending(x => x.Health).FirstOrDefault();
                    break;
                case 1:
                    minion = MinionManager.GetMinions(Variables.Player.ServerPosition, 475).Where(x => !x.HasBuff("YasuoDashWrapper"))
                        .OrderByDescending(x => x.Health).FirstOrDefault();
                    break;
            }

            if (minion == null)
            {
                return;
            }

            // if EQ will hit more than X units and X units die
            if (this.Menu.Item(this.Name + "EQ").GetValue<bool>())
            {
                if (Variables.Spells[SpellSlot.Q].IsReady(50))
                {
                    var possibleDashes = minions.Select(unit => new Common.Objects.Dash(Variables.Player.ServerPosition, unit)).ToList();

                    var mostHits = possibleDashes.MaxOrDefault(x => x.KnockUpMinions.Count);

                    if (mostHits.KnockUpMinions.Count >= this.Menu.Item(this.Name + "MinHitAOE").GetValue<Slider>().Value
                        && Variables.Spells[SpellSlot.Q].Cooldown <= 1
                        && !minions.Any(x => x.Distance(Variables.Player) <= Variables.Player.AttackRange && x.HealthPercent <= 25))
                    {
                        Execute(mostHits.Over);
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
                        var newPos = Variables.Player.ServerPosition.Extend(x.ServerPosition, Variables.Spells[SpellSlot.E].Range);
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
            var dash = new Common.Objects.Dash(Variables.Player.ServerPosition, target);

            if (target.IsValidTarget() && ProviderTurret.IsSafePosition(dash.EndPosition))
            {
                Variables.Spells[SpellSlot.E].CastOnUnit(target);
            }
        }
    }
}

