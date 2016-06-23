namespace RethoughtLib.Notifications
{
    #region Using Directives

    using LeagueSharp.Common;

    using RethoughtLib.Notifications.Designs;

    using SharpDX;

    #endregion

    /// <summary>
    ///     A Notification
    /// </summary>
    public abstract class Notification
    {
        #region Fields

        /// <summary>
        ///     The notifications design
        /// </summary>
        internal readonly NotificationDesign NotificationDesign;

        /// <summary>
        ///     The margin
        /// </summary>
        internal float Margin;

        /// <summary>
        ///     Whether moving or not
        /// </summary>
        internal bool Moving;

        /// <summary>
        ///     The position
        /// </summary>
        internal Vector2 Position;

        /// <summary>
        ///     The start position
        /// </summary>
        internal Vector2 StartPosition;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Notification" /> class.
        /// </summary>
        /// <param name="notificationDesign">The notification design.</param>
        protected Notification(NotificationDesign notificationDesign)
        {
            this.NotificationDesign = notificationDesign;
        }

        #endregion

        #region Public Methods and Operators

        public virtual void Dispose()
        {
            if (this.Moving)
            {
                this.NotificationDesign.Transition.Start(
                    this.Position,
                    this.Position.Extend(this.StartPosition, this.NotificationDesign.Width + this.Margin));
            }

            this.Position = this.NotificationDesign.Transition.GetPosition();
        }

        /// <summary>
        ///     Draws this instance.
        /// </summary>
        public virtual void Draw()
        {
            return;
        }

        #endregion
    }
}