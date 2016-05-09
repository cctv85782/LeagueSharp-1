// TODO: Add Multi Pathing System. The Idea is to get some paths that are equally good and compare them then. This way you could do things like if Path A is safer than Path B in Situation X choose Path A

namespace Yasuo.Modules.Flee
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Yasuo.Common.Classes;
    using Yasuo.Common.LogicProvider;
    using Yasuo.Common.Objects;

    internal class SweepingBlade : Child<Modules>
    {
        #region Fields

        /// <summary>
        ///     The path
        /// </summary>
        public Path Path;

        /// <summary>
        ///     The E logicprovider
        /// </summary>
        public SweepingBladeLogicProvider ProviderE;

        /// <summary>
        ///     The Turret logicprovider
        /// </summary>
        public TurretLogicProvider ProviderTurret;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SweepingBlade" /> class.
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
        public override string Name => "Flee";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Raises the <see cref="E:Draw" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnDraw(EventArgs args)
        {
            //if (this.Path = null)
            //{
            //    Console.WriteLine("Cant draw Path == null");
            //}
            //this.GapClosePath?.RealPath.Draw();
            this.Path?.Draw();
            //this.Path?.DashObject?.Draw();
        }

        /// <summary>
        ///     Raises the<see cref="E:OnProcessSpellCast" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        public void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender != GlobalVariables.Player || args.SData.Name != "YasuoDashWrapper")
            {
                return;
            }

            var connectionToRemove = this.Path?.Connections.First(x => x.Unit == args.Target);

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
            try
            {
                if (!this.Menu.Item(this.Name + "Keybind").GetValue<KeyBind>().Active
                    || !GlobalVariables.Spells[SpellSlot.E].IsReady())
                {
                    return;
                }

                GlobalVariables.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

                var targetedVector = Game.CursorPos;

                if (targetedVector != Vector3.Zero)
                {
                    this.Path = this.ProviderE.GetPath(targetedVector);
                }

                // if a path is given, and the first unit of the path is in dash range
                if (this.Path != null
                    && GlobalVariables.Player.Distance(this.Path.Connections.First().Unit.ServerPosition)
                    <= GlobalVariables.Spells[SpellSlot.E].Range)
                {
                    Execute(Path.Connections.First().Unit);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
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
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            // Spell Settings
            this.Menu.AddItem(
                new MenuItem(this.Name + "Keybind", "Keybind").SetValue(new KeyBind('A', KeyBindType.Press)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "PathAroundSkillShots", "[Experimental] Try to Path around Skillshots")
                    .SetValue(true)
                    .SetTooltip("if this is enabled, the assembly will path around a skillshot if a path is given"));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        /// <summary>
        ///     Executes on the specified unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        private static void Execute(Obj_AI_Base unit)
        {
            try
            {
                if (unit == null || !unit.IsValidTarget() || unit.HasBuff("YasuoDashWrapper"))
                {
                    return;
                }

                GlobalVariables.Spells[SpellSlot.E].CastOnUnit(unit);
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Modules/Flee/SweepingBlade/Execute(): " + ex);
            }
        }

        #endregion
    }
}