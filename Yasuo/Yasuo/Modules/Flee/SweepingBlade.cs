namespace Yasuo.Yasuo.Modules.Flee
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Yasuo.CommonEx;
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

    internal class SweepingBlade : Child<Modules>
    {
        #region Static Fields

        /// <summary>
        ///     The path copy
        /// </summary>
        internal static Path PathCopy;

        #endregion

        #region Fields

        /// <summary>
        ///     The path
        /// </summary>
        internal Path Path;

        /// <summary>
        ///     The pathfinder
        /// </summary>
        private PathfindingContainer pathfinder;

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
        /// Initializes a new instance of the <see cref="SweepingBlade"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public SweepingBlade(Modules parent)
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
        public override string Name => "Dash To Mouse";

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

            //if (this.Path != null && this.Path.Connections.Count > 0)
            //{
            //    var drawingMenu = this.Menu.SubMenu(this.Name + "Drawings");

            //    if (drawingMenu.Item(this.Name + "Enabled").GetValue<bool>())
            //    {
            //        if (drawingMenu.Item(this.Name + "SmartDrawings").GetValue<bool>())
            //        {
            //            if (!this.Path.BuildsUpShield && this.Path.Connections.All(x => !x.IsDash))
            //            {
            //                return;
            //            }

            //            // TODO: Color Path light blue
            //            if (this.Path.BuildsUpShield)
            //            {
            //            }
            //        }
            //        if (drawingMenu.Item(this.Name + "PathDashColor").GetValue<Circle>().Active)
            //        {
            //            var linewidth = drawingMenu.Item(this.Name + "PathDashWidth").GetValue<Slider>().Value;
            //            var color = drawingMenu.Item(this.Name + "PathDashColor").GetValue<Circle>().Color;

            //            this.Path.DashLineWidth = linewidth;
            //            this.Path.DashColor = color;
            //        }

            //        if (drawingMenu.Item(this.Name + "PathWalkColor").GetValue<Circle>().Active)
            //        {
            //            var linewidth = drawingMenu.Item(this.Name + "PathWalkWidth").GetValue<Slider>().Value;
            //            var color = drawingMenu.Item(this.Name + "PathWalkColor").GetValue<Circle>().Color;

            //            this.Path.WalkLineWidth = linewidth;
            //            this.Path.WalkColor = color;
            //        }

            //        if (drawingMenu.Item(this.Name + "CirclesColor").GetValue<Circle>().Active)
            //        {
            //            var linewidth = drawingMenu.Item(this.Name + "CirclesLineWidth").GetValue<Slider>().Value;
            //            var radius = drawingMenu.Item(this.Name + "CirclesRadius").GetValue<Slider>().Value;
            //            var color = drawingMenu.Item(this.Name + "CirclesColor").GetValue<Circle>().Color;

            //            this.Path.CircleLineWidth = linewidth;
            //            this.Path.CircleRadius = radius;
            //            this.Path.CircleColor = color;
            //        }

            //        this.Path.Draw();
            //    }
            //}
        }

        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnUpdate(EventArgs args)
        {
            this.SoftReset();

            if (GlobalVariables.Player.IsDead || !this.Menu.Item(this.Name + "Keybind").GetValue<KeyBind>().Active
                || !GlobalVariables.Spells[SpellSlot.E].IsReady())
            {
                return;
            }

            GlobalVariables.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            this.Path = this.pathfinder.GetPath();

            this.pathfinder.ExecutePath();

            PathCopy = this.Path;
        }

        /// <summary>
        /// Resets the fields/properties
        /// </summary>
        private void SoftReset()
        {
            this.Targets = new List<Obj_AI_Hero>();
            this.Path = null;

            PathCopy = null;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable()
        {
            Events.OnUpdate -= this.OnUpdate;
            Drawing.OnDraw -= this.OnDraw;
            base.OnDisable();
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            Events.OnUpdate += this.OnUpdate;
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

            this.pathfinder = new PathfindingContainer(new SimplePathfinder(this.Menu));

            this.Menu.AddItem(new MenuItem(this.Name + "Keybind", "Keybind").SetValue(new KeyBind('A', KeyBindType.Press)));

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        /// <summary>
        ///     Executes on the specified unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        private static void Execute(Obj_AI_Base unit)
        {
            if (unit == null || !unit.IsValidTarget())
            {
                return;
            }

            GlobalVariables.CastManager.ForceAction(() => GlobalVariables.Spells[SpellSlot.E].CastOnUnit(unit));
        }

        #endregion
    }
}