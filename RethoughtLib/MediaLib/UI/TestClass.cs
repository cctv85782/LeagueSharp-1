namespace RethoughtLib.UI
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using RethoughtLib.UI.Core;
    using RethoughtLib.UI.Core.Designs;
    using RethoughtLib.UI.DefaultImplementations.Displayer;
    using RethoughtLib.UI.Notifications;
    using RethoughtLib.UI.Notifications.Designs;

    #endregion

    internal class TestClass
    {
        #region Methods

        private void Test()
        {
            // displayer that accepts every type of ELEMENT
            var displayer = new DefaultDisplayer<Element>();

            var notification = new DefaultNotification("I'm a title", "I'm the content")
                                   {
                                       Design = new CompactNotificationDesign()
                                   };

            displayer.Add(notification);
        }

        #endregion
    }
}