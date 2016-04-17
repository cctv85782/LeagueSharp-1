namespace Yasuo.OrbwalkingModes.Mixed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Objects;
    using Yasuo.Common.Provider;
    using Yasuo.Common.Utility;

    internal class SweepingBlade : Child<Mixed>
    {
        public SweepingBlade(Mixed parent)
            : base(parent)
        {
            this.OnLoad();
        }

        public List<Obj_AI_Base> BlacklistUnits;

        public Path Path;

        public override string Name => "Sweeping Blade";

        public SweepingBladeLogicProvider ProviderE;

        public TurretLogicProvider ProviderTurret;

        protected override void OnEnable()
        {
            Game.OnUpdate += this.OnUpdate;
            Drawing.OnDraw += this.OnDraw;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            Game.OnUpdate -= this.OnUpdate;
            Drawing.OnDraw -= this.OnDraw;
            base.OnDisable();
        }

        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            // TODO: Add completely automatic harass (dash in/out)
            //// Mode
            //this.Menu.AddItem(
            //    new MenuItem(this.Name + "ModeTarget", "Dash to: ").SetValue(new StringList(new[] { "Mouse", "Auto" })));

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
                    .SetTooltip("The assembly will only Lasthit a minion if Q is not up and the end position of the dash is not too close to the enemy and is not inside a skillshot"));

            #endregion

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.ProviderE = new SweepingBladeLogicProvider();

            this.ProviderTurret = new TurretLogicProvider();

            base.OnInitialize();
        }

        // TODO: PRIORITY HIGH
        public void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Mixed)
            {
                return;
            }

            Obj_AI_Base minion = MinionManager.GetMinions(Game.CursorPos, 475).Where(x =>
                                !x.HasBuff("YasuoDashWrapper")
                                && x.Distance(Variables.Player) <= Variables.Spells[SpellSlot.E].Range)
                                .OrderByDescending(x => x.Health)
                                .FirstOrDefault();

            var minions = MinionManager.GetMinions(
                Variables.Player.ServerPosition,
                Variables.Spells[SpellSlot.E].Range,
                MinionTypes.All,
                MinionTeam.Enemy,
                MinionOrderTypes.None);

            if (this.Menu.Item(this.Name + "LastHit").GetValue<bool>())
            {
                if (minions == null)
                {
                    return;
                }

                var enemies = HeroManager.Enemies.Where(x => x.Health > 0).ToList();
                var possibleExecutions = new List<Obj_AI_Base>();

                foreach (
                    var x in
                        minions.Where(
                            unit =>
                            unit.Health <= this.ProviderE.GetDamage(unit)
                            && unit.Distance(Variables.Player.ServerPosition) <= Variables.Spells[SpellSlot.E].Range))
                {
                    if (enemies.Count(enemy => enemy.Distance(Variables.Player.ServerPosition) <= 1000) > 0)
                    {
                        foreach (var y in enemies.Where(z => z.HealthPercent > 10))
                        {
                            var newPos = Variables.Player.ServerPosition.Extend(
                                x.ServerPosition,
                                Variables.Spells[SpellSlot.E].Range);
                            if (newPos.Distance(y.ServerPosition) < y.AttackRange)
                            {
                                possibleExecutions.Add(x);
                            }
                        }
                    }
                    else
                    {
                        possibleExecutions.Add(minion);
                    }
                }

                if (possibleExecutions.Count == 0)
                {
                    return;
                }

                this.Execute(possibleExecutions.MinOrDefault(x => x.Distance(Helper.GetMeanVector2(minions))));
            }
        }

        public void OnDraw(EventArgs args)
        {
            
        }

        private void Execute(Obj_AI_Base target)
        {
            var dash = new Common.Objects.Dash(target);
            if (target.IsValidTarget() && target != null && ProviderTurret.IsSafePosition(dash.EndPosition))
            {
                Variables.Spells[SpellSlot.E].CastOnUnit(target);
            }
        }
    }
}

