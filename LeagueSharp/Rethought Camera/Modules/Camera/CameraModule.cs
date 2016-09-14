namespace Rethought_Camera.Modules.Camera
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Switches;
    using RethoughtLib.PriorityQuequeV2;

    using SharpDX;

    using Math = RethoughtLib.Utility.Math;

    #endregion

    // TODO CLEANUP
    internal class CameraModule : ChildBase
    {
        #region Fields

        /// <summary>
        ///     The queque
        /// </summary>
        private readonly PriorityQueue<int, Action> queque = new PriorityQueue<int, Action>();

        /// <summary>
        ///     The action limiter
        /// </summary>
        public bool ActionLimiter { get; set; }

        /// <summary>
        ///     The maximum zoom
        /// </summary>
        private float maxZoom;

        /// <summary>
        ///     The zoom
        /// </summary>
        private float zoom;

        /// <summary>
        ///     The zoom hack
        /// </summary>
        private bool zoomHack;

        protected override void SetSwitch()
        {
            this.Switch = new UnreversibleSwitch(this.Menu);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the maximum zoom.
        /// </summary>
        /// <value>
        ///     The maximum zoom.
        /// </value>
        public float MaxZoom
        {
            get
            {
                return this.maxZoom;
            }
            set
            {
                if (!this.Switch.Enabled)
                {
                    return;
                }

                this.maxZoom = value;

                Camera.MaxZoom = this.maxZoom;
            }
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Camera Module";

        /// <summary>
        ///     Gets or sets the position.
        /// </summary>
        /// <value>
        ///     The position.
        /// </value>
        public Vector3 Position { get; set; }

        /// <summary>
        ///     Gets or sets the zoom.
        /// </summary>
        /// <value>
        ///     The zoom.
        /// </value>
        public float Zoom
        {
            get
            {
                return this.zoom;
            }
            set
            {
                if (!this.Switch.Enabled)
                {
                    return;
                }

                this.zoom = value;

                Camera.Zoom = this.zoom;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [zoom hack].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [zoom hack]; otherwise, <c>false</c>.
        /// </value>
        public bool ZoomHack
        {
            get
            {
                return this.zoomHack;
            }
            set
            {
                this.zoomHack = value;

                Camera.ExtendedZoom = this.zoomHack;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Applies a force on the camera position.
        /// </summary>
        /// <param name="position">The new endposition of the camera</param>
        /// <param name="priority">The priority.</param>
        public void ApplyForce(Vector3 position, int priority)
        {
            this.Position = Camera.Position;

            var newVec = Math.VectorBetween(this.Position, position);

            Action action = () => { this.Position += newVec; };

            this.queque.Enqueue(priority, action);
        }

        /// <summary>
        ///     Moves the camera.
        /// </summary>
        public void Reset()
        {
            this.Position = Vector3.Zero;
            this.ActionLimiter = false;
        }

        /// <summary>
        ///     Sets the position. Overwrites Forces!
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="priority">The priority.</param>
        public void SetPosition(Vector3 position, int priority)
        {
            Action action = () => { this.Position = position; };

            this.ActionLimiter = true;

            this.queque.Enqueue(priority, action);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="featureBaseEventArgs"></param>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(new MenuItem("noinfos", "Seems like here are no things here yet ¯\\_(ツ)_/¯"));

            this.Position = Camera.Position;

            Game.OnUpdate += this.OnUpdate;
        }

        /// <summary>
        ///     Raises the <see cref="E:OnUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnUpdate(EventArgs args)
        {
            while (this.queque.Count > 0)
            {
                this.queque.Dequeue().Invoke();

                if (!this.ActionLimiter)
                {
                    continue;
                }

                this.queque.Clear();
                break;
            }

            if (this.Position != Vector3.Zero)
            {
                Camera.Position = this.Position;
            }

            this.Reset();
        }

        #endregion
    }
}