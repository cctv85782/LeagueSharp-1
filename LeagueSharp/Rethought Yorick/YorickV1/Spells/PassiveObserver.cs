namespace Rethought_Yorick.YorickV1.Spells
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Switches;

    #endregion

    internal class PassiveObserver : ChildBase
    {
        #region Constants

        /// <summary>
        ///     The grave limit
        /// </summary>
        private const int GhoulLimit = GraveLimit;

        /// <summary>
        ///     The grave detection range
        /// </summary>
        private const int GraveDetectionRange = 1200;

        /// <summary>
        ///     The grave limit
        /// </summary>
        /// // TODO
        private const int GraveLimit = 8;

        /// <summary>
        ///     The yorick ghoul name
        /// </summary>
        /// // TODO
        private const string YorickGhoulName = "YorickGhoulName";

        /// <summary>
        ///     The yorick passive unit name
        /// </summary>
        /// // TODO
        private const string YorickGraveName = "YorickGrave";

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the ghouls.
        /// </summary>
        /// <value>
        ///     The ghouls.
        /// </value>
        public List<Obj_AI_Base> Ghouls { get; } = new List<Obj_AI_Base>(GhoulLimit);

        /// <summary>
        ///     Gets or sets the graves.
        /// </summary>
        /// <value>
        ///     The graves.
        /// </value>
        public List<GameObject> Graves { get; } = new List<GameObject>(GraveLimit);

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Passive Observer";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            GameObject.OnCreate += this.GameObjectOnOnCreate;
            GameObject.OnDelete += this.GameObjectOnOnDelete;
        }

        /// <summary>
        ///     Sets the switch.
        /// </summary>
        protected override void SetSwitch()
        {
            this.Switch = new UnreversibleSwitch(this.Menu);
        }

        /// <summary>
        ///     Determines whether the specified sender is a yorick ghoul
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        private static bool IsYorickGhoul(GameObject @object)
        {
            return @object.Name == YorickGhoulName && @object.IsMe;
        }

        /// <summary>
        ///     Determines whether the specified sender is a yorick grave.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        private static bool IsYorickGrave(GameObject @object)
        {
            return @object.Name == YorickGraveName && @object.IsMe;
        }

        /// <summary>
        ///     Triggers when a GameObject gets creates
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void GameObjectOnOnCreate(GameObject sender, EventArgs args)
        {
            if (sender.Position.Distance(ObjectManager.Player.ServerPosition) <= 1200 && IsYorickGrave(sender))
            {
                this.Graves.Add(sender);
                return;
            }

            if (IsYorickGhoul(sender))
            {
                this.Ghouls.Add((Obj_AI_Base)sender);
                return;
            }
        }

        /// <summary>
        ///     Triggers when a GameObjects gets deleted
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void GameObjectOnOnDelete(GameObject sender, EventArgs args)
        {
            if (IsYorickGhoul(sender))
            {
                this.Ghouls.Remove((Obj_AI_Base)sender);
                return;
            }

            if (IsYorickGrave(sender))
            {
                this.Graves.Remove(sender);
                return;
            }
        }

        #endregion
    }
}