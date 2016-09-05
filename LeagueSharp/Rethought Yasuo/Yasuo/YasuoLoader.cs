namespace Rethought_Yasuo.Yasuo
{
    #region Using Directives

    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Algorithm.Pathfinding.AStar;
    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.CastManager.Implementations;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.LogicProvider.Modules;
    using RethoughtLib.Utility;

    using Rethought_Yasuo.Yasuo.Modules;
    using Rethought_Yasuo.Yasuo.Modules.Combo;
    using Rethought_Yasuo.Yasuo.Modules.Core;
    using Rethought_Yasuo.Yasuo.Modules.Core.SpellParent;
    using Rethought_Yasuo.Yasuo.Modules.Core.SpellParent.Implementations;
    using Rethought_Yasuo.Yasuo.Modules.Guardians;

    #endregion

    // TODO
    //> a priority menu entry for each module that uses the castmanager
    //  IE: Q > MenuItem(Slider) Priority

    internal class YasuoLoader : LoadableBase
    {
        #region Public Properties



        /// <summary>
        ///     Gets or sets the name that will get displayed.
        /// </summary>
        /// <value>
        ///     The name of the displaying.
        /// </value>
        public override string DisplayName { get; set; } = String.ToTitleCase("Rethought: Yasuo");

        /// <summary>
        ///     Gets or sets the internal name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string InternalName { get; set; } = "Rethought_Yasuo";

        /// <summary>
        ///     Gets or sets the tags.
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public override IEnumerable<string> Tags { get; set; } = new List<string>() { "Yasuo" };

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

            var castManagerModule = new CastManagerModule();

            var orbwalkerModule = new OrbwalkerModule(superParent.Menu);

            var comboParent = new OrbwalkingParent("Combo", orbwalkerModule.Orbwalker, Orbwalking.OrbwalkingMode.Combo);
            var lasthitParent = new OrbwalkingParent("LastHit", orbwalkerModule.Orbwalker, Orbwalking.OrbwalkingMode.LastHit);
            var mixedParent = new OrbwalkingParent("Mixed", orbwalkerModule.Orbwalker, Orbwalking.OrbwalkingMode.Mixed);
            var laneClearParent = new OrbwalkingParent( "LaneClear", orbwalkerModule.Orbwalker, Orbwalking.OrbwalkingMode.LaneClear);

            var wallLogicProvider = new WallLogicProviderModule();
            var astart = new AStarModule<AStarNode, AStarEdge<AStarNode>>();

            var yasuoQ = new YasuoQ();
            var yasuoW = new YasuoW();
            var yasuoE = new YasuoE(wallLogicProvider);
            var yasuoR = new YasuoR();
            var yasuoPassive = new YasuoPassive();

            var spellsModule = new SpellParent();
            spellsModule.Add(
                new SpellChild[]
                    {
                        yasuoQ,
                        yasuoW,
                        yasuoE,
                        yasuoR,
                        yasuoPassive
                    });

            coreParent.Add(new Base[] { spellsModule, castManagerModule, logicProviderParent });

            logicProviderParent.Add(new Base[] { wallLogicProvider });

            comboParent.Add(
                new Base[]
                    {
                        new NonDashingQ1Module(spellsModule, null)
                            .Guardian(new SpellMustBeReady(spellsModule, SpellSlot.Q))
                            .Guardian(new PlayerMustNotHaveBuff(yasuoQ.ChargedBuffName))
                            .Guardian(new AutoMustNotBeCancelled())
                            .Guardian(new PlayerMustNotBeBlinded())
                            .Guardian(new PlayerMustNotBeDashing()),
                        new NonDashingQ2Module(spellsModule, null)
                            .Guardian(new SpellMustBeReady(spellsModule, SpellSlot.Q))
                            .Guardian(new PlayerMustHaveBuff(yasuoQ.ChargedBuffName))
                            .Guardian(new PlayerMustNotBeDashing()),
                        new W(spellsModule, null)
                            .Guardian(new SpellMustBeReady(spellsModule, SpellSlot.W)),
                        new E(spellsModule, null, null)
                            .Guardian(new SpellMustBeReady(spellsModule, SpellSlot.E))
                            .Guardian(new AutoMustNotBeCancelled()),
                        new R(spellsModule, null)
                            .Guardian(new SpellMustBeReady(spellsModule, SpellSlot.R))
                    });

            foreach (var child in comboParent.Children)
            {
                var orbwalkingChild = child.Key as OrbwalkingChild;

                if (orbwalkingChild != null)
                {
                    orbwalkingChild.CastManager = castManagerModule;
                }
            }

            laneClearParent.Add(new Base[] { });

            mixedParent.Add(new Base[] { });

            lasthitParent.Add(new Base[] { });

            superParent.Add(
                new Base[]
                    {
                        orbwalkerModule, coreParent, comboParent, laneClearParent, mixedParent, lasthitParent,
                        logicProviderParent,
                    });

            superParent.Load();
        }

        #endregion
    }
}