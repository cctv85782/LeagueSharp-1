namespace RethoughtLib.Notifications.Displayer
{
    #region Using Directives

    using System;

    using RethoughtLib.Design;

    using SharpDX;

    #endregion

    /// <summary>
    ///     The default Displayer
    /// </summary>
    /// <typeparam name="T">Class of type Notification</typeparam>
    /// <seealso cref="RethoughtLib.Notifications.Displayer.Displayer{T}" />
    public sealed class DefaultDisplayer<T> : Displayer<T>
        where T : Notification
    {
        #region Fields

        /// <summary>
        ///     The new offset
        /// </summary>
        private IntOffset offset;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultDisplayer{T}" /> class.
        /// </summary>
        public DefaultDisplayer()
        {
            this.OnNotificationAdd += this.OnAdd;

            this.OnNotificationDelete += this.OnDelete;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Displays the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Display(T notification)
        {
            base.Display(notification);

            notification.StartPosition = new Vector2(this.offset.Left, this.offset.Top);
            notification.Draw();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Raises the <see cref="E:Add" /> event.
        /// </summary>
        /// <param name="eventargs">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <param name="sender">The sender.</param>
        /// <exception cref="ArgumentException">The notification is already getting displayed</exception>
        protected override void OnAdd(EventArgs eventargs, T sender)
        {
            base.OnAdd(eventargs, sender);

            this.SetOffset();
        }

        /// <summary>
        ///     Raises the <see cref="E:Delete" /> event.
        /// </summary>
        /// <param name="eventargs">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <param name="sender">The sender.</param>
        /// <exception cref="ArgumentException">The notification is not getting displayed</exception>
        protected override void OnDelete(EventArgs eventargs, T sender)
        {
            base.OnDelete(eventargs, sender);

            this.SetOffset();
        }

        /// <summary>
        ///     Sets the offset.
        /// </summary>
        private void SetOffset()
        {
            var tempOffset = new IntOffset();

            foreach (var notification in this.ActiveNotifications)
            {
                tempOffset.Top += notification.Design.Height;
                tempOffset.Top += this.SpacingBetweenNotifications;
            }

            this.offset = tempOffset;
        }

        #endregion
    }
}