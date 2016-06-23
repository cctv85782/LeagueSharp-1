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
            var notification =
                new DefaultNotification(
                    new TestDesign("I'm a notification", "I'm the content that gets displayed in the notification"));

            // Using the default displayer (without priority)
            Notifications.Instance.Add(notification);

            // Using a custom displayer (with priority)
            var displayer = new DefaultDisplayer() { DistanceLeft = 250, DistanceTop = 250 };

            Notifications.Instance.Add(notification, 15);
            // or (without priority)
            displayer.Display(notification);
        }

        #endregion
    }
}