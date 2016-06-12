namespace AssemblyName.Champion.Modules.Hidden
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using AssemblyName.MediaLib.Classes.Feature;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal class SpellInitializer : FeatureChild<Base.Modules.Assembly.Assembly>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellInitializer"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public SpellInitializer(Base.Modules.Assembly.Assembly parent)
            : base(parent)
        {
            this.OnLoad();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether this <see cref="FeatureBase" /> is hidden.
        /// </summary>
        /// <value>
        ///     <c>true</c> if hidden; otherwise, <c>false</c>.
        /// </value>
        public override bool Hidden => true;

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name => "(Hidden) SpellInitializer";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Raises the <see cref="E:Draw" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnDraw(EventArgs args)
        {
        }

        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnUpdate(EventArgs args)
        {
        }

        #endregion

        #region Methods

        // TODO
        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            GlobalVariables.Spells = new Dictionary<SpellSlot, Spell>()
                                         {
                                             { SpellSlot.Q, new Spell(SpellSlot.Q, 475) },
                                             { SpellSlot.W, new Spell(SpellSlot.W, 400) },
                                             {
                                                 SpellSlot.E,
                                                 new Spell(
                                                 SpellSlot.E,
                                                 475,
                                                 TargetSelector.DamageType.Magical)
                                             },
                                             { SpellSlot.R, new Spell(SpellSlot.R, 1200) },
                                         };

            base.OnEnable();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        #endregion
    }
}