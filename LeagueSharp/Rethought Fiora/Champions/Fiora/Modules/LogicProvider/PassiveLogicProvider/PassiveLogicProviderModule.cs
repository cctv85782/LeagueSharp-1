namespace Rethought_Fiora.Champions.Fiora.Modules.LogicProvider.PassiveLogicProvider
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Fiora.Champions.Fiora.Modules.LogicProvider.PassiveLogicProvider.PassiveInstances;

    #endregion

    internal class PassiveLogicProviderModule : ChildBase
    {
        #region Enums

        internal enum PassiveType
        {
            Ult,

            Pre,

            TimeOut,

            Normal
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Passive Logic Provider";

        #endregion

        #region Properties

        internal List<PassiveInstance> PassiveList { get; } = new List<PassiveInstance>();

        #endregion

        #region Public Methods and Operators

        public PassiveInstance GetPassiveInstance(Obj_AI_Hero hero)
        {
            return this.PassiveList.FirstOrDefault(x => x.Owner.NetworkId == hero.NetworkId);
        }

        public bool HasFioraUlt(Obj_AI_Hero unit)
        {
            return this.PassiveList.Any(x => x.Owner.NetworkId == unit.NetworkId);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            GameObject.OnCreate -= this.GameObjectOnOnCreate;
            GameObject.OnDelete -= this.GameObjectOnOnDelete;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            GameObject.OnCreate += this.GameObjectOnOnCreate;
            GameObject.OnDelete += this.GameObjectOnOnDelete;
        }

        /// <summary>
        ///     Triggers on GameObject.OnCreate
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void GameObjectOnOnCreate(GameObject sender, EventArgs args)
        {
            var emitter = sender as Obj_GeneralParticleEmitter;

            if (emitter == null || !this.IsFioraPassive(emitter) || emitter.Team == ObjectManager.Player.Team)
            {
                return;
            }

            var target = HeroManager.Enemies.MinOrDefault(x => x.Position.Distance(emitter.Position));

            if (target != null)
            {
                this.PassiveList.Add(new PassiveInstance(sender, target));
            }
        }

        /// <summary>
        ///     Triggers on GameObject.OnDelete
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void GameObjectOnOnDelete(GameObject sender, EventArgs args)
        {
            var emitter = sender as Obj_GeneralParticleEmitter;

            if (emitter == null || !this.IsFioraPassive(emitter) || emitter.Team == ObjectManager.Player.Team
                || this.PassiveList.All(x => x.NetworkId.Equals(emitter.NetworkId)))
            {
                return;
            }

            this.PassiveList.RemoveAll(x => x.NetworkId.Equals(emitter.NetworkId));
        }

        /// <summary>
        ///     Determines whether GameObject is Fiora passive.
        /// </summary>
        /// <param name="gameObject">The gameObject.</param>
        /// <returns></returns>
        private bool IsFioraPassive(GameObject gameObject)
            =>
                gameObject.Name.Contains("Fiora_Base_R_Mark") || gameObject.Name.Contains("Fiora_Base_R")
                || gameObject.Name.Contains("Timeout") || gameObject.Name.Contains("Fiora_Base_Passive");

        #endregion
    }
}