namespace RethoughtLib.Notifications
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RethoughtLib.Notifications.Displayer;
    using RethoughtLib.PriorityQuequeV2;

    #endregion

    /// <summary>
    ///     The CastManager. Uses a singleton pattern. Access it over the Instance property.
    /// </summary>
    /// <seealso cref="Action" />
    public class Notifications
    {
        #region Static Fields

        /// <summary>
        ///     The displayers
        /// </summary>
        private static readonly List<Displayer.Displayer> Displayers = new List<Displayer.Displayer>() { new DefaultDisplayer() };

        /// <summary>
        ///     The instance
        /// </summary>
        private static Notifications instance;

        #endregion

        #region Fields

        /// <summary>
        ///     The queque
        /// </summary>
        internal PriorityQueue<int, Notification> Queque = new PriorityQueue<int, Notification>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>
        ///     The instance.
        /// </value>
        public static Notifications Instance => instance ?? (instance = new Notifications());

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the specified notification. If the displayer is null it will use the default displayer.
        /// </summary>
        /// <typeparam name="T">Displayer</typeparam>
        /// <param name="notification">The notification.</param>
        /// <param name="priority"></param>
        /// <param name="displayer">The displayer.</param>
        public void Add<T>(Notification notification, int priority = 0, T displayer = default(T))
            where T : Displayer.Displayer
        {
            if (displayer == null)
            {
                var defaultDisplayer = Displayers.FirstOrDefault();
                defaultDisplayer?.Display(notification);
                return;
            }

            if (!Displayers.Any(x => x.Equals(displayer)))
            {
                Displayers.Add(displayer);
                displayer.Display(notification);
                return;
            }

            var newDisplayer = Displayers.FirstOrDefault(x => x.Equals(displayer));
            newDisplayer?.Display(notification);
        }

        /// <summary>
        ///     Adds the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="priority">The priority.</param>
        public void Add(Notification notification, int priority = 0)
        {
            this.Add<Displayer.Displayer>(notification, priority);
        }

        #endregion
    }
}