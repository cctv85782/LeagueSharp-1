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

namespace Rethought_Camera.Modules.Static.DeathCam
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Camera.Modules.Camera;

    #endregion

    internal class DeathCamParent : ParentBase
    {
        #region Fields

        private readonly CameraModule cameraModule;

        #endregion

        #region Constructors and Destructors

        #region Constructors

        public DeathCamParent(CameraModule cameraModule)
        {
            this.cameraModule = cameraModule;
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
        public override string Name { get; set; } = "DeathCam";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnDisable(sender, featureBaseEventArgs);
            Game.OnUpdate -= this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnEnable(sender, featureBaseEventArgs);
            Game.OnUpdate += this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            var children = this.Children.OfType<DeathCamModule>().ToList();

            foreach (var child in children) child.MaxPriority = children.Count;

            base.OnLoad(sender, featureBaseEventArgs);
        }

        private void GameOnOnUpdate(EventArgs args)
        {
            if (!ObjectManager.Player.IsDead) return;

            foreach (var child in this.Children.OfType<DeathCamModule>()) this.cameraModule.SetPosition(child.GetPosition(), child.Priority);

            this.cameraModule.ActionLimiter = true;
        }

        #endregion
    }
}