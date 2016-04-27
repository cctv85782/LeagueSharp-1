namespace Yasuo.OrbwalkingModes.Combo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;
    using LeagueSharp.SDK;

    using SharpDX;

    using Yasuo.Common.Algorithm.Djikstra;
    using Yasuo.Common.Classes;
    using Yasuo.Common.Extensions;
    using Yasuo.Common.Extensions.MenuExtensions;
    using Yasuo.Common.Objects;
    using Yasuo.Common.Provider;
    using Yasuo.Common.Utility;

    using Color = System.Drawing.Color;
    using Dash = Yasuo.Common.Objects.Dash;
    using Geometry = LeagueSharp.Common.Geometry;
    using TargetSelector = LeagueSharp.Common.TargetSelector;

    internal class SweepingBlade : Child<Combo>
    {
        #region Static Fields

        /// <summary>
        ///     The path copy
        /// </summary>
        internal static Path PathCopy = new Path();

        #endregion

        #region Fields

        /// <summary>
        ///     The blacklist
        /// </summary>
        public Blacklist Blacklist;

        /// <summary>
        ///     The blacklist champions
        /// </summary>
        public List<Obj_AI_Base> BlacklistChampions = new List<Obj_AI_Base>();

        /// <summary>
        ///     The provider e
        /// </summary>
        public SweepingBladeLogicProvider ProviderE;

        /// <summary>
        ///     The provider turret
        /// </summary>
        public TurretLogicProvider ProviderTurret;

        /// <summary>
        ///     The grid
        /// </summary>
        internal Grid Grid;

        /// <summary>
        ///     The path
        /// </summary>
        internal Path Path;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SweepingBlade" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public SweepingBlade(Combo parent)
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
        ///     Raises the <see cref="E:Draw" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnDraw(EventArgs args)
        {
            if (GlobalVariables.Player.IsDead || GlobalVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
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

        /// <summary>
        ///     Called when [process spell cast].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        public void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender != GlobalVariables.Player || args.SData.Name != "YasuoDashWrapper")
            {
                return;
            }

            var connectionToRemove = this.Path?.Connections?.First(x => x.Unit == args.Target);

            if (connectionToRemove != null)
            {
                this.Path.RemoveConnection(connectionToRemove);
            }
        }

        // TODO: Decompositing
        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnUpdate(EventArgs args)
        {
            try
            {
                Path = null;
                PathCopy = null;

                if (GlobalVariables.Player.IsDead || GlobalVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                    || !GlobalVariables.Spells[SpellSlot.E].IsReady())
                {
                    return;
                }

                #region Cast on Champion

                var targetE = TargetSelector.SelectedTarget
                              ?? TargetSelector.GetTarget(
                                  GlobalVariables.Spells[SpellSlot.E].Range,
                                  TargetSelector.DamageType.Magical);

                if (targetE != null && !this.BlacklistChampions.Contains(targetE)
                    && Geometry.Distance(targetE, GlobalVariables.Player.ServerPosition) <= GlobalVariables.Spells[SpellSlot.E].Range)
                {
                    var dash = new Dash(GlobalVariables.Player.ServerPosition, targetE);

                    if (targetE.Health < ProviderE.GetDamage(targetE))
                    {
                        var meanvector =
                            Helper.GetMeanVector3(
                                HeroManager.Enemies.Where(x => Geometry.Distance(x, dash.EndPosition) <= 1000)
                                    .Select(x => x.ServerPosition)
                                    .ToList());

                        if (GlobalVariables.Player.Health
                            > Menu.SubMenu(this.Name + "EOnChampionMenu")
                                  .Item(this.Name + "MaxHealthDashOut")
                                  .GetValue<Slider>()
                                  .Value)
                        {
                            if (Geometry.Distance(dash.EndPosition, meanvector)
                                <= Geometry.Distance(GlobalVariables.Player, meanvector))
                            {
                                Execute(targetE);
                            }
                        }
                        else
                        {
                            if (Geometry.Distance(dash.EndPosition, meanvector)
                                >= Geometry.Distance(GlobalVariables.Player, meanvector))
                            {
                                Execute(targetE);
                            }
                        }
                    }
                    if (GlobalVariables.Player.HasQ3())
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
                            var heroes = HeroManager.Enemies.Where(x => Geometry.Distance(x, dash.EndPosition) <= 1000);

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
                                if (this.BlacklistChampions.Any() && !this.BlacklistChampions.Contains(target))
                                {
                                    if (this.Menu.Item("Prediction").GetValue<bool>())
                                    {
                                        targetedVector =
                                            Prediction.GetPrediction(
                                                target,
                                                Geometry.Distance(GlobalVariables.Player, target) / ProviderE.Speed())
                                                .UnitPosition;
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

                    ProviderE.GenerateGrid(
                        GlobalVariables.Player.ServerPosition,
                        targetedVector,
                        SweepingBladeLogicProvider.Units.All);

                    if (this.ProviderE.GridGenerator.Grid == null
                        || !this.ProviderE.GridGenerator.Grid.Connections.Any())
                    {
                        return;
                    }

                    // TODO: PRIORITY MEDIUM > Make some more settings for this, such as Danger Value of skillshot etc.
                    if (pathfindingMenu.Item("PathAroundSkillShots").GetValue<bool>())
                    {
                        var skillshotList = Tracker.DetectedSkillshots.Where(x => x.SData.DangerValue > 1).ToList();

                        this.ProviderE.GridGenerator.RemovePathesThroughSkillshots(skillshotList);
                    }

                    // TODO: PRIORITY MEDIUM > Make some more settings for this, such as minions under turret etc. Ref; TurretLP
                    if (pathfindingMenu.Item("DontDashUnderTurret").GetValue<bool>())
                    {
                        foreach (var connection in this.ProviderE.GridGenerator.Grid.Connections)
                        {
                            if (!ProviderTurret.IsSafePosition(connection.To.Position))
                            {
                                this.ProviderE.GridGenerator.Grid.Connections.Remove(connection);
                            }
                        }
                        this.ProviderE.FinalizeGrid();
                    }

                    this.ProviderE.FinalizeGrid();

                    this.Path = this.ProviderE.GetPath(targetedVector);

                    #endregion

                    #region Path Execute

                    if (this.Path != null && Path.Connections != null)
                    {
                        PathCopy = Path;
                        // Auto-Dashing
                        if (this.Path.Connections.FirstOrDefault(x => x.IsDash) != null)
                        {
                            if (pathfindingMenu.Item("AutoDashing").GetValue<bool>()
                                && Geometry.Distance(
                                    GlobalVariables.Player,
                                    this.Path.Connections.First().Unit.ServerPosition)
                                <= GlobalVariables.Spells[SpellSlot.E].Range
                                && Geometry.Distance(
                                    this.Path.Connections.First().To.Position,
                                    Geometry.Extend(
                                        GlobalVariables.Player.ServerPosition,
                                        Path.Connections.First().Unit.ServerPosition,
                                        GlobalVariables.Spells[SpellSlot.E].Range)) <= 50)
                            {
                                Execute(this.Path.Connections.First().Unit);
                            }
                        }

                        // TODO: Priority Low - Med
                        // Notice: Make it a way that it won't cancel AA
                        if (!this.Path.Connections.First().IsDash && !GlobalVariables.Player.IsWindingUp)
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
                                if (Geometry.Distance(
                                    GlobalVariables.Player.ServerPosition,
                                    Path.Connections.First().To.Position) <= 50)
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

        private void OnChampion()
        {
            
        }

        private void Pathfinding()
        {
            
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable()
        {
            Game.OnUpdate -= this.OnUpdate;
            Obj_AI_Base.OnProcessSpellCast -= this.OnProcessSpellCast;
            Drawing.OnDraw -= this.OnDraw;
            base.OnDisable();
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            Game.OnUpdate += this.OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += this.OnProcessSpellCast;
            Drawing.OnDraw += this.OnDraw;
            base.OnEnable();
        }

        /// <summary>
        ///     Called when [initialize].
        /// </summary>
        protected override void OnInitialize()
        {
            this.ProviderE = new SweepingBladeLogicProvider();
            this.ProviderTurret = new TurretLogicProvider();

            base.OnInitialize();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            Blacklist = new Blacklist(this.Menu, "Blacklist");


            #region E on Champion

            var onchampion = new Menu("Dash On Champion", this.Name + "EOnChampionMenu");

            onchampion.AddItem(
                new MenuItem(this.Name + "MaxHealthDashOut", "Dash defensively if Health % <=").SetValue(new Slider(30)));

            onchampion.AddItem(
                new MenuItem(this.Name + "OnlyKillableCombo", "Only dash on champion if killable by Combo").SetValue(
                    true));

            onchampion.AddItem(new MenuItem(this.Name + "Whirlwind", "Smart EQ").SetValue(true));

            this.Menu.AddSubMenu(onchampion);

            #endregion

            #region EQ

            this.Menu.AddItem(
                new MenuItem(this.Name + "EQ", "Try to E for EQ").SetValue(true)
                    .SetTooltip("The assembly will try to E on a minion in order to Q"));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MinHitAOE", "Min HitCount for AOE").SetValue(new Slider(1, 1, 5)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MinOwnHealth", "Min Player Health%").SetValue(new Slider(15, 1))
                    .SetTooltip("The assembly will try to E on a minion in order to Q"));

            this.Menu.AddItem(
                new MenuItem(this.Name + "DontDashUnderTurret", "Don't Dash under turret").SetValue(true)
                    .SetTooltip("Assembly won't tower dive"));

            #endregion

            #region Drawings

            var drawingMenu = new Menu("Drawings", this.Name + "Drawings");

            drawingMenu.AddItem(
                new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true)
                    .SetTooltip("The assembly will Draw the expected path to the enemy"));

            drawingMenu.AddItem(
                new MenuItem(this.Name + "SmartDrawings", "Smart Drawings").SetValue(true)
                    .SetTooltip("Automaticall disables Drawings under certain circumstances and will do auto-coloring."));

            drawingMenu.AddItem(
                new MenuItem(this.Name + "PathDashColor", "Dashes").SetValue(new Circle(true, Color.DodgerBlue)));

            drawingMenu.AddItem(
                new MenuItem(this.Name + "PathDashWidth", "Width of lines").SetValue(new Slider(2, 1, 10)));

            drawingMenu.AddItem(new MenuItem(this.Name + "Seperator1", ""));

            drawingMenu.AddItem(
                new MenuItem(this.Name + "PathWalkColor", "Walking").SetValue(new Circle(true, Color.White)));

            drawingMenu.AddItem(
                new MenuItem(this.Name + "PathWalkWidth", "Width of lines").SetValue(new Slider(2, 1, 10)));

            drawingMenu.AddItem(
                new MenuItem(this.Name + "CirclesColor", "Draw Circles").SetValue(new Circle(true, Color.DodgerBlue)));

            drawingMenu.AddItem(
                new MenuItem(this.Name + "CirclesLineWidth", "Width of lines").SetValue(new Slider(2, 1, 10)));

            drawingMenu.AddItem(new MenuItem(this.Name + "CirclesRadius", "Radius").SetValue(new Slider(40, 10, 475)));

            this.Menu.AddSubMenu(drawingMenu);

            #endregion

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        private void Execute(Obj_AI_Base unit)
        {
            try
            {
                if (unit == null || !Utility.IsValidTarget(unit) || unit.HasBuff("YasuoDashWrapper"))
                {
                    return;
                }

                GlobalVariables.Spells[SpellSlot.E].CastOnUnit(unit);
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Skills/Combo/SweepingBlade/Execute(): " + ex);
            }
        }

        #endregion
    }
}