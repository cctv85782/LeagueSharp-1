//     File:  Championship Series/Championship Yasuo/Q.cs
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
//     Created: 04.10.2016 3:53 PM
//     Last Edited: 04.10.2016 7:37 PM

namespace Championship_Yasuo.Modules.Spells
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;
    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.FeatureSystem.Switches;
    using System;

    #endregion

    /// <summary>
    ///     All spell relevant logic
    /// </summary>
    internal class Q : SpellChild
    {
        #region Constants

        /// <summary>
        ///     The buff name of the stack instance.
        /// </summary>
        public const string ChargedBuffName = "YasuoQ3W";

        #endregion

        #region Enums

        /// <summary>
        ///     The spell state of Q
        /// </summary>
        public enum SpellState
        {
            /// <summary>
            ///     The next cast will unleash a whirlwind
            /// </summary>
            Charged,

            /// <summary>
            ///     The next cast will be circular
            /// </summary>
            Circular,

            /// <summary>
            ///     The next cast will be circular and a whirlwind
            /// </summary>
            CircularCharged,

            /// <summary>
            ///     The next cast will be no special cast
            /// </summary>
            None
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the state of the active spell.
        /// </summary>
        /// <value>
        ///     The state of the active spell.
        /// </value>
        public SpellState ActiveSpellState { get; private set; } = SpellState.None;

        /// <summary>
        ///     Gets the charged buff instance.
        /// </summary>
        /// <value>
        ///     The charged buff instance.
        /// </value>
        public BuffInstance ChargedBuffInstance { get; private set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = nameof(Q);

        /// <summary>
        ///     Gets or sets the spell.
        /// </summary>
        /// <value>
        ///     The spell.
        /// </value>
        public override Spell Spell { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the amount of stacks.
        /// </summary>
        /// <returns>the sum of all stacks</returns>
        public int GetBuffCount()
        {
            // todo Q.GetBuffCount() 
            throw new NotImplementedException();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="Base.FeatureBaseEventArgs" /> instance containing the event data.</param>
        protected override void OnLoad(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnLoad(sender, eventArgs);

            this.Spell = new Spell(SpellSlot.Q, true);

            Obj_AI_Base.OnBuffAdd += this.OnBuffAdd;
            Obj_AI_Base.OnBuffRemove -= this.OnBuffRemove;
            CustomEvents.Unit.OnDash += this.OnDash;
        }

        /// <summary>
        ///     Sets the switch. Switch becomes not reversible and won't be shown.
        /// </summary>
        protected override void SetSwitch() => this.Switch = new UnreversibleSwitch(this.Menu);

        /// <summary>
        ///     Called when [buff add].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Obj_AI_BaseBuffAddEventArgs" /> instance containing the event data.</param>
        private void OnBuffAdd(Obj_AI_Base sender, Obj_AI_BaseBuffAddEventArgs args)
        {
            if (!sender.IsMe) return;

            if (args.Buff.Name == ChargedBuffName)
            {
                this.ChargedBuffInstance = args.Buff;

                this.ActiveSpellState = SpellState.Charged;
            }
        }

        /// <summary>
        ///     Called when [buff remove].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Obj_AI_BaseBuffRemoveEventArgs" /> instance containing the event data.</param>
        private void OnBuffRemove(Obj_AI_Base sender, Obj_AI_BaseBuffRemoveEventArgs args)
        {
            if (!sender.IsMe) return;


            if (args.Buff.Name == ChargedBuffName)
            {
                this.ChargedBuffInstance = null;

                this.ActiveSpellState = SpellState.None;
            }
        }

        /// <summary>
        ///     Called when [dash].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        private void OnDash(Obj_AI_Base sender, Dash.DashItem args)
        {
            if (!sender.IsMe) return;

            var spellE = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E);

            if (spellE.IsReady()) return;

            switch (this.ActiveSpellState)
            {
                case SpellState.Charged:
                    this.ActiveSpellState = SpellState.CircularCharged;
                    break;
                case SpellState.None:
                    this.ActiveSpellState = SpellState.Circular;
                    break;
                case SpellState.Circular:
                    throw new InvalidOperationException();
                case SpellState.CircularCharged:
                    throw new InvalidOperationException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}