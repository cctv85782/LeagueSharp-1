namespace Rethought_Camera.Modules.Camera
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.PriorityQuequeV2;

    using SharpDX;

    using Math = RethoughtLib.Utility.Math;

    #endregion

    internal class CameraModule : ChildBase
    {
        #region Fields

        /// <summary>
        /// The queque
        /// </summary>
        private readonly PriorityQueue<int, Action> queque = new PriorityQueue<int, Action>();

        /// <summary>
        /// The action limiter
        /// </summary>
        private bool actionLimiter = false;

        /// <summary>
        /// The zoom hack
        /// </summary>
        private bool zoomHack;

        #endregion

        #region Public Properties

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

                Camera.ExtendedZoom = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Applies a force on the camera position. Force should be normalized!
        /// </summary>
        /// <param name="position">The new endposition of the camera</param>
        /// <param name="priority">The priority.</param>
        public void ApplyForce(Vector3 position, int priority)
        {
            this.Position = Camera.Position;

            var newVec = Math.VectorBetween(this.Position, position);

            Console.WriteLine("Distance between: " + newVec);

            Action action = () =>
                {
                    Console.WriteLine("Old Position Vector: " + this.Position);
                    this.Position = this.Position + newVec;

                    Console.WriteLine("New Position Vector: " + this.Position);
                };

            this.queque.Enqueue(priority, action);
        }

        /// <summary>
        ///     Moves the camera.
        /// </summary>
        public void Reset()
        {
            Console.WriteLine("Reset");
            this.Position = Vector3.Zero;
            this.actionLimiter = false;
        }

        /// <summary>
        ///     Sets the position. Overwrites Forces!
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="priority">The priority.</param>
        public void SetPosition(Vector3 position, int priority)
        {
            Action action = () => { this.Position = position; };

            this.actionLimiter = true;

            this.queque.Enqueue(priority, action);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when [load].
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="featureBaseEventArgs"></param>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

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

                if (!this.actionLimiter)
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