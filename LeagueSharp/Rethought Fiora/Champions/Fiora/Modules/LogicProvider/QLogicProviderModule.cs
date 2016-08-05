namespace Rethought_Fiora.Champions.Fiora.Modules.LogicProvider
{
    #region Using Directives

    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.LogicProvider;
    using RethoughtLib.Utility;

    using Rethought_Fiora.Champions.Fiora.Modules.Core.SpellsModule;
    using Rethought_Fiora.Champions.Fiora.Modules.LogicProvider.PassiveLogicProvider.PassiveInstances;

    using SharpDX;

    #endregion

    internal class QLogicProviderModule : ChildBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Q Logic Provider";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the cast position respecting the fiora passive.
        /// </summary>
        /// <param name="passiveInstance">The passive instance.</param>
        /// <returns></returns>
        public static Vector3 GetCastPosition(PassiveInstance passiveInstance)
        {
            if (passiveInstance.Polygon == null)
            {
                return Vector3.Zero;
            }

            var center = passiveInstance.Polygon.Center.To3D();

            var pred = SpellsModule.Spells[SpellSlot.Q].GetPrediction(
                passiveInstance.Owner,
                collisionable: new[] { CollisionableObjects.Walls });

            if (pred == null)
            {
                return center;
            }

            var predictedPolygon = passiveInstance.Polygon;

            predictedPolygon.MovePolygone(pred.UnitPosition.To2D());

            var vectors = Math.CircleToVector2Segments(ObjectManager.Player.Distance(pred.UnitPosition), 60);

            vectors.MoveTo(ObjectManager.Player.ServerPosition.To2D());

            foreach (var directionVector in vectors.ToList())
            {
                var rawEndVector = ObjectManager.Player.ServerPosition.Extend(directionVector.To3D(), 400);

                var wallLogicProvider = new WallDashLogicProvider();

                var realVector =
                    wallLogicProvider.GetFirstWallPoint(ObjectManager.Player.ServerPosition, rawEndVector, 5).To2D();

                if (predictedPolygon.IsOutside(realVector)
                    || rawEndVector.Distance(passiveInstance.Owner.ServerPosition) < 50
                    || rawEndVector.Distance(passiveInstance.Owner.ServerPosition) > 100)
                {
                    vectors.Remove(realVector);
                }
            }

            return vectors.To3D().MinOrDefault(x => x.Distance(passiveInstance.Owner.ServerPosition));
        }

        #endregion
    }
}