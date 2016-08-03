namespace Rethought_Fiora.Champions.Fiora.Modules.LogicProvider
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Fiora.Champions.Fiora.Modules.Core;
    using Rethought_Fiora.Champions.Fiora.Modules.Core.SpellsModule;
    using Rethought_Fiora.Champions.Fiora.Modules.Core.TargetSelectorModule;
    using Rethought_Fiora.Champions.Fiora.Modules.LogicProvider.PassiveLogicProvider.PassiveInstances;

    using SharpDX;

    #endregion

    internal class QLogicProviderModule : ChildBase, ITargetRequester, ISpellsRequester
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

        public Vector3 GetCastPosition(PassiveInstance passiveInstance)
        {
            if (passiveInstance.Polygon == null)
            {
                return Vector3.Zero;
            }

            var center = passiveInstance.Polygon.Center.To3D();

            var pred = this.Spells[SpellSlot.Q].GetPrediction(
                passiveInstance.Owner,
                collisionable: new [] { CollisionableObjects.Walls });

            if (pred == null)
            {
                return center;
            }

            var vectors = new List<Vector3>();

            for (var i = 0; i < center.Distance(pred.UnitPosition); i++)
            {
                var vector = center.Extend(pred.UnitPosition, i);

                if (vector.Distance(passiveInstance.Owner.ServerPosition) < 50
                    || vector.Distance(ObjectManager.Player.ServerPosition) < 100
                    || passiveInstance.Polygon.IsOutside(vector.To2D()))
                {
                    continue;
                }

                vectors.Add(vector);
            }

            return vectors.MinOrDefault(x => x.Distance(pred.UnitPosition));
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);
        }

        #endregion

        /// <summary>
        /// Occurs when [spell requested].
        /// </summary>
        public event EventHandler SpellRequested;

        /// <summary>
        /// Gets or sets the spells.
        /// </summary>
        /// <value>
        /// The spells.
        /// </value>
        public Dictionary<SpellSlot, Spell> Spells { get; set; }

        protected virtual void OnSpellRequested()
        {
            this.SpellRequested?.Invoke(this, EventArgs.Empty);
        }

        public Obj_AI_Hero Target { get; set; }

        public ITargetRetrieveMethod TargetRetrieveMethod { get; set; }
    }
}