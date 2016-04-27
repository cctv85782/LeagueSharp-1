namespace Yasuo.Modules.Assembly
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Utility;

    internal class Debug : Child<Assembly>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Debug"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public Debug(Assembly parent)
            : base(parent)
        {
            this.OnLoad();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name => "Debug";

        #endregion

        #region Methods

        /// <summary>
        /// Called when [disable].
        /// </summary>
        protected override void OnDisable()
        {
            GlobalVariables.Debug = false;
            base.OnDisable();
        }

        /// <summary>
        /// Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            GlobalVariables.Debug = true;
            base.OnEnable();
        }

        /// <summary>
        /// Called when [load].
        /// </summary>
        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(false));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        #endregion
    }
}