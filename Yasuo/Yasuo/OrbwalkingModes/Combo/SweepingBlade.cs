namespace Yasuo.Yasuo.OrbwalkingModes.Combo
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Yasuo.CommonEx;
    using global::Yasuo.CommonEx.Algorithm.Djikstra;
    using global::Yasuo.CommonEx.Classes;
    using global::Yasuo.CommonEx.Extensions;
    using global::Yasuo.CommonEx.Menu;
    using global::Yasuo.CommonEx.Menu.Presets;
    using global::Yasuo.CommonEx.Objects;
    using global::Yasuo.CommonEx.Objects.Pathfinding;
    using global::Yasuo.CommonEx.Utility;
    using global::Yasuo.Yasuo.LogicProvider;
    using global::Yasuo.Yasuo.Menu.MenuSets.OrbwalkingModes.Combo;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Dash = global::Yasuo.CommonEx.Objects.Dash;

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
        private void OnDraw(EventArgs args)
        {
            if (GlobalVariables.Player.IsDead || GlobalVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
            {
                return;
            }

            if (this.Path != null && this.Path.Connections.Count > 0)
            {
                var drawingMenu = this.Menu.SubMenu(this.Name + "Drawings");

                if (drawingMenu.Item(this.Name + "Enabled").GetValue<bool>())
                {
                    if (drawingMenu.Item(this.Name + "SmartDrawings").GetValue<bool>())
                    {
                        if (!this.Path.BuildsUpShield && this.Path.Connections.All(x => !x.IsDash))
                        {
                            return;
                        }

                        // TODO: Color Path light blue
                        if (this.Path.BuildsUpShield)
                        {
                        }
                    }
                    if (drawingMenu.Item(this.Name + "PathDashColor").GetValue<Circle>().Active)
                    {
                        var linewidth = drawingMenu.Item(this.Name + "PathDashWidth").GetValue<Slider>().Value;
                        var color = drawingMenu.Item(this.Name + "PathDashColor").GetValue<Circle>().Color;

                        this.Path.DashLineWidth = linewidth;
                        this.Path.DashColor = color;
                    }

                    if (drawingMenu.Item(this.Name + "PathWalkColor").GetValue<Circle>().Active)
                    {
                        var linewidth = drawingMenu.Item(this.Name + "PathWalkWidth").GetValue<Slider>().Value;
                        var color = drawingMenu.Item(this.Name + "PathWalkColor").GetValue<Circle>().Color;

                        this.Path.WalkLineWidth = linewidth;
                        this.Path.WalkColor = color;
                    }

                    if (drawingMenu.Item(this.Name + "CirclesColor").GetValue<Circle>().Active)
                    {
                        var linewidth = drawingMenu.Item(this.Name + "CirclesLineWidth").GetValue<Slider>().Value;
                        var radius = drawingMenu.Item(this.Name + "CirclesRadius").GetValue<Slider>().Value;
                        var color = drawingMenu.Item(this.Name + "CirclesColor").GetValue<Circle>().Color;

                        this.Path.CircleLineWidth = linewidth;
                        this.Path.CircleRadius = radius;
                        this.Path.CircleColor = color;
                    }

                    this.Path.Draw();
                }
            }
        }

        /// <summary>
        ///     Called when [process spell cast].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        private void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
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
        private void OnUpdate(EventArgs args)
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

            this.Path = this.pathfinder.Path;

            PathCopy = this.pathfinder.Path;
        }

        /// <summary>
        /// Gets the targets.
        /// </summary>
        private void GetTargets()
        {
            this.Targets =
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
            Events.OnUpdate -= this.OnUpdate;
            Obj_AI_Base.OnProcessSpellCast -= this.OnProcessSpellCast;
            Drawing.OnDraw -= this.OnDraw;
            base.OnDisable();
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            Events.OnUpdate += this.OnUpdate;
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
        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            var menuGenerator = new MenuGenerator(new SweepingBladeMenu(this.Menu));

            menuGenerator.Generate();

            this.BlacklistMenu = new BlacklistMenu(this.Menu, "Blacklist");

            this.PathfindingMenu = new PathfindingMenu(this.Menu, "Pathfinder");

            this.pathfinder = new Pathfinder(this.PathfindingMenu);

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

            GlobalVariables.CastManager.Queque.Enqueue(3, () => GlobalVariables.Spells[SpellSlot.E].CastOnUnit(unit));
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
                    Helper.GetMeanVector3(this.Targets.Where(x => x.Distance(dash.EndPosition) <= 1000)
                            .Select(x => x.ServerPosition)
                            .ToList());

                if (meanvector == target.ServerPosition)
                {
                    Execute(target);
                }

                if (GlobalVariables.Player.Health
                    > this.Menu.SubMenu(this.Name + "EOnChampionMenu")
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
                var heroes = this.Targets.Where(x => x.Distance(dash.EndPosition) <= 1000);

                if (dash.HeroesHitCircular.Count >= (heroes.Count() / 2))
                {
                    Execute(target);
                }
            }
        }
        
        #endregion
    }
}