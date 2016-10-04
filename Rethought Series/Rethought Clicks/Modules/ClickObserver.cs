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

namespace Rethought_Clicks.Modules
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;

    using Color = System.Drawing.Color;

    #endregion

    internal class ClickObserver : ChildBase
    {
        #region Fields

        private readonly Dictionary<GameObjectIssueOrderEventArgs, float> positions =
            new Dictionary<GameObjectIssueOrderEventArgs, float>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Click Observer";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="Base.FeatureBaseEventArgs" /> instance containing the event data.</param>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Obj_AI_Base.OnIssueOrder -= this.OnIssueOrder;
            Drawing.OnDraw -= this.OnDraw;
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="Base.FeatureBaseEventArgs" /> instance containing the event data.</param>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Obj_AI_Base.OnIssueOrder += this.OnIssueOrder;
            Drawing.OnDraw += this.OnDraw;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="featureBaseEventArgs">The <see cref="Base.FeatureBaseEventArgs" /> instance containing the event data.</param>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);
        }

        /// <summary>
        ///     Raises the <see cref="E:Draw" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void OnDraw(EventArgs args)
        {
            foreach (var keyValuePair in this.positions.ToList())
            {
                if (Game.Time - keyValuePair.Value >= 0.5f) this.positions.Remove(keyValuePair.Key);

                switch (keyValuePair.Key.Order)
                {
                    case GameObjectOrder.HoldPosition:
                        Render.Circle.DrawCircle(keyValuePair.Key.TargetPosition, 20, Color.Yellow, 8);
                        break;

                    case GameObjectOrder.MoveTo:
                        Render.Circle.DrawCircle(keyValuePair.Key.TargetPosition, 20, Color.Blue, 8);
                        break;

                    case GameObjectOrder.AttackUnit:
                        Render.Circle.DrawCircle(
                            keyValuePair.Key.TargetPosition,
                            keyValuePair.Key.Target.BoundingRadius,
                            Color.Red,
                            8);
                        break;

                    case GameObjectOrder.AutoAttackPet:
                        Render.Circle.DrawCircle(keyValuePair.Key.TargetPosition, 20, Color.Gray, 8);
                        break;

                    case GameObjectOrder.AutoAttack:
                        Render.Circle.DrawCircle(keyValuePair.Key.TargetPosition, 20, Color.Red, 8);
                        break;

                    case GameObjectOrder.MovePet:
                        Render.Circle.DrawCircle(keyValuePair.Key.TargetPosition, 20, Color.Gray, 8);
                        break;

                    case GameObjectOrder.AttackTo:
                        Render.Circle.DrawCircle(keyValuePair.Key.TargetPosition, 20, Color.OrangeRed, 8);
                        break;

                    case GameObjectOrder.Stop:
                        Render.Circle.DrawCircle(keyValuePair.Key.TargetPosition, 20, Color.Yellow, 8);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///     Called when [issue order].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectIssueOrderEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void OnIssueOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            if (!sender.IsMe || (args.TargetPosition == Vector3.Zero)) return;

            this.positions.Add(args, Game.Time);
        }

        #endregion
    }
}