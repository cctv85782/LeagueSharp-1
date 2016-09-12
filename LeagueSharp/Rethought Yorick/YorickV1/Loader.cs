namespace Rethought_Yorick.YorickV1
{
    #region Using Directives

    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Guardians;
    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Yorick.YorickV1.Combo;
    using Rethought_Yorick.YorickV1.Modules;
    using Rethought_Yorick.YorickV1.Spells;

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
        public override string DisplayName { get; set; } = "Rethought Yorick";

        /// <summary>
        ///     Gets or sets the internal name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string InternalName { get; set; } = "Rethought_Yorick";

        /// <summary>
        ///     Gets or sets the tags.
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public override IEnumerable<string> Tags { get; set; } = new[] { "Yorick" };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public override void Load()
        {
            var superParent = new SuperParent(this.DisplayName);

            var orbwalkingModule = new OrbwalkingModule();

            var passiveObserver = new PassiveObserver();

            var lastRites = new LastRites();
            var darkProcession = new DarkProcession();
            var mourningMist = new MourningMist();
            var eulogyOfTheIsles = new EulogyOfTheIsles();

            var spellParent = new SpellParent.SpellParent();

            spellParent.Add(new List<Base> { lastRites, darkProcession, mourningMist, eulogyOfTheIsles, passiveObserver });

            var comboParent = new OrbwalkingParent(
                "Combo",
                orbwalkingModule.OrbwalkerInstance,
                Orbwalking.OrbwalkingMode.Combo);

            var laneClearParent = new OrbwalkingParent(
                "LaneClear",
                orbwalkingModule.OrbwalkerInstance,
                Orbwalking.OrbwalkingMode.LaneClear);

            var lastHitParent = new OrbwalkingParent(
                "LastHit",
                orbwalkingModule.OrbwalkerInstance,
                Orbwalking.OrbwalkingMode.LastHit,
                Orbwalking.OrbwalkingMode.Mixed,
                Orbwalking.OrbwalkingMode.LaneClear);

            var mixedParent = new OrbwalkingParent(
                "Mixed",
                orbwalkingModule.OrbwalkerInstance,
                Orbwalking.OrbwalkingMode.Mixed);

            comboParent.Add(
                new List<Base>()
                    {
                        new Q(passiveObserver, lastRites).Guardian(new SpellMustBeReady(SpellSlot.Q)),
                        new W(darkProcession).Guardian(new SpellMustBeReady(SpellSlot.W)),
                        new E(mourningMist).Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustNotBeWindingUp())
                    });

            laneClearParent.Add(
                new List<Base>()
                    {
                        new LaneClear.Q(passiveObserver, lastRites).Guardian(
                            new SpellMustBeReady(SpellSlot.Q)),
                        new LaneClear.W(darkProcession).Guardian(new SpellMustBeReady(SpellSlot.W)),
                        new LaneClear.E(mourningMist).Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustNotBeWindingUp())
                    });

            lastHitParent.Add(
                new List<Base>()
                    {
                        new LastHit.Q(passiveObserver, lastRites).Guardian(new SpellMustBeReady(SpellSlot.Q)),
                        new LastHit.W(darkProcession).Guardian(new SpellMustBeReady(SpellSlot.W)),
                        new LastHit.E(mourningMist).Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustNotBeWindingUp())
                    });

            mixedParent.Add(
                new List<Base>()
                    {
                        new Mixed.Q(passiveObserver, lastRites).Guardian(new SpellMustBeReady(SpellSlot.Q)),
                        new Mixed.W(darkProcession).Guardian(new SpellMustBeReady(SpellSlot.W)),
                        new Mixed.E(mourningMist).Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustNotBeWindingUp())
                    });

            superParent.Add(new List<Base>()
                                {
                                    orbwalkingModule,
                                    spellParent,
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