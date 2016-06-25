namespace RethoughtLib.Notifications.Displayer
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using SharpDX;

    #endregion

    /// <summary>
    ///     Displayer are declaring zones and behaiviors to display Notifications
    /// </summary>
    /// <typeparam name="T">Class of type Notification</typeparam>
    public abstract class Displayer<T>
        where T : Notification
    {
        #region Fields

        /// <summary>
        ///     The active notifications
        /// </summary>
        protected List<T> ActiveNotifications = new List<T>();

        #endregion

        #region Delegates

        /// <summary>
        /// </summary>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <param name="sender">The sender.</param>
        public delegate void DisplayerEvent(EventArgs eventArgs, T sender);

        #endregion

        #region Public Events

        /// <summary>
        ///     Occurs when [on notification add].
        /// </summary>
        public event DisplayerEvent OnNotificationAdd;

        /// <summary>
        ///     Occurs when [on notification delete].
        /// </summary>
        public event DisplayerEvent OnNotificationDelete;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the bot left point.
        /// </summary>
        /// <value>
        ///     The bot left point.
        /// </value>
        public Point BotLeftPoint => new Point(this.DistanceLeft, this.DistanceTop + this.MaxHeight);

        /// <summary>
        ///     Gets the bot right point.
        /// </summary>
        /// <value>
        ///     The bot right point.
        /// </value>
        public Point BotRightPoint => new Point(this.DistanceLeft + this.MaxWidth, this.DistanceTop + this.MaxHeight);

        /// <summary>
        ///     Gets the distance left (in pixels).
        /// </summary>
        /// <value>
        ///     The distance left.
        /// </value>
        public virtual int DistanceLeft { get; set; } = 40;

        /// <summary>
        ///     Gets the distance top (in pixels).
        /// </summary>
        /// <value>
        ///     The distance left.
        /// </value>
        public virtual int DistanceTop { get; set; } = 195;

        /// <summary>
        ///     Gets the maximum height (in pixels)
        /// </summary>
        /// <value>
        ///     The maximum height.
        /// </value>
        public virtual int MaxHeight { get; set; } = 700;

        /// <summary>
        ///     Gets the maximum width (in pixels)
        /// </summary>
        /// <value>
        ///     The maximum width.
        /// </value>
        public virtual int MaxWidth { get; set; } = 300;

        /// <summary>
        ///     Gets or sets the spacing between notifications.
        /// </summary>
        /// <value>
        ///     The spacing between notifications.
        /// </value>
        public virtual int SpacingBetweenNotifications { get; set; } = 25;

        /// <summary>
        ///     Gets the top left point.
        /// </summary>
        /// <value>
        ///     The top left point.
        /// </value>
        public Point TopLeftPoint => new Point(this.DistanceLeft, this.DistanceTop);

        /// <summary>
        ///     Gets the top right point.
        /// </summary>
        /// <value>
        ///     The top right point.
        /// </value>
        public Point TopRightPoint => new Point(this.DistanceLeft + this.MaxWidth, this.DistanceTop);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Displays the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public virtual void Display(T notification)
        {
            this.OnOnNotificationAdd(notification);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Raises the <see cref="E:Add" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <param name="sender">The sender.</param>
        /// <exception cref="ArgumentException">ActiveNotifications already contains this notification.</exception>
        protected virtual void OnAdd(EventArgs args, T sender)
        {
            if (this.ActiveNotifications.Contains(sender))
            {
                throw new ArgumentException("ActiveNotifications already contains this notification.");
            }

            this.ActiveNotifications.Add(sender);
        }

        /// <summary>
        ///     Raises the <see cref="E:Delete" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <param name="sender">The sender.</param>
        /// <exception cref="ArgumentException">ActiveNotifications does not contain this notification.</exception>
        protected virtual void OnDelete(EventArgs args, T sender)
        {
            if (!this.ActiveNotifications.Contains(sender))
            {
                throw new ArgumentException("ActiveNotifications does not contain this notification.");
            }

            this.ActiveNotifications.Remove(sender);
        }

        /// <summary>
        ///     Called when [on notification add].
        /// </summary>
        /// <param name="sender">The sender.</param>
        protected virtual void OnOnNotificationAdd(T sender)
        {
            this.OnNotificationAdd?.Invoke(EventArgs.Empty, sender);
        }

        /// <summary>
        ///     Called when [on notification delete].
        /// </summary>
        /// <param name="sender">The sender.</param>
        protected virtual void OnOnNotificationDelete(T sender)
        {
            this.OnNotificationDelete?.Invoke(EventArgs.Empty, sender);
        }

        #endregion
    }
}