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

namespace Rethought_Kayle.KayleV1
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.DamageCalculator;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Guardians;
    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.Orbwalker.Implementations;

    using Rethought_Kayle.KayleV1.Combo;
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
            damageCalculator.Add(kayleE);

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
                new List<Base>
                    {
                        new Q(kayleQ, damageCalculator).Guardian(new SpellMustBeReady(SpellSlot.Q))
                            .Guardian(new PlayerMustNotBeWindingUp()),
                        new W(kayleW).Guardian(new SpellMustBeReady(SpellSlot.W)).Guardian(new PlayerMustNotBeWindingUp()),
                        new E(kayleE).Guardian(new SpellMustBeReady(SpellSlot.E)),
                    });

            laneClearParent.Add(
                new List<Base>
                    {
                        new LaneClear.E(kayleE).Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustNotBeWindingUp())
                    });

            lastHitParent.Add(new List<Base> { new LastHit.E(kayleE).Guardian(new SpellMustBeReady(SpellSlot.E)) });

            mixedParent.Add(
                new List<Base>
                    {
                        new Q(kayleQ, damageCalculator).Guardian(new SpellMustBeReady(SpellSlot.Q))
                            .Guardian(new PlayerMustNotBeWindingUp()),
                        new Mixed.E(kayleE).Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustNotBeWindingUp())
                    });

            var module = new DamageDrawingParent("Damage Drawings");

            module.Add(
                new List<IDamageDrawing>
                    {
                        new DamageDrawingChild(kayleQ.Spell, "Q", kayleQ.GetDamage),
                        new DamageDrawingChild(kayleE.Spell, "E", kayleE.GetDamage)
                    });

            var spellRangeParent = new Parent("Ranges");

            spellRangeParent.Add(
                new List<Base>
                    {
                        new RangeDrawingChild(kayleQ.Spell, "Q"),
                        new RangeDrawingChild(kayleW.Spell, "W", true),
                        new RangeDrawingChild(kayleE.Spell, "E"),
                        new RangeDrawingChild(kayleR.Spell, "R", true),
                    });

            drawingParent.Add(new List<Base> { spellRangeParent, module });

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