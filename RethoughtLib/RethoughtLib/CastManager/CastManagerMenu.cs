namespace RethoughtLib.CastManager
{
    #region Using Directives

    using System;
    using System.Reflection;

    using LeagueSharp.Common;

    using global::RethoughtLib;
    using global::RethoughtLib.Classes.Feature;
    using global::RethoughtLib.Events;
    using global::RethoughtLib.Utility;

    #endregion

    internal class CastManagerMenu : FeatureChild<Root>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CastManagerMenu" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public CastManagerMenu(Root parent)
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
        public override string Name => "Cast Manager (Priority System)";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable()
        {
            Events.OnPostUpdate -= OnPostUpdate;
            GlobalVariables.Debug = false;
            base.OnDisable();
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            Events.OnPostUpdate += OnPostUpdate;
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

        /// <summary>
        /// Raises the <see cref="E:PostUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void OnPostUpdate(EventArgs args)
        {
            CastManager.Instance.Process();
        }

        #endregion
    }
}