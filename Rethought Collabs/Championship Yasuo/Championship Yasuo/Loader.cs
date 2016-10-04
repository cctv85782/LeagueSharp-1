//     File:  Championship Series/Championship Yasuo/Loader.cs
//     Copyright (C) 2016 Rethought and SupportExTraGoZ
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
//     Created: 04.10.2016 3:10 PM
//     Last Edited: 04.10.2016 7:37 PM

namespace Championship_Yasuo
{
    #region Using Directives

    using Championship_Yasuo.Modules.Spells;
    using LeagueSharp;
    using LeagueSharp.Common;
    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.DamageCalculator;
    using RethoughtLib.Drawings;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Guardians;
    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.Orbwalker.Implementations;
    using System.Collections.Generic;
    using System.Drawing;
    using Color = SharpDX.Color;

    #endregion

    /// <summary>
    ///     The loader
    /// </summary>
    public class Loader : LoadableBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        /// <value>
        ///     The display name.
        /// </value>
        public override string DisplayName { get; set; } = "Championship Yasuo";

        /// <summary>
        ///     Gets or sets the name of the internal.
        /// </summary>
        /// <value>
        ///     The name of the internal.
        /// </value>
        public override string InternalName { get; set; } = "rethought_collab_yasuo";

        /// <summary>
        ///     Gets or sets the tags.
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public override IEnumerable<string> Tags { get; set; } = new[] { "Yasuo" };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public override void Load()
        {
            var root = new SuperParent(this.DisplayName);

            var orbwalkerModule = new OrbwalkerModule();
            orbwalkerModule.Load();

            this.RootMenu = root.Menu;

            // initializes the spells and adds them to a SpellParent, will also pre-load them.
            var spellParent = new Parent("Spells");

            var yasuoQ = new Q();
            var yasuoW = new W();
            var yasuoE = new E();
            var yasuoR = new R();

            spellParent.Add(new List<Base>() { yasuoQ, yasuoW, yasuoE, yasuoR });

            spellParent.Load(); // pre-loading


            var damageCalculatorParent = new DamageCalculatorParent();

            damageCalculatorParent.Add(yasuoQ);
            damageCalculatorParent.Add(yasuoW);
            damageCalculatorParent.Add(yasuoE);
            damageCalculatorParent.Add(yasuoR);


            var drawingRangeParent = new Parent("Range");

            drawingRangeParent.Add(
                new List<Base>()
                    {
                        new RangeDrawingChild(yasuoQ.Spell, yasuoQ.Name),
                        new RangeDrawingChild(yasuoW.Spell, yasuoW.Name, true),
                        new RangeDrawingChild(yasuoE.Spell, yasuoE.Name),
                        new RangeDrawingChild(yasuoR.Spell, yasuoR.Name)
                    });


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

            // assigns the specific spell to an OrbwalkingParent, so you don't need to take care of Orbwalker.ActiveMode for example.
            comboParent.Add(
                new List<Base>()
                    {
                        new Modules.Combo.Q()
                            .Guardian(new SpellMustBeReady(SpellSlot.Q))
                            .Guardian(new PlayerMustNotBeWindingUp()),
                        new Modules.Combo.W()
                            .Guardian(new SpellMustBeReady(SpellSlot.W)),
                        new Modules.Combo.E()
                            .Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustBeDashing()),
                        new Modules.Combo.R()
                            .Guardian(new SpellMustBeReady(SpellSlot.R))
                    });

            laneClearParent.Add(
                new List<Base>()
                    {
                        new Modules.LaneClear.Q()
                            .Guardian(new SpellMustBeReady(SpellSlot.Q)),
                        new Modules.LaneClear.W()
                            .Guardian(new SpellMustBeReady(SpellSlot.W)),
                        new Modules.LaneClear.E()
                            .Guardian(new PlayerMustNotBeWindingUp())
                            .Guardian(new PlayerMustBeDashing())
                            .Guardian(new SpellMustBeReady(SpellSlot.E)),
                    });

            lastHitParent.Add(
                new List<Base>
                    {
                        new Modules.LastHit.Q()
                            .Guardian(new SpellMustBeReady(SpellSlot.Q))
                            .Guardian(new PlayerMustNotBeWindingUp()),
                        new Modules.LastHit.E()
                            .Guardian(new SpellMustBeReady(SpellSlot.E))
                            .Guardian(new PlayerMustBeDashing())
                            .Guardian(new PlayerMustNotBeWindingUp()),
                    });

            mixedParent.Add(
                new List<Base>()
                    {
                        new Modules.Mixed.Q()
                            .Guardian(new SpellMustBeReady(SpellSlot.Q)),
                        new Modules.Mixed.W()
                            .Guardian(new SpellMustBeReady(SpellSlot.W)),
                        new Modules.Mixed.E()
                            .Guardian(new SpellMustBeReady(SpellSlot.E)),
                    });

            var drawingParent = new Parent("Drawings");


            root.Add(
                new List<Base>()
                    {
                        orbwalkerModule,
                        spellParent,
                        damageCalculatorParent,
                        comboParent,
                        laneClearParent,
                        lastHitParent,
                        mixedParent,
                        drawingParent
                    });

            root.Load();

            root.Menu.Style = FontStyle.Bold;
            root.Menu.Color = new Color(2, 196, 234); // google #02a9ea
        }

        #endregion
    }
}