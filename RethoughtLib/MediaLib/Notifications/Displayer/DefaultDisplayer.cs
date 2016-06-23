namespace RethoughtLib.Notifications.Displayer
{
    #region Using Directives

    using System;

    using RethoughtLib.Design;

    using SharpDX;

    #endregion

    /// <summary>
    ///     The default implementation of a Displayer
    /// </summary>
    /// <seealso cref="RethoughtLib.Notifications.Displayer.Displayer" />
    public sealed class DefaultDisplayer : Displayer
    {
        #region Fields

        /// <summary>
        ///     The new offset
        /// </summary>
        private IntOffset offset;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultDisplayer" /> class.
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
        public override void Display(Notification notification)
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
        private void OnAdd(EventArgs eventargs, Notification sender)
        {
            if (this.ActiveNotifications.Contains(sender))
            {
                throw new ArgumentException("The notification is already getting displayed.");
            }

            this.SetNewOffset();

            this.ActiveNotifications.Add(sender);
        }

        /// <summary>
        ///     Raises the <see cref="E:Delete" /> event.
        /// </summary>
        /// <param name="eventargs">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <param name="sender">The sender.</param>
        /// <exception cref="ArgumentException">The notification is not getting displayed</exception>
        private void OnDelete(EventArgs eventargs, Notification sender)
        {
            if (!this.ActiveNotifications.Contains(sender))
            {
                throw new ArgumentException("The notification is not getting displayed.");
            }

            this.SetNewOffset();

            this.ActiveNotifications.Remove(sender);
        }

        /// <summary>
        ///     Sets the new offset.
        /// </summary>
        private void SetNewOffset()
        {
            var tempOffset = new IntOffset();

            foreach (var notification in this.ActiveNotifications)
            {
                tempOffset.Top += notification.NotificationDesign.Height;
                tempOffset.Top += this.SpacingBetweenNotifications;
            }

            this.offset = tempOffset;
        }

        #endregion
    }
}