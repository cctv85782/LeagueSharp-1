﻿namespace RethoughtLib.Notifications
{
    #region Using Directives

    using RethoughtLib.Notifications.Designs;

    #endregion

    public class DefaultNotification : Notification
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultNotification" /> class.
        /// </summary>
        /// <param name="notificationDesign">The notification design.</param>
        public DefaultNotification(NotificationDesign notificationDesign)
            : base(notificationDesign)
        { }

        #endregion

    }
}