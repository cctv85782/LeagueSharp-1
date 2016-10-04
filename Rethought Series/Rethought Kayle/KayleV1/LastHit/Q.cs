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

namespace Rethought_Kayle.KayleV1.LastHit
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Kayle.KayleV1.Spells;

    #endregion

    internal class Q : OrbwalkingChild
    {
        #region Fields

        /// <summary>
        ///     The irelia q
        /// </summary>
        private readonly KayleQ kayleQ;

        #endregion

        #region Constructors and Destructors

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Q" /> class.
        /// </summary>
        /// <param name="kayleQ">The Q logic</param>
        public Q(KayleQ kayleQ)
        {
            this.kayleQ = kayleQ;
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

            this.Menu.AddItem(
                new MenuItem(this.Path + ".clearmode", "Clear Mode: ").SetValue(
                    new StringList(new[] { "Unkillable", "Always" })));
        }

        private void LogicLaneLastHit()
        {
            var units =
                MinionManager.GetMinions(this.kayleQ.Spell.Range, MinionTypes.All, MinionTeam.NotAlly)
                    .Where(x => this.kayleQ.GetDamage(x) >= x.Health)
                    .ToList();

            Obj_AI_Base unit = null;

            switch (this.Menu.Item(this.Path + ".clearmode").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    unit =
                        units.FirstOrDefault(
                            x =>
                                (x.Distance(ObjectManager.Player) >= ObjectManager.Player.AttackRange)
                                && (x.HealthPercent <= 20));
                    break;
                case 1:
                    unit = units.MinOrDefault(x => x.Health);
                    break;
            }

            if (unit != null) this.kayleQ.Spell.Cast(unit);
        }

        /// <summary>
        ///     Raises the <see cref="E:GameUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnGameUpdate(EventArgs args)
        {
            if (!this.CheckGuardians()) return;

            this.LogicLaneLastHit();
        }

        #endregion
    }
}