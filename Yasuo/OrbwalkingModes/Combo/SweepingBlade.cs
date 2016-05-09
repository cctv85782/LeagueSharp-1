namespace Yasuo.OrbwalkingModes.Combo
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Extensions;
    using Yasuo.Common.Extensions.MenuExtensions;
    using Yasuo.Common.LogicProvider;
    using Yasuo.Common.Objects;
    using Yasuo.Common.Objects.Pathfinding;
    using Yasuo.Common.Utility;

    using Dash = Yasuo.Common.Objects.Dash;

    #endregion

    internal class SweepingBlade : Child<Combo>
    {
        #region Static Fields

        /// <summary>
        ///     The path copy
        /// </summary>
        internal static Path PathCopy;

        #endregion

        #region Fields

        /// <summary>
        ///     The blacklist champions
        /// </summary>
        public List<Obj_AI_Base> BlacklistChampions = new List<Obj_AI_Base>();

        /// <summary>
        ///     The blacklist Menu
        /// </summary>
        public BlacklistMenu BlacklistMenu;

        /// <summary>
        ///     The pathfinder Menu
        /// </summary>
        public PathfindingMenu PathfindingMenu;

        /// <summary>
        ///     The path
        /// </summary>
        internal Path Path;

        /// <summary>
        ///     The pathfinder
        /// </summary>
        private Pathfinder pathfinder;

        /// <summary>
        ///     The provider e
        /// </summary>
        private SweepingBladeLogicProvider providerE;

        /// <summary>
        ///     The provider turret
        /// </summary>
        private TurretLogicProvider providerTurret;

        /// <summary>
        /// The targets
        /// </summary>
        protected List<Obj_AI_Hero> Targets; 

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

        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnUpdate(EventArgs args)
        {
            this.SoftReset();

            if (GlobalVariables.Player.IsDead || GlobalVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || !GlobalVariables.Spells[SpellSlot.E].IsReady())
            {
                return;
            }

            this.GetTargets();

            this.LogicOnChampion();

            this.pathfinder.CalculatePath();

            this.pathfinder.ExecutePath();

            Path = this.pathfinder.Path;

            PathCopy = this.pathfinder.Path;
        }

        /// <summary>
        /// Gets the targets.
        /// </summary>
        private void GetTargets()
        {
            Targets =
                HeroManager.Enemies.Where(
                    x =>
                    x.Health > 0 && x.IsValid
                    && x.Distance(GlobalVariables.Player.ServerPosition) <= 1000).ToList();
        }

        /// <summary>
        /// Resets the fields/properties
        /// </summary>
        private void SoftReset()
        {
            this.Targets = new List<Obj_AI_Hero>();
            this.Path = new Path();

            PathCopy = new Path();
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
            this.providerE = new SweepingBladeLogicProvider();
            this.providerTurret = new TurretLogicProvider();

            this.Targets = new List<Obj_AI_Hero>();

            base.OnInitialize();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.BlacklistMenu = new BlacklistMenu(this.Menu, "Blacklist");

            this.PathfindingMenu = new PathfindingMenu(this.Menu, "Pathfinder");

            this.pathfinder = new Pathfinder(PathfindingMenu);

            this.SetupOnChampionMenu();

            this.SetupGeneralMenu();

            this.SetupDrawingMenu();

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        /// <summary>
        ///     Executes on the specified unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        private static void Execute(Obj_AI_Base unit)
        {
            if (unit == null || !unit.IsValidTarget() || unit.HasBuff("YasuoDashWrapper"))
            {
                return;
            }

            GlobalVariables.Spells[SpellSlot.E].CastOnUnit(unit);
        }

        /// <summary>
        ///     Executes Logic to dash on champion
        /// </summary>
        private void LogicOnChampion()
        {
            var target = TargetSelector.SelectedTarget
                            ?? TargetSelector.GetTarget(
                                GlobalVariables.Spells[SpellSlot.E].Range,
                                TargetSelector.DamageType.Magical);

            if (target == null || this.BlacklistChampions.Contains(target)
                || !target.IsValidTarget(GlobalVariables.Spells[SpellSlot.E].Range))
            {
                return;
            }
            
            var dash = new Dash(GlobalVariables.Player.ServerPosition, target);

            if (target.Health < this.providerE.GetDamage(target) && !GlobalVariables.Spells[SpellSlot.Q].IsReady())
            {
                var meanvector =
                    Helper.GetMeanVector3(
                        Targets.Where(x => x.Distance(dash.EndPosition) <= 1000)
                            .Select(x => x.ServerPosition)
                            .ToList());

                if (meanvector == target.ServerPosition)
                {
                    Execute(target);
                }

                if (GlobalVariables.Player.Health
                    > Menu.SubMenu(this.Name + "EOnChampionMenu")
                            .Item(this.Name + "MaxHealthDashOut")
                            .GetValue<Slider>()
                            .Value)
                {
                    if (dash.EndPosition.Distance(meanvector)
                        <= GlobalVariables.Player.Distance(meanvector))
                    {
                        Execute(target);
                    }
                }
                else
                {
                    if (dash.EndPosition.Distance(meanvector)
                        >= GlobalVariables.Player.Distance(meanvector))
                    {
                        Execute(target);
                    }
                }
            }

            if (!GlobalVariables.Player.HasQ3())
            {
                return;
            }

            // 1 v 1
            if (dash.EndPosition.CountEnemiesInRange(1000) == 1)
            {
                if (dash.HeroesHitCircular.Contains(target))
                {
                    Execute(target);
                }
            }
            else
            {
                var heroes = Targets.Where(x => x.Distance(dash.EndPosition) <= 1000);

                if (dash.HeroesHitCircular.Count >= (heroes.Count() / 2))
                {
                    Execute(target);
                }
            }
        }

        /// <summary>
        /// Setups the drawing menu.
        /// </summary>
        private void SetupDrawingMenu()
        {
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
        }


        /// <summary>
        /// Setups the general menu.
        /// </summary>
        private void SetupGeneralMenu()
        {
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
        }


        /// <summary>
        /// Setups the on champion menu.
        /// </summary>
        private void SetupOnChampionMenu()
        {
                var onchampion = new Menu("Dash On Champion", this.Name + "EOnChampionMenu");

            onchampion.AddItem(
                new MenuItem(this.Name + "MaxHealthDashOut", "Dash defensively if Health % <=").SetValue(new Slider(30)));

            onchampion.AddItem(
                new MenuItem(this.Name + "OnlyKillableCombo", "Only dash on champion if killable by Combo").SetValue(
                    true));

            onchampion.AddItem(new MenuItem(this.Name + "Whirlwind", "Smart EQ").SetValue(true));

            this.Menu.AddSubMenu(onchampion);
        }

        #endregion
    }
}