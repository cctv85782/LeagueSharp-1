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

namespace Rethought_Camera.Modules.Static
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Camera.Modules.Camera;
    using Rethought_Camera.Modules.Dynamic;
    using Rethought_Camera.Modules.Transitions;

    #endregion

    internal class QuickSwitchModule : ParentBase
    {
        #region Fields

        /// <summary>
        ///     The camera module
        /// </summary>
        private readonly CameraModule cameraModule;

        private readonly DynamicCameraParent dynamicCameraParent;

        /// <summary>
        ///     Whether moving camera
        /// </summary>
        private bool executing;

        #endregion

        #region Constructors and Destructors

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="QuickSwitchModule" /> class.
        /// </summary>
        /// <param name="cameraModule">The camera module.</param>
        /// <param name="dynamicCameraParent">The dynamic camera parent</param>
        /// <param name="transitionsModule">The transitions module, can be null and a default will be attached</param>
        public QuickSwitchModule(
            CameraModule cameraModule,
            DynamicCameraParent dynamicCameraParent,
            TransitionsModule transitionsModule = null)
        {
            if (transitionsModule == null)
            {
                this.TransitionsModule = new TransitionsModule("Transitions");
                this.Add(this.TransitionsModule);
            }
            else
            {
                this.TransitionsModule = transitionsModule;
            }

            this.cameraModule = cameraModule;
            this.dynamicCameraParent = dynamicCameraParent;
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
        public override string Name { get; set; } = "QuickSwitch";

        /// <summary>
        ///     Gets or sets the transitions module.
        /// </summary>
        /// <value>
        ///     The transitions module.
        /// </value>
        public TransitionsModule TransitionsModule { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="featureBaseEventArgs"></param>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            var enemies = this.Menu.AddSubMenu(new Menu("Enemies", "enemies"));
            var allies = this.Menu.AddSubMenu(new Menu("Allies", "allies"));

            var index = 1;
            var index2 = 6;

            foreach (var hero in HeroManager.AllHeroes)
            {
                index++;
                if (hero.IsEnemy)
                    enemies.AddItem(
                            new MenuItem(hero.ChampionName, hero.ChampionName).SetValue(
                                new KeyBind(index.ToString().ToCharArray()[0], KeyBindType.Press))).ValueChanged +=
                        (o, args) => { this.Execute(args, hero); };
                else if (hero.IsAlly)
                    allies.AddItem(
                            new MenuItem(hero.ChampionName, hero.ChampionName).SetValue(
                                new KeyBind(index2.ToString().ToCharArray()[0], KeyBindType.Press))).ValueChanged +=
                        (o, args) => { this.Execute(args, hero); };
            }

            if (enemies.Items.Count == 0) enemies.AddItem(new MenuItem("nothingfound1", "No Enemies found. :("));
            if (allies.Items.Count == 0) allies.AddItem(new MenuItem("nothingfound2", "No Allies found. :("));

            this.Menu.AddItem(new MenuItem("disabledynamic", "Disable Dynamic Camera on Move").SetValue(true));
        }

        /// <summary>
        ///     Send a move command to the camera module in order to move it.
        /// </summary>
        /// <param name="args">The <see cref="OnValueChangeEventArgs" /> instance containing the event data.</param>
        /// <param name="hero">The object.</param>
        private void Execute(OnValueChangeEventArgs args, GameObject hero)
        {
            if (this.executing || !this.Switch.Enabled) return;

            this.executing = true;

            this.TransitionsModule.ActiveTransitionBase.Start(Camera.Position, hero.Position, Utils.TickCount);

            Game.OnUpdate += this.GameOnOnUpdate;

            if (!this.Menu.Item("disabledynamic").GetValue<bool>()) return;

            this.dynamicCameraParent.Disable(this);
        }

        /// <summary>
        ///     OnGameUpdate
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void GameOnOnUpdate(EventArgs args)
        {
            if (this.TransitionsModule.ActiveTransitionBase.Moving)
            {
                this.cameraModule.SetPosition(this.TransitionsModule.ActiveTransitionBase.GetPosition(), 0);
            }
            else
            {
                this.executing = false;
                Game.OnUpdate -= this.GameOnOnUpdate;
            }
        }

        #endregion
    }
}