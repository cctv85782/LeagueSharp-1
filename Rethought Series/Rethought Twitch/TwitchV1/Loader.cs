//     Copyright (C) 2016 Rethought
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
//     Created: 04.10.2016 1:05 PM
//     Last Edited: 04.10.2016 1:44 PM

namespace Rethought_Twitch.TwitchV1
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.DamageCalculator;
    using RethoughtLib.Drawings;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Guardians;
    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.FeatureSystem.Implementations.SpellParent;
    using RethoughtLib.Orbwalker.Implementations;

    using Rethought_Twitch.TwitchV1.Combo;
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

            //var twitchP = new TwitchP();
            var twitchQ = new TwitchQ();
            var twitchW = new TwitchW();
            var twitchE = new TwitchE();
            var twitchR = new TwitchR();

            var aa = new AutoAttacks();

            damageCalculator.Add(new AutoAttacks());
            damageCalculator.Add(twitchQ);
            damageCalculator.Add(twitchW);
            damageCalculator.Add(twitchE);
            damageCalculator.Add(twitchR);

            var spellParent = new SpellParent();

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
                new List<Base>
                    {
                        new Q(twitchQ, damageCalculator).Guardian(new SpellMustBeReady(SpellSlot.Q)),
                        new W(twitchW, twitchQ).Guardian(new SpellMustBeReady(SpellSlot.W))
                            .Guardian(new PlayerMustNotBeWindingUp()),
                        new E(twitchE).Guardian(new SpellMustBeReady(SpellSlot.E)).Guardian(new PlayerMustNotBeWindingUp()),
                        new R(twitchR, damageCalculator).Guardian(new SpellMustBeReady(SpellSlot.R))
                            .Guardian(new PlayerMustNotBeWindingUp { Negated = true })
                    });

            laneClearParent.Add(
                new List<Base>
                    {
                        // TODO: Add Q
                        new LaneClear.W(twitchW).Guardian(new SpellMustBeReady(SpellSlot.W)),
                        new LaneClear.E(twitchE).Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustNotBeWindingUp())
                    });

            lastHitParent.Add(new List<Base> { new LastHit.E(twitchE).Guardian(new SpellMustBeReady(SpellSlot.E)) });

            mixedParent.Add(
                new List<Base>
                    {
                        new Q(twitchQ, damageCalculator).Guardian(new SpellMustBeReady(SpellSlot.Q)),
                        new W(twitchW, twitchQ).Guardian(new SpellMustBeReady(SpellSlot.W)),
                        new Mixed.E(twitchE).Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustNotBeWindingUp())
                    });

            const byte DEFAULT_ALPHA = 50 * 255 / 100;

            var damageDrawingParent = new DamageDrawingParent("Damage Drawings");

            damageDrawingParent.Add(
                new List<IDamageDrawing>
                    {
                        new DamageDrawingChild("Auto-Attacks", aa.GetDamage)
                            {
                                Color = new Color(40, 175, 175) { A = DEFAULT_ALPHA },
                            },
                        new DamageDrawingChild("E", twitchE.GetDamage)
                            {
                                Color = new Color(255, 210, 95) { A = DEFAULT_ALPHA }
                            },
                        new DamageDrawingChild("R", twitchR.GetDamage)
                            {
                                Color = new Color(240, 150, 75) { A = DEFAULT_ALPHA }
                            }
                    });

            var spellRangeParent = new Parent("Ranges");

            spellRangeParent.Add(
                new List<Base>
                    {
                        new RangeDrawingChild(twitchQ.Spell, "Q"),
                        new RangeDrawingChild(twitchE.Spell, "E"),
                        new RangeDrawingChild(twitchW.Spell, "W"),
                        new RangeDrawingChild(twitchR.Spell, "R", true),
                    });

            drawingParent.Add(new List<Base> { spellRangeParent, damageDrawingParent });

            superParent.Add(
                new List<Base>
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