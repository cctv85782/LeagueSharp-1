﻿namespace Rethought_Fiora.Champions.Fiora.Modules.LogicProvider
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;

    #endregion

    internal class WallLogicProvider : ChildBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Wall Logic Provider";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the first wall point.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="step">The step.</param>
        /// <returns></returns>
        public Vector3 GetFirstWallPoint(Vector3 start, Vector3 end, int step = 1)
        {
            if (start.IsValid() && end.IsValid())
            {
                var distance = start.Distance(end);
                for (var i = 0; i < distance; i = i + step)
                {
                    var newPoint = start.Extend(end, i);

                    if (NavMesh.GetCollisionFlags(newPoint) == CollisionFlags.Wall || newPoint.IsWall())
                    {
                        return newPoint;
                    }
                }
            }

            return Vector3.Zero;
        }

        /// <summary>
        ///     Gets the width of the wall.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="step">The step.</param>
        /// <returns></returns>
        public float GetWallWidth(Vector3 start, Vector3 direction, int step = 1)
        {
            var thickness = 0f;

            if (!start.IsValid() || !direction.IsValid())
            {
                return thickness;
            }

            for (var i = 0; i < this.Menu.Item("MaxWallWidth").GetValue<Slider>().Value; i = i + step)
            {
                if (NavMesh.GetCollisionFlags(start.Extend(direction, i)) == CollisionFlags.Wall
                    || start.Extend(direction, i).IsWall())
                {
                    thickness += step;
                }
                else
                {
                    return thickness;
                }
            }

            return thickness;
        }

        /// <summary>
        ///     Determines whether dash is wall-jump over a specified unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="dashRange">The dash range.</param>
        public bool IsWallDash(Obj_AI_Base unit, float dashRange)
        {
            return this.IsWallDash(unit.ServerPosition, dashRange);
        }

        /// <summary>
        ///     Determines whether dash is wall-jump.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="dashRange">The dash range.</param>
        public bool IsWallDash(Vector3 direction, float dashRange)
        {
            var dashEndPos = ObjectManager.Player.Position.Extend(direction, dashRange);
            var firstWallPoint = this.GetFirstWallPoint(ObjectManager.Player.Position, dashEndPos);

            if (firstWallPoint.Equals(Vector3.Zero))
            {
                // No Wall
                return false;
            }

            if (dashEndPos.IsWall())
                // End Position is in Wall
            {
                var wallWidth = this.GetWallWidth(firstWallPoint, dashEndPos);

                if (wallWidth > this.Menu.Item("MinWallWidth").GetValue<Slider>().Value
                    && wallWidth - firstWallPoint.Distance(dashEndPos) < wallWidth * 0.6f)
                {
                    return true;
                }
            }
            else
            // End Position is not a Wall
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            this.Menu.AddItem(
                new MenuItem("MinWallWidth", "Min. Wall-width before a wall is recognized").SetValue(
                    new Slider(50, 0, 400)));

            this.Menu.AddItem(
                new MenuItem("MaxWallWidth", "Max. Wall-width after a wall is recognized").SetValue(
                    new Slider(500, 400, 1000)));
        }

        #endregion
    }
}