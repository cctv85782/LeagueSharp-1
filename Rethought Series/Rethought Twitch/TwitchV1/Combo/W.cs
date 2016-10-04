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

namespace Rethought_Twitch.TwitchV1.Combo
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Twitch.TwitchV1.Spells;

    #endregion

    internal class W : OrbwalkingChild
    {
        #region Fields

        /// <summary>
        ///     The Twitch q
        /// </summary>
        private readonly TwitchQ twitchQ;

        /// <summary>
        ///     The Twitch w
        /// </summary>
        private readonly TwitchW twitchW;

        #endregion

        #region Constructors and Destructors

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="W" /> class.
        /// </summary>
        /// <param name="twitchW">Twitch W.</param>
        /// <param name="twitchQ">Twitch Q</param>
        public W(TwitchW twitchW, TwitchQ twitchQ)
        {
            this.twitchW = twitchW;
            this.twitchQ = twitchQ;
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
        public override string Name { get; set; } = "W";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Obj_AI_Base.OnProcessSpellCast -= this.OnProcessSpellCast;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Obj_AI_Base.OnProcessSpellCast += this.OnProcessSpellCast;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnLoad(sender, eventArgs);
        }

        /// <summary>
        ///     Called when [process spell cast].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!this.CheckGuardians()) return;

            if (!sender.IsMe) return;

            var target = args.Target as Obj_AI_Hero;

            if ((target == null) || this.twitchQ.WillReset(target)) return;

            if (args.Slot == SpellSlot.Q) this.twitchW.Spell.Cast();

            if (args.SData.ConsideredAsAutoAttack || args.SData.IsAutoAttack()) this.twitchW.Spell.Cast();
        }

        #endregion
    }
}