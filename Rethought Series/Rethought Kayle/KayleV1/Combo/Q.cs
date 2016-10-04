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

namespace Rethought_Kayle.KayleV1.Combo
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.DamageCalculator;
    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Kayle.KayleV1.Spells;

    #endregion

    internal class Q : OrbwalkingChild
    {
        #region Fields

        private readonly IDamageCalculator damageCalculator;

        /// <summary>
        ///     Gets or sets the last rites logic provider.
        /// </summary>
        /// <value>
        ///     The logic provider.
        /// </value>
        private readonly KayleQ kayleQ;

        /// <summary>
        ///     The target
        /// </summary>
        private Obj_AI_Hero target;

        #endregion

        #region Constructors and Destructors

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Q" /> class.
        /// </summary>
        /// <param name="kayleQ">The Q logic</param>
        /// <param name="damageCalculator">The damage calculator</param>
        public Q(KayleQ kayleQ, IDamageCalculator damageCalculator)
        {
            this.kayleQ = kayleQ;
            this.damageCalculator = damageCalculator;
        }

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Q";

        /// <summary>
        ///     Gets or sets the spell priority.
        /// </summary>
        /// <value>
        ///     The spell priority.
        /// </value>
        public int SpellPriority { get; set; } = 2;

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Game.OnUpdate -= this.OnGameUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Game.OnUpdate += this.OnGameUpdate;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnLoad(sender, eventArgs);

            this.Menu.AddItem(new MenuItem(this.Path + ".slowdown", "Slow down").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Path + ".burst", "Burst").SetValue(true));
        }

        private void Execute()
        {
            this.ActionManager.Queue.Enqueue(this.SpellPriority, () => this.kayleQ.Spell.Cast(this.target));
        }

        /// <summary>
        ///     Logic to burst enemies.
        /// </summary>
        private void LogicBurst()
        {
            if (!this.Menu.Item(this.Path + ".burst").GetValue<bool>()) return;

            if (this.damageCalculator.GetDamage(this.target) >= this.target.Health) this.Execute();
        }

        /// <summary>
        ///     Logic to finish enemies with Q.
        /// </summary>
        private void LogicFinisher()
        {
            if (this.kayleQ.GetDamage(this.target) > this.target.Health) this.Execute();
        }

        /// <summary>
        ///     Logic to slow enemies with Q.
        /// </summary>
        private void LogicSlowDown()
        {
            if (!this.Menu.Item(this.Path + ".slowdown").GetValue<bool>()) return;

            if ((this.target.MoveSpeed > ObjectManager.Player.MoveSpeed)
                && (Prediction.GetPrediction(this.target, 100f)
                        .UnitPosition.Distance(ObjectManager.Player.ServerPosition)
                    > ObjectManager.Player.Distance(this.target))) this.Execute();
        }

        /// <summary>
        ///     Raises the <see cref="E:GameUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnGameUpdate(EventArgs args)
        {
            if (!this.CheckGuardians()) return;

            this.target = TargetSelector.GetTarget(this.kayleQ.Spell.Range, this.kayleQ.Spell.DamageType);

            if (this.target == null) return;

            this.LogicFinisher();

            this.LogicSlowDown();

            this.LogicBurst();
        }

        #endregion
    }
}