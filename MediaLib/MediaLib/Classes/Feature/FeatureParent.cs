﻿namespace RethoughtLib.Classes.Feature
{
    #region Using Directives

    using LeagueSharp.Common;

    #endregion

    /// <summary>
    ///     Feature Parent class deriving from FeatureBase
    /// </summary>
    /// <seealso cref="RethoughtLib.Classes.Feature.FeatureBase" />
    public abstract class FeatureParent : FeatureBase
    {
        private readonly Menu rootMenu;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureParent"/> class.
        /// </summary>
        /// <param name="rootMenu">The root menu. (the generated menu will get attached to it)</param>
        protected FeatureParent(Menu rootMenu)
        {
            this.rootMenu = rootMenu;
            this.OnLoad();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether this <see cref="FeatureBase" /> is enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public override bool Enabled
            =>
                !this.Unloaded && this.Menu?.Item(this.Name + "Enabled") != null
                && this.Menu.Item(this.Name + "Enabled").GetValue<bool>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Called when [load].
        /// </summary>
        public void OnLoad()
        {
            if (!string.IsNullOrWhiteSpace(this.Name) || !this.Hidden)
            {
                this.Menu = new Menu(this.Name, this.Name);

                this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

                this.rootMenu.AddSubMenu(this.Menu);
            }

            this.OnInitialize();
        }

        #endregion
    }
}