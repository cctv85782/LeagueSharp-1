namespace RethoughtLib.FeatureSystem.Abstract_Classes
{
    #region Using Directives

    using System;

    using global::RethoughtLib.FeatureSystem.Switches;

    using LeagueSharp.Common;

    #endregion

    public abstract class SuperParentBase : ParentBase
    {
        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            this.Menu.AddToMainMenu();

            base.OnLoad(sender, featureBaseEventArgs);
        }

        /// <summary>
        /// Initializes the menu, overwrite this method to change the menu type. Do not overwrite if you only want to change
        /// the menu content.
        /// </summary>
        protected override void SetMenu()
        {
            this.Menu = new Menu(this.Name, this.Name, true);

            this.Switch = new BoolSwitch(this.Menu, "Enabled", true, this);
        }

        /// <summary>
        ///     Called when [unload].
        /// </summary>
        protected override void OnUnload(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Menu.Remove(this.Menu);

            foreach (var child in this.Children)
            {
                child.Key.OnUnLoadInvoker();
            }
        }

        #endregion
    }
}