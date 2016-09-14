﻿namespace Rethought_Irelia.IreliaV1
{
    #region Using Directives

    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Guardians;
    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.Orbwalker.Implementations;

    using Rethought_Irelia.IreliaV1.Combo;
    using Rethought_Irelia.IreliaV1.DamageCalculator;
    using Rethought_Irelia.IreliaV1.GridGenerator;
    using Rethought_Irelia.IreliaV1.Pathfinder;
    using Rethought_Irelia.IreliaV1.Spells;

    #endregion

    /// <summary>
    ///     Class which represents the loader for <c>Yorick</c>
    /// </summary>
    /// <seealso cref="RethoughtLib.Bootstraps.Abstract_Classes.LoadableBase" />
    public class Loader : LoadableBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name that will get displayed.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string DisplayName { get; set; } = "Rethought Irelia";

        /// <summary>
        ///     Gets or sets the internal name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string InternalName { get; set; } = "Rethought_Irelia_Version1";

        /// <summary>
        ///     Gets or sets the tags.
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public override IEnumerable<string> Tags { get; set; } = new[] { "Irelia_1" };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public override void Load()
        {
            var superParent = new SuperParent(this.DisplayName);

            var orbwalkerModule = new OrbwalkerModule();
            orbwalkerModule.Load();

            var graphGeneratorModule = new GraphGeneratorModule(null);

            var pathfinderModule = new PathfinderModule();

            var damageCalculator = new DamageCalculatorParent();

            //var ireliaP = new IreliaP();
            var ireliaQ = new IreliaQ(graphGeneratorModule, pathfinderModule);
            graphGeneratorModule.IreliaQ = ireliaQ;
            var ireliaW = new IreliaW();
            var ireliaE = new IreliaE();
            var ireliaR = new IreliaR();

            damageCalculator.Add(ireliaQ);
            damageCalculator.Add(ireliaW);
            damageCalculator.Add(ireliaE);
            damageCalculator.Add(ireliaR);

            graphGeneratorModule.IreliaQ = ireliaQ;

            var spellParent = new SpellParent.SpellParent();

            spellParent.Add(new List<Base> { ireliaQ, ireliaW, ireliaE, ireliaR, });

            var comboParent = new OrbwalkingParent(
                "Combo",
                orbwalkerModule.OrbwalkerInstance,
                Orbwalking.OrbwalkingMode.Combo);

            var laneClearParent = new OrbwalkingParent(
                "LaneClear",
                orbwalkerModule.OrbwalkerInstance,
                Orbwalking.OrbwalkingMode.LaneClear);

            var lastHitParent = new OrbwalkingParent(
                "LastHit",
                orbwalkerModule.OrbwalkerInstance,
                Orbwalking.OrbwalkingMode.LastHit,
                Orbwalking.OrbwalkingMode.Mixed,
                Orbwalking.OrbwalkingMode.LaneClear);

            var mixedParent = new OrbwalkingParent(
                "Mixed",
                orbwalkerModule.OrbwalkerInstance,
                Orbwalking.OrbwalkingMode.Mixed);

            comboParent.Add(
                new List<Base>()
                    {
                        new Q(ireliaQ).Guardian(new SpellMustBeReady(SpellSlot.Q)),
                        new W(ireliaW, ireliaQ).Guardian(new SpellMustBeReady(SpellSlot.W)),
                        new E(ireliaE).Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustNotBeWindingUp()),
                        new R(ireliaR, damageCalculator).Guardian(new SpellMustBeReady(SpellSlot.R))
                            .Guardian(new PlayerMustNotBeWindingUp())
                    });

            laneClearParent.Add(
                new List<Base>()
                    {
                        new LaneClear.W(ireliaW).Guardian(new SpellMustBeReady(SpellSlot.W)),
                        //new LaneClear.E(ireliaE).Guardian(new SpellMustBeReady(SpellSlot.E))
                        //    .Guardian(new PlayerMustNotBeWindingUp())
                    });

            lastHitParent.Add(
                new List<Base>()
                    {
                        new LastHit.Q(ireliaQ).Guardian(new SpellMustBeReady(SpellSlot.Q)),
                        //new LastHit.W(ireliaW).Guardian(new SpellMustBeReady(SpellSlot.W)),
                        //new LastHit.E(ireliaE).Guardian(new SpellMustBeReady(SpellSlot.E))
                        //    .Guardian(new PlayerMustNotBeWindingUp())
                    });

            mixedParent.Add(
                new List<Base>()
                    {
                        //new Mixed.Q(ireliaQ).Guardian(new SpellMustBeReady(SpellSlot.Q)),
                        //new Mixed.W(ireliaW).Guardian(new SpellMustBeReady(SpellSlot.W)),
                        new Mixed.E(ireliaE).Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustNotBeWindingUp())
                    });

            superParent.Add(
                new List<Base>()
                    {
                        orbwalkerModule,
                        spellParent,
                        damageCalculator,
                        pathfinderModule,
                        graphGeneratorModule,
                        comboParent,
                        laneClearParent,
                        lastHitParent,
                        mixedParent,
                    });

            superParent.Load();
        }

        #endregion
    }
}