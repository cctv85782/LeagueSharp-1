namespace AssemblyName.Champion.OrbwalkingModes.Combo
{
    using System;

    using AssemblyName.Champion.LogicProvider;
    using AssemblyName.MediaLib.Classes.Feature;
    using AssemblyName.MediaLib.Utility;

    using LeagueSharp;
    using LeagueSharp.Common;

    internal class Flash : FeatureChild<Combo>
    {
        #region Fields

        /// <summary>
        ///     The Flash logicprovider
        /// </summary>
        private FlashLogicProvider provider;

        /// <summary>
        ///     The flash slot
        /// </summary>
        internal SpellSlot FlashSlot;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Flash" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public Flash(Combo parent)
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
        public override string Name => "Flash";

        #endregion

        #region Public Methods and Operators

        // TODO
        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnUpdate(EventArgs args)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable()
        {
            Events.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            Events.OnUpdate += this.OnUpdate;
            base.OnEnable();
        }

        /// <summary>
        ///     Called when [initialize].
        /// </summary>
        protected override void OnInitialize()
        {
            this.provider = new FlashLogicProvider();

            foreach (var spell in GlobalVariables.Player.Spellbook.Spells)
            {
                if (spell.Name == "SummonerFlash")
                {
                    this.FlashSlot = spell.Slot;
                }
            }

            base.OnInitialize();
        }

        // TODO: Add Settings
        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        // TODO
        /// <summary>
        ///     Executes to the specified position.
        /// </summary>
        private void Execute()
        {
        }

        #endregion
    }
}