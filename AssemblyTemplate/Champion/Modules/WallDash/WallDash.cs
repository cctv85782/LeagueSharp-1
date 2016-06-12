namespace AssemblyName.Champion.Modules.WallDash
{
    #region Using Directives

    using System;

    using AssemblyName.MediaLib.Classes.Feature;
    using AssemblyName.MediaLib.Exceptions;
    using AssemblyName.MediaLib.Utility;
    using AssemblyName.MediaLib.LogicProvider;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal class WallDash : FeatureChild<Modules>
    {
        #region Fields

        /// <summary>
        ///     The provider wall dash
        /// </summary>
        public WallDashLogicProvider ProviderWallDash;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WallDash" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public WallDash(Modules parent)
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
        public override string Name => "Wall Dash";

        #endregion

        #region Public Methods and Operators

        // TODO
        /// <summary>
        ///     Raises the <see cref="E:Draw" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnDraw(EventArgs args)
        {
            if (GlobalVariables.Player.IsDead || GlobalVariables.Player.IsDashing()
                || !this.Menu.Item(this.Name + "Keybind").GetValue<KeyBind>().Active || !GlobalVariables.Debug)
            {
                return;
            }
        }

        // TODO
        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnUpdate(EventArgs args)
        {
            if (this.Menu.Item(this.Name + "Keybind").GetValue<KeyBind>().Active)
            {
                GlobalVariables.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }
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
            this.ProviderWallDash = new WallDashLogicProvider();
            base.OnInitialize();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Menu.AddItem(
                new MenuItem(this.Name + "Keybind", "Keybind").SetValue(new KeyBind('G', KeyBindType.Press)));

            this.Menu.AddItem(new MenuItem(this.Name + "MouseCheck", "Check for mouse position").SetValue(false));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MinWallWidth", "Minimum wall width: ").SetValue(
                    new Slider(150, 10, (int)GlobalVariables.Spells[SpellSlot.E].Range / 2)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "Helper", "How it works").SetTooltip(
                    "Hold down the Keybind to let the assembly perform a Dash over a unit that will be a WallDash"));

            //var advanced = new Menu("Advanced Settings", this.Name + "Advanced");

            //advanced.AddItem(
            //    new MenuItem(this.Name + "WidthReduction", "WallWidth %").SetValue(new Slider(100, 0, 200)));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        // TODO: Implementations
        /// <summary>
        ///     Executes on the specified target with the specified spellslot.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="spellslot">The spellslot</param>
        private static void Execute(Obj_AI_Base target, SpellSlot spellslot = SpellSlot.W)
        {
            switch (spellslot)
            {
                case SpellSlot.W:
                    break;
                case SpellSlot.Q:
                    break;
                default:
                    throw new MissingSpellSlotImplementationException("The SpellSlot choosed is not implemented");
            }

            /* Example:
            if (target.IsValidTarget())
            {
                CastManager.Manager.ForceAction(() =>
                GlobalVariables.Spells[spellslot].Cast(target));
            }
            */
        }

        #endregion
    }
}