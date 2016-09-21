namespace Rethought_Clicks.Modules
{
    #region Using Directives

    using System.Collections.Generic;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    #endregion

    internal class Loader : LoadableBase
    {
        #region Public Properties

        public override string DisplayName { get; set; } = "Rethought Clicks";

        public override string InternalName { get; set; } = "rethought_clicks_V1";

        public override IEnumerable<string> Tags { get; set; } = new[] { "Version_1" };

        #endregion

        #region Public Methods and Operators

        public override void Load()
        {
            var superParent = new SuperParent(this.DisplayName);

            var clickModule = new ClickObserver();

            superParent.Add(clickModule);

            superParent.Load();
        }

        #endregion
    }
}