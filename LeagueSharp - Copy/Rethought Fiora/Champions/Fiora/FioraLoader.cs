namespace Rethought_Fiora.Champions.Fiora
{
    #region Using Directives

    using System.Collections.Generic;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Fiora.Champions.Fiora.Modules.Core;
    using Rethought_Fiora.Champions.Fiora.Modules.Core.SpellsModule;
    using Rethought_Fiora.Champions.Fiora.Modules.Core.TargetSubmitterModule;
    using Rethought_Fiora.Champions.Fiora.Modules.LogicProvider;
    using Rethought_Fiora.Champions.Fiora.Modules.LogicProvider.PassiveLogicProvider;

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
        public override string DisplayingName { get; set; } = RethoughtLib.Utility.String.ToTitleCase("Fiora the explorer");

        /// <summary>
        ///     Gets or sets the internal name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string InternalName { get; set; } = "Fuck you laura, I hate that god damn name.";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public override void Load()
        {
            var superParent = new SuperParent(this.DisplayingName);

            var coreParent = new Parent("Core");

            var logicProviderParent = new Parent("Logic Provider");

            var comboParent = new Parent("Combo");
            var laneClearParent = new Parent("LaneClear");
            var mixedParent = new Parent("Mixed");
            var lasthitParent = new Parent("LastHit");

            var comboQ = new Modules.Combo.Q();
            comboParent.AddChild(comboQ);

            logicProviderParent.AddChildren(
                new List<Base>() { new PassiveLogicProviderModule(), new QLogicProviderModule() });

            var spells = new SpellsModule();
            coreParent.AddChild(spells);

            superParent.AddChildren(new List<Base>()
                                        {
                                            new OrbwalkerModule(superParent.Menu),
                                            coreParent,
                                            logicProviderParent,
                                        });

            superParent.OnLoadInvoker();
        }

        #endregion
    }
}