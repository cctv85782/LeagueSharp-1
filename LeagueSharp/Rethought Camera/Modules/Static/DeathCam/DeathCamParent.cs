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

        public DeathCamParent(CameraModule cameraModule)
        {
            this.cameraModule = cameraModule;
        }

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

            foreach (var child in children)
            {
                child.MaxPriority = children.Count;
            }

            base.OnLoad(sender, featureBaseEventArgs);
        }

        private void GameOnOnUpdate(EventArgs args)
        {
            if (!ObjectManager.Player.IsDead)
            {
                return;
            }

            foreach (var child in this.Children.OfType<DeathCamModule>())
            {
                this.cameraModule.SetPosition(child.GetPosition(), child.Priority);
            }

            this.cameraModule.ActionLimiter = true;
        }

        #endregion
    }
}