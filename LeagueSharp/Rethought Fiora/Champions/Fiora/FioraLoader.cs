namespace Rethought_Fiora.Champions.Fiora
{
    #region Using Directives

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Fiora.Champions.Fiora.Modules.Core;

    #endregion

    internal class FioraLoader : LoadableBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name that will get displayed.
        /// </summary>
        /// <value>
        ///     The name of the displaying.
        /// </value>
        public override string DisplayingName { get; set; } = "Rethought Fiora";

        /// <summary>
        ///     Gets or sets the internal name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string InternalName { get; set; } = "Fiora";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public override void Load()
        {
            var superParent = new SuperParent(this.DisplayingName);

            var coreParent = new Parent("Core");

            var orbwalker = new OrbwalkerModule(superParent.Menu);
            superParent.AddChildren(orbwalker);

            var spells = new SpellModule();
            coreParent.AddChildren(spells);


        }

        #endregion
    }
}