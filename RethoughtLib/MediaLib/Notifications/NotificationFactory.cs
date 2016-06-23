namespace RethoughtLib.Notifications
{
    using Designs;

    // TODO
    public class NotificationFactory
    {
        #region Fields

        public NotificationDesign NotificationDesign;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public NotificationFactory(NotificationDesign notificationDesign)
        {
            this.NotificationDesign = notificationDesign;
        }

        #endregion

        #region Public Methods and Operators

        public Notification Create()
        {
            return new DefaultNotification(this.NotificationDesign);
        }

        #endregion
    }
}