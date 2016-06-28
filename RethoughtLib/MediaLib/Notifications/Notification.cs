namespace RethoughtLib.Notifications
{
    #region Using Directives

    using LeagueSharp.Common;

    using RethoughtLib.Notifications.Designs;
    using RethoughtLib.Transitions;

    using SharpDX;

    #endregion

    /// <summary>
    ///     A Notification
    /// </summary>
    public abstract class Notification : Element
    {
        #region Fields

        /// <summary>
        ///     Whether moving or not
        /// </summary>
        internal bool Moving;

        /// <summary>
        ///     The start position
        /// </summary>
        internal Vector2 StartPosition;

        /// <summary>
        ///     The transition
        /// </summary>
        internal Transition Transition = new ExpoEaseInOut(0.5);

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the design.
        /// </summary>
        /// <value>
        ///     The design.
        /// </value>
        internal new virtual NotificationDesign Design { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Attaches the design.
        /// </summary>
        /// <param name="design">The design.</param>
        public virtual void AttachDesign(NotificationDesign design)
        {
            this.Design = design;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (this.Moving)
            {
                this.Design.Transition.Start(this.Position, this.Position.Extend(this.StartPosition, this.Design.Width));
            }

            this.Position = this.Design.Transition.GetPosition();
        }

        /// <summary>
        ///     Draws this instance.
        /// </summary>
        public override void Draw()
        {
        }

        #endregion
    }
}