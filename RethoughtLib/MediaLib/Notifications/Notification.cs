namespace RethoughtLib.Notifications
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp.Common;

    using RethoughtLib.Classes.Observer;
    using RethoughtLib.Notifications.Designs;

    using SharpDX;

    #endregion

    /// <summary>
    ///     A Notification
    /// </summary>
    public abstract class Notification : Element<NotificationDesign>
    {
        #region Fields

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
            this.Design = notificationDesign;
        }

        /// <summary>
        /// Gets or sets the design.
        /// </summary>
        /// <value>
        /// The design.
        /// </value>
        public sealed override NotificationDesign Design { get; set; }

        #endregion

        #region Public Methods and Operators

        public virtual void Dispose()
        {
            if (this.Moving)
            {
                this.Design.Transition.Start(
                    this.Position,
                    this.Position.Extend(this.StartPosition, this.Design.Width));
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