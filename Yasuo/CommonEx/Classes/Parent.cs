namespace Yasuo.CommonEx.Classes
{
    using System;

    using LeagueSharp.Common;

    public abstract class Parent : Base
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Parent"/> class.
        /// </summary>
        protected Parent()
        {
            this.OnLoad();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether this <see cref="Base" /> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public override bool Enabled
            => !this.Unloaded && this.Menu != null && this.Menu.Item(this.Name + "Enabled").GetValue<bool>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Called when [load].
        /// </summary>
        public void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            GlobalVariables.RootMenu.AddSubMenu(this.Menu);

            this.OnInitialize();
        }

        #endregion
    }
}