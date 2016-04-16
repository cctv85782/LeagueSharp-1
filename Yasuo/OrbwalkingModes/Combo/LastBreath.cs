namespace Yasuo.OrbwalkingModes.Combo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Extensions;
    using Yasuo.Common.Provider;

    internal class LastBreath : Child<Combo>
    {
        public LastBreath(Combo parent)
            : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "Last Breath";

        public LastBreathLogicProvider Provider;

        protected override void OnEnable()
        {
            Game.OnUpdate += this.OnUpdate2;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            Game.OnUpdate -= this.OnUpdate2;
            base.OnDisable();
        }

        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "[Disabled] Enabled").SetValue(true));

            #region Blacklist

            var blacklist = new Menu("Blacklist", this.Name + "Blacklist");

            if (HeroManager.Enemies.Count == 0)
            {
                blacklist.AddItem(new MenuItem(blacklist.Name + "null", "No enemies found"));
            }
            else
            {
                foreach (var x in HeroManager.Enemies)
                {
                    blacklist.AddItem(new MenuItem(blacklist.Name + x.ChampionName, x.ChampionName).SetValue(false));
                }
                MenuExtensions.AddToolTip(
                    blacklist,
                    "Setting a champion to 'on', will make the script not using R for him anymore");
            }

            this.Menu.AddSubMenu(blacklist);

            #endregion

            #region Spell specific settings

            this.Menu.AddItem(
                new MenuItem(this.Name + "AOE", "Try to hit multiple").SetValue(true)
                    .SetTooltip(
                        "If this is enabled, the assembly will try to hit multiple targets"));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MinHitAOE", "Min HitCount for AOE").SetValue(new Slider(2, 1, 5)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MinPlayerHealth", "Min Player Health (%)").SetValue(new Slider(10, 0, 100)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MaxTargetsMeanHealth", "Max Target(s) Health (%)").SetValue(new Slider(80, 0, 100)));


            #region Advanced

            var advanced = new Menu("Advanced", this.Name + "Advanced");

            advanced.AddItem(
                new MenuItem(advanced.Name + "EvaluationLogic", "Evaluation Logic").SetValue(
                    new StringList(new[] { "Damage", "Count", "Priority", "Auto" })));

            advanced.AddItem(
                new MenuItem(advanced.Name + "MaxHealthPercDifference", "Max Health (%) Difference").SetValue(new Slider(40, 0, 100)));

            advanced.AddItem(new MenuItem(advanced.Name + "OverkillCheck", "Overkill Check").SetValue(true)
                .SetTooltip("If Combo is enough to finish the target it won't execute. Only works on single targets."));

            advanced.AddItem(
                new MenuItem(advanced.Name + "Disclaimer", "[i] Disclaimer")
                    .SetTooltip("Changing Values here might destroy the assembly logic, only change values if you know what you are doing!"));

            #endregion

            #endregion


            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.Provider = new LastBreathLogicProvider();

            base.OnInitialize();
        }

        public void OnUpdate(EventArgs args)
        {
            try
            {
                if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !Variables.Spells[SpellSlot.R].IsReady())
                {
                    return;
                }

                var enemies = new List<Obj_AI_Hero>();
                enemies.AddRange(HeroManager.Enemies.Where(enemy => enemy.IsAirbone()));

                var possibleExecutions = new List<Common.Objects.LastBreath>();
                var validatedExecutions = new List<Common.Objects.LastBreath>();
                if (enemies.Count > 0)
                {
                    possibleExecutions.AddRange(enemies.Select(enemy => new Yasuo.Common.Objects.LastBreath(enemy)));
                }

                var execution = new Yasuo.Common.Objects.LastBreath(null);

                // Menu: Min Hit AOE && AOE
                if (this.Menu.Item(this.Name + "AOE").GetValue<bool>())
                {
                    validatedExecutions.AddRange(possibleExecutions.Where(entry => entry.EnemiesInUlt >= this.Menu.Item(this.Name + "MinHitAOE").GetValue<Slider>().Value));
                }
                else
                {
                    validatedExecutions = possibleExecutions;
                }

                // TODO: Add a lot more stuff here
                #region TargetSelector

                if (validatedExecutions.Count > 0)
                {
                    execution = validatedExecutions.MaxOrDefault(x => x.DamageDealt);
                }

                #endregion

                if (execution == null || !this.Provider.ShouldCastNow(execution, new SweepingBladeLogicProvider().GetPath(execution.Target.ServerPosition)))
                {
                    return;
                }

                // Menu: Min Player Health
                if (Variables.Player.HealthPercent <= this.Menu.Item(this.Name + "MinPlayerHealth").GetValue<Slider>().Value)
                {
                    return;
                }

                // OBSERVATION: Needs some more love. Often using ult, even if you could gapclose for just QE or EQ or just Q AA.
                // Menu: Overkill Check
                if (this.Menu.Item(this.Name + "OverkillCheck").GetValue<bool>())
                {
                    var healthAll = 0f;
                    var damageAll = 0f;

                    if (execution.AffectedEnemies.Count > 0)
                    {
                        healthAll += execution.AffectedEnemies.Sum(enemy => enemy.Health);
                    }
                    else
                    {
                        healthAll = execution.Target.Health;
                    }

                    foreach (var spell in Variables.Spells.Where(x => x.Value.IsReady() && x.Value.Slot != SpellSlot.R && x.Value.Slot != SpellSlot.W))
                    {
                        damageAll = execution.DamageDealt;
                    }

                    if (Variables.Debug)
                    {
                        Game.PrintChat(@"healthAll:" + healthAll);
                        Game.PrintChat(@"damageAll:" + damageAll);
                    }

                    if (healthAll > damageAll)
                    {
                        this.Execute(execution.Target);
                    }
                }
                else
                {
                    this.Execute(execution.Target);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        public void OnUpdate2(EventArgs args)
        {
            var executions = new List<Common.Objects.LastBreath>();

            foreach (var hero in HeroManager.Enemies.Where(x => x.IsAirbone()))
            {
                var execution = new Common.Objects.LastBreath(hero);

                if (!executions.Contains(execution))
                {
                    executions.Add(execution);
                }
            }

            var advanced = Menu.SubMenu(this.Name + "advanced");

            Common.Objects.LastBreath possibleExecution = null;

            if (executions.Count == 0)
            {
                return;
            }

            // TODO: ADD safetyvalue/dangervalue
            switch (advanced.Item("EvaluationMode").GetValue<StringList>().SelectedIndex)
            {
                // Damage
                case 0:
                    possibleExecution = executions.MaxOrDefault(x => x.DamageDealt);
                    break;
                // Count
                case 1:
                    possibleExecution = executions.MaxOrDefault(x => x.AffectedEnemies.Count);
                    break;
                // Priority
                case 2:
                    possibleExecution = executions.MaxOrDefault(x => x.Priority);
                    break;
                // Auto
                case 3:
                    break;
            }

            if (possibleExecution != null)
            {
                Execute(possibleExecution);
            }
        }

        private void Execute(Obj_AI_Hero target)
        {
            if (target.IsValid && !target.IsZombie && target.IsAirbone())
            {
                Variables.Spells[SpellSlot.R].CastOnUnit(target);
            }
        }

        private void Execute(Common.Objects.LastBreath execution)
        {
            if (execution.AffectedEnemies.Count < Menu.Item("MinHitAOE").GetValue<Slider>().Value
                || Variables.Player.HealthPercent < Menu.Item("MinPlayerHealth").GetValue<Slider>().Value
                || (execution.AffectedEnemies.Sum(x => x.HealthPercent) / execution.AffectedEnemies.Count) > Menu.Item("MaxTargetsMeanHealth").GetValue<Slider>().Value
                || !Provider.ShouldCastNow(execution, SweepingBlade.PathCopy))
            {
                return;
            }

            Variables.Spells[SpellSlot.R].CastOnUnit(execution.Target);
        }
    }
}

