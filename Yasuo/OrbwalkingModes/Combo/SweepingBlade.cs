namespace Yasuo.OrbwalkingModes.Combo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Yasuo.Common.Algorithm.Djikstra;
    using Yasuo.Common.Algorithm.Media;
    using Yasuo.Common.Classes;
    using Yasuo.Common.Extensions;
    using Yasuo.Common.Objects;
    using Yasuo.Common.Provider;
    using Yasuo.Common.Utility;

    internal class SweepingBlade : Child<Combo>
    {
        public SweepingBlade(Combo parent)
            : base(parent)
        {
            this.OnLoad();
        }

        public List<Obj_AI_Base> BlacklistChampions = new List<Obj_AI_Base>();

        internal Path Path;

        internal static Path PathCopy = new Path();

        internal Grid Grid;

        public override string Name => "Sweeping Blade";

        public SweepingBladeLogicProvider ProviderE;

        public TurretLogicProvider ProviderTurret;

        protected override void OnEnable()
        {
            Game.OnUpdate += this.OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += this.OnProcessSpellCast;
            Drawing.OnDraw += this.OnDraw;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            Game.OnUpdate -= this.OnUpdate;
            Obj_AI_Base.OnProcessSpellCast -= this.OnProcessSpellCast;
            Drawing.OnDraw -= this.OnDraw;
            base.OnDisable();
        }

        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            // Blacklist
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
                    "Setting a champion to 'on', will make the script not using Q for him anymore");
            }

            this.Menu.AddSubMenu(blacklist);


            #region Pathfinding

            var pathfindingMenu = new Menu("Pathfinding", this.Name + "PathfindingMenu");

            pathfindingMenu.AddItem(new MenuItem("Enabled", "Enabled").SetValue(true));

            pathfindingMenu.AddItem(
                new MenuItem("ModeTarget", "Dash to: ").SetValue(new StringList(new[] { "Mouse", "Enemy" })));

            pathfindingMenu.AddItem(
                new MenuItem("Prediction", "Predict enemy position").SetValue(true)
                    .SetTooltip("The assembly will try to E to the enemy predicted position. This will not work if Mode is set to Mouse."));

            pathfindingMenu.AddItem(
                new MenuItem("AutoWalkToDash", "[Experimental] Auto-Walk to dash").SetValue(true)
                    .SetTooltip("If this is enabled the assembly will auto-walk behind a unit to dash over it."));

            pathfindingMenu.AddItem(
                new MenuItem("AutoDashing", "[Experimental] Auto-Dash dashable path (Dashing-Path)").SetValue(true)
                .SetTooltip("If this is enabled the assembly will automatic pathfind and walk to the end of the path. This is a basic feature of pathfinding."));

            pathfindingMenu.AddItem(
                new MenuItem("AutoWalking", "[Experimental] Auto-Walk non-dashable path (Walking-Path)").SetValue(false)
                    .SetTooltip("If this is enabled the assembly will automatic pathfind and walk to the end of the path. If you like to have maximum control or your champion disable this."));

            pathfindingMenu.AddItem(
                new MenuItem("PathAroundSkillShots", "[Experimental] Try to Path around Skillshots").SetValue(
                    true).SetTooltip("if this is enabled, the assembly will path around a skillshot if a path is given."));

            this.Menu.AddSubMenu(pathfindingMenu);
            #endregion


            #region E on Champion
            
            var onchampion = new Menu("Dash On Champion", this.Name + "EOnChampionMenu");

            onchampion.AddItem(new MenuItem(this.Name + "MaxHealthDashOut", "Dash defensively if Health % <=").SetValue(new Slider(30, 0, 100)));

            onchampion.AddItem(new MenuItem(this.Name + "OnlyKillableCombo", "Only dash on champion if killable by Combo").SetValue(true));

            onchampion.AddItem(new MenuItem(this.Name + "Whirlwind", "Smart EQ").SetValue(true));

            #endregion


            #region EQ

            this.Menu.AddItem(
                new MenuItem(this.Name + "EQ", "Try to E for EQ").SetValue(true)
                    .SetTooltip("The assembly will try to E on a minion in order to Q"));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MinHitAOE", "Min HitCount for AOE").SetValue(new Slider(1, 1, 5)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MinOwnHealth", "Min Player Health%").SetValue(new Slider(15, 1, 100))
                    .SetTooltip("The assembly will try to E on a minion in order to Q"));

            #endregion


            #region Prediction



            #endregion


            #region Drawings

            var drawingMenu = new Menu("Drawings", this.Name + "Drawings");

            drawingMenu.AddItem(
                new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true)
                    .SetTooltip("The assembly will Draw the expected path to the enemy"));

            drawingMenu.AddItem(
                new MenuItem(this.Name + "SmartDrawings", "Smart Drawings").SetValue(true)
                    .SetTooltip("Automaticall disables Drawings under certain circumstances and will do auto-coloring."));

            drawingMenu.AddItem(new MenuItem(this.Name + "PathDashColor", "Dashes").SetValue(new Circle(true, System.Drawing.Color.DodgerBlue)));

            drawingMenu.AddItem(new MenuItem(this.Name + "PathDashWidth", "Width of lines").SetValue(new Slider(2, 1, 10)));

            drawingMenu.AddItem(new MenuItem(this.Name + "Seperator1", ""));

            drawingMenu.AddItem(new MenuItem(this.Name + "PathWalkColor", "Walking").SetValue(new Circle(true, System.Drawing.Color.White)));

            drawingMenu.AddItem(new MenuItem(this.Name + "PathWalkWidth", "Width of lines").SetValue(new Slider(2, 1, 10)));

            drawingMenu.AddItem(new MenuItem(this.Name + "CirclesColor", "Draw Circles").SetValue(new Circle(true, System.Drawing.Color.DodgerBlue)));

            drawingMenu.AddItem(new MenuItem(this.Name + "CirclesLineWidth", "Width of lines").SetValue(new Slider(2, 1, 10)));

            drawingMenu.AddItem(new MenuItem(this.Name + "CirclesRadius", "Radius").SetValue(new Slider(40, 10, 475)));

            this.Menu.AddSubMenu(drawingMenu);

            #endregion

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.ProviderE = new SweepingBladeLogicProvider();
            this.ProviderTurret = new TurretLogicProvider();

            base.OnInitialize();
        }

        // ReSharper disable once FunctionComplexityOverflow
        public void OnUpdate(EventArgs args)
        {
            try
            {
                Path = null;
                PathCopy = null;

                if (Variables.Player.IsDead
                    || Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                    || !Variables.Spells[SpellSlot.E].IsReady())
                {
                    return;
                }

                #region Cast on Champion

                var targetE = TargetSelector.SelectedTarget ?? TargetSelector.GetTarget(Variables.Spells[SpellSlot.E].Range, TargetSelector.DamageType.Magical);

                if (targetE != null && !this.BlacklistChampions.Contains(targetE)
                    && targetE.Distance(Variables.Player.ServerPosition) <= Variables.Spells[SpellSlot.E].Range)
                {
                    var dash = new Yasuo.Common.Objects.Dash(Variables.Player.ServerPosition, targetE);

                    if (targetE.Health < ProviderE.GetDamage(targetE))
                    {
                        var meanvector = Helper.GetMeanVector3(HeroManager.Enemies.Where(x => x.Distance(dash.EndPosition) <= 1000).Select(x => x.ServerPosition).ToList());

                        if (Variables.Player.Health > Menu.SubMenu(this.Name + "EOnChampionMenu").Item(this.Name + "MaxHealthDashOut").GetValue<Slider>().Value)
                        {
                            if (dash.EndPosition.Distance(meanvector) <= Variables.Player.Distance(meanvector))
                            {
                                Execute(targetE);
                            }
                        }
                        else
                        {
                            if (dash.EndPosition.Distance(meanvector) >= Variables.Player.Distance(meanvector))
                            {
                                Execute(targetE);
                            }
                        }

                    }
                    if (Variables.Player.HasQ3())
                    {
                        // 1 v 1
                        if (dash.EndPosition.CountEnemiesInRange(1000) == 1)
                        {
                            if (dash.KnockUpHeroes.Contains(targetE))
                            {
                                Execute(targetE);
                            }
                        }
                        else
                        {
                            var heroes = HeroManager.Enemies.Where(x => x.Distance(dash.EndPosition) <= 1000);

                            if (dash.KnockUpHeroes.Count >= (heroes.Count() / 2))
                            {
                                Execute(targetE);
                            }
                        }
                    }
                }

                #endregion


                #region Pathfinding

                #region Dash to XY-Vector

                if (Menu.SubMenu(this.Name + "PathfindingMenu").Item("Enabled").GetValue<bool>())
                {
                    var pathfindingMenu = Menu.SubMenu(this.Name + "PathfindingMenu");

                    var targetedVector = Vector3.Zero;

                    switch (pathfindingMenu.Item("ModeTarget").GetValue<StringList>().SelectedIndex)
                    {
                        case 0:
                            targetedVector = Game.CursorPos;
                            break;
                        case 1:
                            var target = TargetSelector.GetTarget(5000, TargetSelector.DamageType.Physical);

                            if (target != null && !target.IsValid && !target.IsZombie)
                            {
                                if (!this.BlacklistChampions.Contains(target))
                                {
                                    if (this.Menu.Item("Prediction").GetValue<bool>())
                                    {
                                        targetedVector = Prediction.GetPrediction(target, Variables.Player.Distance(target) / ProviderE.Speed()).UnitPosition;
                                    }
                                    else
                                    {
                                        targetedVector = target.ServerPosition;
                                    }
                                }
                                else
                                {
                                    targetedVector = Game.CursorPos;
                                }
                            }
                            else
                            {
                                targetedVector = Game.CursorPos;
                            }
                            break;
                    }

                    #endregion


                    #region Path Settings

                    ProviderE.GenerateGrid(Variables.Player.ServerPosition, targetedVector, SweepingBladeLogicProvider.Units.All);

                    if (this.ProviderE.GridGenerator.Grid == null)
                    {
                        return;
                    }

                    // TODO: PRIORITY MEDIUM > Make some more settings for this, such as Danger Value of skillshot etc.
                    if (pathfindingMenu.Item("PathAroundSkillShots").GetValue<bool>())
                    {
                        var skillshotList =
                            LeagueSharp.SDK.Tracker.DetectedSkillshots.Where(x => x.SData.DangerValue > 1).ToList();

                        this.ProviderE.GridGenerator.RemovePathesThroughSkillshots(skillshotList);
                    }

                    this.ProviderE.FinalizeGrid();

                    this.Path = this.ProviderE.GetPath(targetedVector);

                    #endregion


                    #region Path Execute

                    if (this.Path?.Connections != null && this.Path?.Connections?.Count > 0)
                    {
                        PathCopy = Path;
                        // Auto-Dashing
                        if (this.Path.Connections.First().IsDash)
                        {
                            if (pathfindingMenu.Item("AutoDashing").GetValue<bool>()
                                && Variables.Player.Distance(this.Path.Connections.First().Over.ServerPosition) <= Variables.Spells[SpellSlot.E].Range
                                && this.Path.Connections.First().To.Position.Distance(Variables.Player.ServerPosition.Extend(Path.Connections.First().Over.ServerPosition, Variables.Spells[SpellSlot.E].Range)) <= 50)
                            {
                                Execute(this.Path.Connections.First().Over);
                            }
                        }

                        // TODO: Priority Low - Med
                        // Notice: Make it a way that it won't cancel AA
                        if (!this.Path.Connections.First().IsDash && !Variables.Player.IsWindingUp)
                        {
                            // Auto-Walk-To-Dash
                            if (pathfindingMenu.Item("AutoWalkToDash").GetValue<bool>())
                            {
                                // Connection considered to walk behind a unit
                                if (Path.Connections.First().Lenght <= 50)
                                {
                                    // Walk logic here
                                }
                            }

                            // Auto-Walking
                            if (pathfindingMenu.Item("AutoWalking").GetValue<bool>())
                            {
                                if (Variables.Player.ServerPosition.Distance(Path.Connections.First().To.Position) <= 50)
                                {
                                    Path.RemoveConnection(Path.Connections.First());
                                }

                                if (Path.Connections.First().Lenght > 50)
                                {
                                    // Walk logic here
                                }
                            }

                        }

                    }

                    #endregion
                }

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        public void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender != Variables.Player || args.SData.Name != "YasuoDashWrapper")
            {
                return;
            }

            var connectionToRemove = this.Path?.Connections?.First(x => x.Over == args.Target);

            if (connectionToRemove != null)
            {
                this.Path.RemoveConnection(connectionToRemove);
            }
        }

        public void OnDraw(EventArgs args)
        {
            if (Variables.Player.IsDead 
                || Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
            {
                return;
            }


            if (Path != null && Path.Connections.Count > 0)
            {
                var drawingMenu = this.Menu.SubMenu(this.Name + "Drawings");

                if (drawingMenu.Item(this.Name + "Enabled").GetValue<bool>())
                {
                    if (drawingMenu.Item(this.Name + "SmartDrawings").GetValue<bool>())
                    {
                        if (!Path.BuildsUpShield && Path.Connections.All(x => !x.IsDash))
                        {
                            return;
                        }

                        // TODO: Color Path light blue
                        if (Path.BuildsUpShield)
                        {
                            
                        }
                    }
                    if (drawingMenu.Item(this.Name + "PathDashColor").GetValue<Circle>().Active)
                    {
                        var linewidth = drawingMenu.Item(this.Name + "PathDashWidth").GetValue<Slider>().Value;
                        var color = drawingMenu.Item(this.Name + "PathDashColor").GetValue<Circle>().Color;

                        Path.DashLineWidth = linewidth;
                        Path.DashColor = color;
                    }

                    if (drawingMenu.Item(this.Name + "PathWalkColor").GetValue<Circle>().Active)
                    {
                        var linewidth = drawingMenu.Item(this.Name + "PathWalkWidth").GetValue<Slider>().Value;
                        var color = drawingMenu.Item(this.Name + "PathWalkColor").GetValue<Circle>().Color;

                        Path.WalkLineWidth = linewidth;
                        Path.WalkColor = color;
                    }

                    if (drawingMenu.Item(this.Name + "CirclesColor").GetValue<Circle>().Active)
                    {
                        var linewidth = drawingMenu.Item(this.Name + "CirclesLineWidth").GetValue<Slider>().Value;
                        var radius = drawingMenu.Item(this.Name + "CirclesRadius").GetValue<Slider>().Value;
                        var color = drawingMenu.Item(this.Name + "CirclesColor").GetValue<Circle>().Color;

                        Path.CircleLineWidth = linewidth;
                        Path.CircleRadius = radius;
                        Path.CircleColor = color;
                    }

                    Path.Draw();
                }
            }
        }

        private void Execute(Obj_AI_Base unit)
        {
            try
            {
                if (unit == null || !unit.IsValidTarget() || unit.HasBuff("YasuoDashWrapper"))
                {
                    return;
                }

                Variables.Spells[SpellSlot.E].CastOnUnit(unit);
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Skills/Combo/SweepingBlade/Execute(): " + ex);
            }
        }
    }
}