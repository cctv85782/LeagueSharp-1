namespace Rethought_Camera.Modules.Dynamic
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Events;
    using RethoughtLib.Extensions;
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="DynamicCameraParent" /> class.
        /// </summary>
        /// <param name="camera">The camera.</param>
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

            // TODO QOL Change SMOOTH
            this.cameraModule.SetPosition(ObjectManager.Player.Position, 0);
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            var disableOnDeath =
                this.Menu.AddItem(
                    new MenuItem("disableondeath", "Disable on death").SetValue(true)
                        .SetTooltip(
                            "When you die and already prepare for the next movement or move issue order in general it can be very annoying to be locked at the fountain."));
        }

        /// <summary>
        /// Sets the switch.
        /// </summary>
        protected override void SetSwitch()
        {
            this.Switch = new KeybindSwitch(this.Menu, "Enabled", 'H', this);
        }

        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnUpdate(EventArgs args)
        {
            if (ObjectManager.Player.IsDead && this.Menu.Item("disableondeath").GetValue<bool>())
            {
                this.Disable();
            }

            if (MenuGUI.IsChatOpen || MenuGUI.IsShopOpen)
            {
                return;
            }

            foreach (var child in this.Children.Keys.OfType<DynamicCameraChild>())
            {
                if (!this.Children[child].Item1)
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