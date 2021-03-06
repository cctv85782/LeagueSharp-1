﻿//     Copyright (C) 2016 Rethought
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

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Twitch.TwitchV1.Spells;

    #endregion

    internal class E : OrbwalkingChild
    {
        #region Fields

        /// <summary>
        ///     The Twitch e
        /// </summary>
        private readonly TwitchE twitchE;

        /// <summary>
        ///     The target
        /// </summary>
        private Obj_AI_Hero target;

        #endregion

        #region Constructors and Destructors

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="E" /> class.
        /// </summary>
        /// <param name="twitchE">The Twitch e.</param>
        public E(TwitchE twitchE)
        {
            this.twitchE = twitchE;
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
        public override string Name { get; set; } = "E";

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

            this.Menu.AddItem(new MenuItem(this.Path + ".killable", "Cast when killable").SetValue(true));

            this.Menu.AddItem(
                new MenuItem(this.Path + ".killable.hitmorethan", "    > min. enemies hit").SetValue(
                    new Slider(1, 0, HeroManager.Enemies.Count)));

            this.Menu.AddItem(
                new MenuItem(this.Path + ".killable.killmorethan", "    > min. enemies that will die").SetValue(
                    new Slider(1, 0, HeroManager.Enemies.Count)));

            this.Menu.AddItem(
                new MenuItem(this.Path + ".stacksmorethan", "Cast when enemy has more than X stacks").SetValue(
                    new Slider(6, 6, 0)));

            this.Menu.AddItem(
                new MenuItem(this.Path + ".stacksmorethan.outofaarange", "    > only if out of aa-range").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Path + ".beforedeath", "Cast if you are about to die").SetValue(true));
        }

        /// <summary>
        ///     Raises the <see cref="E:GameUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnGameUpdate(EventArgs args)
        {
            if (!this.CheckGuardians()) return;

            this.target = TargetSelector.GetTarget(this.twitchE.Spell.Range, this.twitchE.Spell.DamageType);

            if (this.target == null) return;
        }

        #endregion
    }
}