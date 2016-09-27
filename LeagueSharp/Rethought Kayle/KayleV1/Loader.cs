namespace Rethought_Kayle.KayleV1
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

    using Rethought_Kayle.KayleV1.Combo;
    using Rethought_Kayle.KayleV1.DamageCalculator;
    using Rethought_Kayle.KayleV1.Drawings;
    using Rethought_Kayle.KayleV1.Spells;

    using Color = SharpDX.Color;

    #endregion

    /// <summary>
    ///     Class which represents the loader for <c>Twitch</c>
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
        public override string DisplayName { get; set; } = "Rethought Kayle";

        /// <summary>
        ///     Gets or sets the internal name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string InternalName { get; set; } = "Rethought_Kayle_The_Judicator";

        /// <summary>
        ///     Gets or sets the tags.
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public override IEnumerable<string> Tags { get; set; } = new[] { "Kayle" };

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

            var damageCalculator = new DamageCalculatorParent();

            var kayleQ = new KayleQ();
            var kayleW = new KayleW();
            var kayleE = new KayleE();
            var kayleR = new KayleR();

            damageCalculator.Add(new AutoAttacks());
            damageCalculator.Add(kayleQ);
            damageCalculator.Add(kayleW);
            damageCalculator.Add(kayleE);
            damageCalculator.Add(kayleR);

            var spellParent = new SpellParent.SpellParent();

            spellParent.Add(new List<Base> { kayleQ, kayleW, kayleE, kayleR, });
            spellParent.Load();

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

            var drawingParent = new Parent("Drawings");

            comboParent.Add(
                new List<Base>()
                    {
                        new Q(kayleQ, damageCalculator).Guardian(new SpellMustBeReady(SpellSlot.Q)),
                        new W(kayleW, kayleQ).Guardian(new SpellMustBeReady(SpellSlot.W))
                            .Guardian(new PlayerMustNotBeWindingUp()),
                        new E(kayleE).Guardian(new SpellMustBeReady(SpellSlot.E)),
                        new R(kayleR, kayleQ, damageCalculator).Guardian(new SpellMustBeReady(SpellSlot.R))
                            .Guardian(new PlayerMustNotBeWindingUp() { Negated = true })
                    });

            laneClearParent.Add(
                new List<Base>()
                    {
                        // TODO: Add Q
                        new LaneClear.W(kayleW).Guardian(new SpellMustBeReady(SpellSlot.W)),
                        new LaneClear.E(kayleE).Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustNotBeWindingUp())
                    });

            lastHitParent.Add(new List<Base>() { new LastHit.E(kayleE).Guardian(new SpellMustBeReady(SpellSlot.E)) });

            mixedParent.Add(
                new List<Base>()
                    {
                        new Q(kayleQ, damageCalculator).Guardian(new SpellMustBeReady(SpellSlot.Q)),
                        new W(kayleW, kayleQ).Guardian(new SpellMustBeReady(SpellSlot.W)),
                        new Mixed.E(kayleE).Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustNotBeWindingUp())
                    });

            var module = new DamageDrawingParent("Damage Drawings");

            module.Add(
                new List<IDamageDrawing>()
                    {
                        new DamageDrawingChild(kayleQ.Spell, "Q", kayleQ.GetDamage)
                            {
                                Color =
                                    kayleQ
                                    .Color
                            }
                    });

            var spellRangeParent = new Parent("Ranges");

            spellRangeParent.Add(
                new List<Base>()
                    {
                        new RangeDrawingChild(kayleQ.Spell, "Q"),
                        new RangeDrawingChild(kayleE.Spell, "E"),
                        new RangeDrawingChild(kayleW.Spell, "W"),
                        new RangeDrawingChild(kayleR.Spell, "R", true),
                    });

            drawingParent.Add(new List<Base>() { spellRangeParent, module });

            superParent.Add(
                new List<Base>()
                    {
                        orbwalkerModule,
                        spellParent,
                        damageCalculator,
                        comboParent,
                        laneClearParent,
                        lastHitParent,
                        mixedParent,
                        drawingParent
                    });

            superParent.Load();

            superParent.Menu.Style = FontStyle.Bold;
            superParent.Menu.Color = Color.LightSeaGreen;
        }

        #endregion
    }
}