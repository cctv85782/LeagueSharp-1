namespace RethoughtLib.Notifications
{
    #region Using Directives

    using RethoughtLib.Notifications.Designs;
    using RethoughtLib.Notifications.Displayer;

    #endregion

    internal class TestClass
    {
        #region Methods

        private void Test()
        {
            var notification = new DefaultNotification("I'm a title", "I'm the content");


            // Using the default displayer (without priority)
            Notifications.Instance.Add(notification);

            // Using a custom displayer (with priority)
            var displayerThatAcceptsEveryNotification = new DefaultDisplayer<Notification> { DistanceLeft = 250, DistanceTop = 250 };

            var displayerThatOnlyAcceptsDefaultNotifications = new DefaultDisplayer<DefaultNotification>();

            Notifications.Instance.Add(notification, 15);
            // or (without priority)
            displayerThatAcceptsEveryNotification.Add(notification);
        }

        #endregion
    }
}