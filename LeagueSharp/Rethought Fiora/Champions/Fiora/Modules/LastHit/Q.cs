namespace Rethought_Fiora.Champions.Fiora.Modules.LastHit
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.Utility;

    using Rethought_Fiora.Champions.Fiora.Modules.Core;
    using Rethought_Fiora.Champions.Fiora.Modules.Core.SpellsModule;
    using Rethought_Fiora.Champions.Fiora.Modules.LogicProvider;

    using SharpDX;

    using Math = RethoughtLib.Utility.Math;

    #endregion

    internal class Q : ChildBase
    {
        #region Fields

        internal Orbwalking.OrbwalkingMode RequiredOrbwalkerMode { get; set; }

        internal WallLogicProvider WallDashLogicProvider { get; set; }

        #endregion

        #region Constructors and Destructors

        public Q(Orbwalking.OrbwalkingMode requiredOrbwalkerMode, WallLogicProvider wallDashLogicProvider)
        {
            this.RequiredOrbwalkerMode = requiredOrbwalkerMode;
            this.WallDashLogicProvider = wallDashLogicProvider;
        }

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
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets the minions.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Obj_AI_Base> GetMinions()
        {
            var minions = MinionManager.GetMinions(SpellsModule.Spells[SpellSlot.Q].Range);

            return minions.Where(x => x.Health <= QLogicProviderModule.GetDamage(x));
        }

        /// <summary>
        ///     Triggers on OnUpdate
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void GameOnOnUpdate(EventArgs args)
        {
            if (OrbwalkerModule.Orbwalker.ActiveMode != this.RequiredOrbwalkerMode
                || !SpellsModule.Spells[SpellSlot.Q].IsReady())
            {
                return;
            }

            var minions = GetMinions().ToList();

            if (!minions.Any())
            {
                return;
            }

            var minion = minions.MinOrDefault(x => x.Health);

            var vectorCircle = Math.CircleToVector2Segments(400, 60);

            vectorCircle.MoveTo(ObjectManager.Player.ServerPosition.To2D());

            var realVectors = new List<Vector2>();

            foreach (var v in vectorCircle)
            {
                var vector = this.WallDashLogicProvider.GetFirstWallPoint(
                    ObjectManager.Player.ServerPosition,
                    v.To3D(),
                    5);

                if (vector.Distance(minion.ServerPosition) > 100 || vector.Distance(minion.ServerPosition) < 10
                    || minions.Any(x => x.Distance(vector) <= vector.Distance(minion.ServerPosition)))
                {
                    return;
                }

                realVectors.Add(vector.To2D());
            }

            SpellsModule.Spells[SpellSlot.Q].Cast(
                realVectors.MaxOrDefault(x => x.Distance(HeroManager.Enemies.MaxOrDefault(y => y.Distance(x)))));
        }

        #endregion
    }
}