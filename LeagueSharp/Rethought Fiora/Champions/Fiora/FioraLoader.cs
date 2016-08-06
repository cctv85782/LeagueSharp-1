namespace Rethought_Fiora.Champions.Fiora
{
    #region Using Directives

    using System.Collections.Generic;

    using LeagueSharp.Common;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.Utility;

    using Rethought_Fiora.Champions.Fiora.Modules;
    using Rethought_Fiora.Champions.Fiora.Modules.Core;
    using Rethought_Fiora.Champions.Fiora.Modules.Core.SpellsModule;
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
        public override string DisplayName { get; set; } = String.ToTitleCase("Fiora the explorer");

        /// <summary>
        ///     Gets or sets the internal name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string InternalName { get; set; } = "Rethought_Fiora";

        /// <summary>
        ///     Gets or sets the tags.
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public override IEnumerable<string> Tags { get; set; } = new List<string>() { "Fiora " };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public override void Load()
        {
            var superParent = new SuperParent(this.DisplayName);

            var coreParent = new Parent("Core");

            var logicProviderParent = new Parent("Logic Provider");

            var comboParent = new Parent("Combo");
            var laneClearParent = new Parent("LaneClear");
            var mixedParent = new Parent("Mixed");
            var lasthitParent = new Parent("LastHit");

            var evadeParent = new Parent("Evade");

            var wallLogicProvider = new WallLogicProvider();

            coreParent.AddChildren(
                new ChildBase[]
                    {
                        new SpellsModule(),
                    });

            comboParent.AddChildren(
                new ChildBase[]
                    {
                        new Modules.Combo.Q(),
                        new Modules.Combo.W(),
                        new Modules.Combo.E(),
                        new Modules.Combo.R(),
                    });

            laneClearParent.AddChildren(
                new ChildBase[]
                    {
                        new Modules.LaneClear.Q(),
                        new Modules.LaneClear.E(),
                        new Modules.LastHit.E(Orbwalking.OrbwalkingMode.LaneClear),
                    });

            mixedParent.AddChildren(
                new ChildBase[]
                    {
                        new Modules.Mixed.Q(),
                        new Modules.Mixed.E(),
                        new Modules.LastHit.E(Orbwalking.OrbwalkingMode.Mixed),

                    });

            lasthitParent.AddChildren(
                new ChildBase[]
                    {
                        new Modules.LastHit.Q(Orbwalking.OrbwalkingMode.LastHit, wallLogicProvider),
                        new Modules.LastHit.E(Orbwalking.OrbwalkingMode.LastHit),
                    });

            logicProviderParent.AddChildren(
                new ChildBase[]
                    {
                        new PassiveLogicProviderModule(), new QLogicProviderModule(), wallLogicProvider
                    });

            superParent.AddChildren(
                new Base[]
                    {
                        new OrbwalkerModule(superParent.Menu),
                        coreParent,
                        comboParent,
                        laneClearParent,
                        mixedParent,
                        lasthitParent,
                        logicProviderParent,
                    });

            superParent.OnLoadInvoker();
        }

        #endregion
    }
}