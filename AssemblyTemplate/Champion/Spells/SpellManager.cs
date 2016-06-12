namespace AssemblyName.Champion.Spells
{
    #region Using Directives

    using System;
    using System.Linq;

    using AssemblyName.Base.Modules.Assembly;
    using AssemblyName.MediaLib.Classes.Feature;
    using AssemblyName.MediaLib.Utility;

    using LeagueSharp.Common;

    #endregion

    internal class SpellManager : FeatureChild<Assembly>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CastManager" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public SpellManager(Base.Modules.Assembly.Assembly parent)
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
        public override string Name => "Spell Manager";

        #endregion

        #region Public Methods and Operators

        // TODO
        /// <summary>
        ///     Method that adjusts the spells to the current circumstances
        /// </summary>
        public void SetSpells()
        {
            if (GlobalVariables.Spells == null)
            {
                return;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable()
        {
            Events.OnPreUpdate -= this.OnPreUpdate;
            base.OnDisable();
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            Events.OnPreUpdate += this.OnPreUpdate;
            base.OnEnable();
        }

        // TODO
        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            // TODO: Add slider to adjust spells in lenght, speed etc.

            this.Menu.AddItem(
                new MenuItem(this.Name + "Q.Range", "Q (Non-Stacked) Range: ").SetValue(new Slider(475, 0, 2000)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "Q.Width", "Q (Non-Stacked) Width: ").SetValue(new Slider(20, 0, 2000)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "Q.Speed", "Q (Non-Stacked) Speed: ").SetValue(new Slider(10000, 0, 10000)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "Q2.Range", "Q (Stacked) Range: ").SetValue(new Slider(950, 0, 2000)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "Q2.Width", "Q (Stacked) Width: ").SetValue(new Slider(90, 0, 2000)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "Q2.Speed", "Q (Stacked) Speed: ").SetValue(new Slider(1250, 0, 2000)));

            // continue doing that...

            foreach (var item in
                this.Menu.Items.Where(x => x.Name != (this.Name + "Enabled")))
            {
                item.DontSave();
            }

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        /// <summary>
        ///     Raises the <see cref="E:PreUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnPreUpdate(EventArgs args)
        {
            this.SetSpells();
        }

        #endregion
    }
}