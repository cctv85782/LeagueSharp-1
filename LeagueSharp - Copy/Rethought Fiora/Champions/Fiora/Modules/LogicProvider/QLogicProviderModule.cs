namespace Rethought_Fiora.Champions.Fiora.Modules.LogicProvider
{
    #region Using Directives

    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;
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

            foreach (var vector in vectors.ToList())
            {
                if (predictedPolygon.IsOutside(vector)
                    || vector.Distance(passiveInstance.Owner.ServerPosition) < 50
                    || vector.Distance(passiveInstance.Owner.ServerPosition) > 100)
                {
                    vectors.Remove(vector);
                }
            }

            return vectors.To3D().MinOrDefault(x => x.Distance(passiveInstance.Owner.ServerPosition));
        }

        #endregion
    }
}