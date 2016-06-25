namespace RethoughtLib.Notifications.Designs
{
    #region Using Directives

    using RethoughtLib.Transitions;

    #endregion

    /// <summary>
    ///     Base NotificationDesign
    /// </summary>
    public abstract class NotificationDesign : Design
    {
        #region Public Properties

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public abstract void Update<T>(T notification) where T : Notification;

        #endregion
    }
}