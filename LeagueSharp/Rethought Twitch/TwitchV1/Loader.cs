namespace Rethought_Twitch.TwitchV1
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Guardians;
    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.Orbwalker.Implementations;

    using Rethought_Twitch.TwitchV1.Combo;
    using Rethought_Twitch.TwitchV1.DamageCalculator;
    using Rethought_Twitch.TwitchV1.Drawings;
    using Rethought_Twitch.TwitchV1.Spells;

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
        public override string DisplayName { get; set; } = "Rethought Twitch";

        /// <summary>
        ///     Gets or sets the internal name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string InternalName { get; set; } = "Rethought_Twitch_The_Plague_Rat";

        /// <summary>
        ///     Gets or sets the tags.
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public override IEnumerable<string> Tags { get; set; } = new[] { "Twitch" };

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

            //var twitchP = new IreliaP();
            var twitchQ = new TwitchQ();
            var twitchW = new TwitchW();
            var twitchE = new TwitchE();
            var twitchR = new TwitchR();

            damageCalculator.Add(new AutoAttacks());
            damageCalculator.Add(twitchQ);
            damageCalculator.Add(twitchW);
            damageCalculator.Add(twitchE);
            damageCalculator.Add(twitchR);

            var spellParent = new SpellParent.SpellParent();

            spellParent.Add(new List<Base> { twitchQ, twitchW, twitchE, twitchR, });
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
                        new Q(twitchQ, damageCalculator).Guardian(new SpellMustBeReady(SpellSlot.Q)),
                        new W(twitchW, twitchQ).Guardian(new SpellMustBeReady(SpellSlot.W))
                            .Guardian(new PlayerMustNotBeWindingUp()),
                        new E(twitchE).Guardian(new SpellMustBeReady(SpellSlot.E)),
                        new R(twitchR, twitchQ, damageCalculator).Guardian(new SpellMustBeReady(SpellSlot.R))
                            .Guardian(new PlayerMustNotBeWindingUp() { Negated = true })
                    });

            laneClearParent.Add(
                new List<Base>()
                    {
                        // TODO: Add Q
                        new LaneClear.W(twitchW).Guardian(new SpellMustBeReady(SpellSlot.W)),
                        new LaneClear.E(twitchE).Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustNotBeWindingUp())
                    });

            lastHitParent.Add(new List<Base>() { new LastHit.E(twitchE).Guardian(new SpellMustBeReady(SpellSlot.E)) });

            mixedParent.Add(
                new List<Base>()
                    {
                        new Q(twitchQ, damageCalculator).Guardian(new SpellMustBeReady(SpellSlot.Q)),
                        new W(twitchW, twitchQ).Guardian(new SpellMustBeReady(SpellSlot.W)),
                        new Mixed.E(twitchE).Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustNotBeWindingUp())
                    });

            var module = new DamageDrawingParent("Damage Drawings");

            module.Add(
                new List<IDamageDrawing>()
                    {
                        new DamageDrawingChild(twitchQ.Spell, "Q", twitchQ.GetDamage)
                            {
                                Color =
                                    twitchQ
                                    .Color
                            }
                    });

            var spellRangeParent = new Parent("Ranges");

            spellRangeParent.Add(
                new List<Base>()
                    {
                        new RangeDrawingChild(twitchQ.Spell, "Q"),
                        new RangeDrawingChild(twitchE.Spell, "E"),
                        new RangeDrawingChild(twitchW.Spell, "W"),
                        new RangeDrawingChild(twitchR.Spell, "R", true),
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