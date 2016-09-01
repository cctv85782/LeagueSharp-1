namespace Rethought_Camera.Modules.Dynamic
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;

    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Switches;

    using Rethought_Camera.Modules.Camera;
    using Rethought_Camera.Modules.Transitions;

    using SharpDX;

    #endregion

    internal class DynamicCameraParent : ParentBase
    {
        #region Fields

        /// <summary>
        ///     The camera module
        /// </summary>
        private readonly CameraModule cameraModule;

        #endregion

        #region Constructors and Destructors

        public DynamicCameraParent(CameraModule camera)
        {
            this.cameraModule = camera;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Dynamic Camera";

        /// <summary>
        ///     Gets or sets the transitions module.
        /// </summary>
        /// <value>
        ///     The transitions module.
        /// </value>
        public TransitionsModule TransitionsModule { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);
            Game.OnUpdate -= this.OnUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);
            Game.OnUpdate += this.OnUpdate;
        }

        protected override void SetSwitch()
        {
            this.Switch = new KeybindSwitch(this.Menu, "Enabled", 'H', this);
        }

        private void OnUpdate(EventArgs args)
        {
            if (MenuGUI.IsChatOpen || MenuGUI.IsShopOpen)
            {
                return;
            }

            foreach (var child in this.Children.Keys.OfType<DynamicCameraChild>())
            {
                if (!child.Switch.Enabled)
                {
                    continue;
                }

                var focus = child.GetPosition();

                if (focus == Vector3.Zero)
                {
                    continue;
                }

                this.cameraModule.ApplyForce(focus, 1);
            }
        }

        #endregion
    }
}