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

    internal class MoveToMouseModule : ParentBase
    {
        #region Fields

        /// <summary>
        ///     The camera module
        /// </summary>
        private readonly CameraModule cameraModule;

        private readonly bool dynamicActive = false;

        /// <summary>
        ///     The dynamic camera parent
        /// </summary>
        private readonly DynamicCameraParent dynamicCameraParent;

        /// <summary>
        ///     The executing
        /// </summary>
        private bool executing;

        #endregion

        #region Constructors and Destructors

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MoveToMouseModule" /> class.
        /// </summary>
        /// <param name="cameraModule">The camera module.</param>
        /// <param name="dynamicCameraParent">The dynamic camera parent.</param>
        /// <param name="transitionsModule">The transitions module.</param>
        public MoveToMouseModule(
            CameraModule cameraModule,
            DynamicCameraParent dynamicCameraParent,
            TransitionsModule transitionsModule = null)
        {
            this.TransitionsModule = transitionsModule ?? new TransitionsModule("Transition");

            this.Add(this.TransitionsModule);

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
        public override string Name { get; set; } = "Move to Mouse";

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
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(new MenuItem("disabledynamic", "Disable Dynamic Camera on Move").SetValue(true));

            this.Menu.AddItem(new MenuItem("keybind", "Keybind: ").SetValue(new KeyBind('A', KeyBindType.Press)))
                .ValueChanged += (o, args) =>
                {
                    if (args.GetNewValue<KeyBind>().Active) this.Execute(args);

                    if (!this.Menu.Item("disabledynamic").GetValue<bool>()) return;

                    this.dynamicCameraParent.Disable(this);
                };
        }

        /// <summary>
        ///     Send a move command to the camera module in order to move it.
        /// </summary>
        /// <param name="args">The <see cref="OnValueChangeEventArgs" /> instance containing the event data.</param>
        private void Execute(OnValueChangeEventArgs args)
        {
            if (this.executing || !args.GetNewValue<KeyBind>().Active) return;

            this.executing = true;

            this.TransitionsModule.ActiveTransitionBase.Start(Camera.Position, Game.CursorPos);

            Game.OnUpdate += this.OnUpdate;
        }

        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnUpdate(EventArgs args)
        {
            if (this.TransitionsModule.ActiveTransitionBase.Moving)
            {
                this.cameraModule.SetPosition(this.TransitionsModule.ActiveTransitionBase.GetPosition(), 0);
            }
            else
            {
                if (this.dynamicActive) this.dynamicCameraParent.Enable(this);

                this.executing = false;
                Game.OnUpdate -= this.OnUpdate;
            }
        }

        #endregion
    }
}