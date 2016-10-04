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
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Kayle.KayleV1.Spells;

    #endregion

    internal class W : OrbwalkingChild
    {
        #region Fields

        /// <summary>
        ///     The irelia q
        /// </summary>
        private readonly KayleW kayleW;

        #endregion

        #region Constructors and Destructors

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="W" /> class.
        /// </summary>
        /// <param name="kayleW">The kayle w.</param>
        public W(KayleW kayleW)
        {
            this.kayleW = kayleW;
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

            Game.OnUpdate -= this.OnUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Game.OnUpdate += this.OnUpdate;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnLoad(sender, eventArgs);

            this.Menu.AddItem(
                new MenuItem(this.Path + ".prioritymode", "Priority").SetValue(
                    new StringList(new[] { "Priority", "Current Health" })));

            this.Menu.AddItem(new MenuItem("disclaimer", "Min Health % (per ally)"));

            foreach (var ally in HeroManager.Allies)
            {
                if (ally.ChampionName == ObjectManager.Player.ChampionName)
                {
                    this.Menu.AddItem(
                        new MenuItem(this.Path + ".minhealthally." + ally.ChampionName, "Yourself").SetValue(
                            new Slider(40, 0, 100)));
                    continue;
                }

                this.Menu.AddItem(
                    new MenuItem(this.Path + ".minhealthally." + ally.ChampionName, ally.ChampionName).SetValue(
                        new Slider(20, 0, 100)));
            }

            this.Menu.AddItem(new MenuItem(this.Path + ".speedup", "Speed Up (per ally)"));

            foreach (var ally in HeroManager.Allies)
            {
                if (ally.ChampionName == ObjectManager.Player.ChampionName)
                {
                    this.Menu.AddItem(
                        new MenuItem(this.Path + ".speedup." + ally.ChampionName, "Yourself").SetValue(true));
                    continue;
                }

                this.Menu.AddItem(
                    new MenuItem(this.Path + ".speedup." + ally.ChampionName, ally.ChampionName).SetValue(true));
            }
        }

        /// <summary>
        ///     The logic to heal allies
        /// </summary>
        private void LogicHeal()
        {
            var targetList =
                HeroManager.Allies.Where(
                        x =>
                            x.Health
                            <= this.Menu.Item(this.Path + ".minhealthally." + x.ChampionName).GetValue<Slider>().Value)
                    .ToList();

            if (!targetList.Any()) return;

            Obj_AI_Base target = null;

            switch (this.Menu.Item(this.Path + ".prioritymode").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    target = targetList.MaxOrDefault(TargetSelector.GetPriority);
                    break;
                case 1:
                    target = targetList.MinOrDefault(x => x.Health);
                    break;
            }

            if (target == null) return;

            this.kayleW.Spell.Cast(target);
        }

        /// <summary>
        ///     The logic to speedup allies
        /// </summary>
        private void LogicSpeedup()
        {
            var targetList =
                HeroManager.Allies.Where(x => this.Menu.Item(this.Path + ".speedup." + x.ChampionName).GetValue<bool>())
                    .ToList();

            if (!targetList.Any()) return;

            Obj_AI_Base target = null;

            var minDist = float.MaxValue;

            foreach (var hero in targetList)
                foreach (var enemy in HeroManager.Enemies)
                {
                    var distance = hero.Distance(enemy);

                    if (!(distance <= minDist) || !(distance <= 900)) continue;

                    target = hero;
                    minDist = distance;
                }

            if (target == null) return;

            this.kayleW.Spell.Cast(target);
        }

        // TODO: Think about HealthPrediction
        /// <summary>
        ///     Called when the game updates
        /// </summary>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnUpdate(EventArgs eventArgs)
        {
            if (!this.CheckGuardians()) return;

            this.LogicSpeedup();

            this.LogicHeal();
        }

        #endregion
    }
}