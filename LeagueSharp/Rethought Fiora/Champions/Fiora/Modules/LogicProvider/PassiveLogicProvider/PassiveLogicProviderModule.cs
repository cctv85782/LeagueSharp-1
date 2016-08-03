﻿namespace Rethought_Fiora.Champions.Fiora.Modules.LogicProvider.PassiveLogicProvider
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
        #region Fields

        internal static List<PassiveInstance> PassiveList { get; } = new List<PassiveInstance>();

        #endregion

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

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            GameObject.OnCreate -= GameObjectOnOnCreate;
            GameObject.OnDelete -= GameObjectOnOnDelete;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            GameObject.OnCreate += GameObjectOnOnCreate;
            GameObject.OnDelete += GameObjectOnOnDelete;
        }

        /// <summary>
        ///     Determines whether GameObject is Fiora passive.
        /// </summary>
        /// <param name="gameObject">The gameObject.</param>
        /// <returns></returns>
        private static bool IsFioraPassive(GameObject gameObject)
            =>
                gameObject.Name.Contains("Fiora_Base_R_Mark") || gameObject.Name.Contains("Fiora_Base_R")
                || gameObject.Name.Contains("Timeout") || gameObject.Name.Contains("Fiora_Base_Passive");

        /// <summary>
        ///     Triggers on GameObject.OnCreate
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void GameObjectOnOnCreate(GameObject sender, EventArgs args)
        {
            var emitter = sender as Obj_GeneralParticleEmitter;

            if (emitter == null || !IsFioraPassive(emitter) || emitter.Team == ObjectManager.Player.Team)
            {
                return;
            }

            var target = HeroManager.Enemies.MinOrDefault(x => x.Position.Distance(emitter.Position));

            if (target != null)
            {
                PassiveList.Add(new PassiveInstance(sender, target));
            }
        }

        /// <summary>
        ///     Triggers on GameObject.OnDelete
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private static void GameObjectOnOnDelete(GameObject sender, EventArgs args)
        {
            var emitter = sender as Obj_GeneralParticleEmitter;

            if (emitter == null || !IsFioraPassive(emitter) || emitter.Team == ObjectManager.Player.Team
                || PassiveList.All(x => x.NetworkId.Equals(emitter.NetworkId)))
            {
                return;
            }

            PassiveList.RemoveAll(x => x.NetworkId.Equals(emitter.NetworkId));
        }

        #endregion


    }
}