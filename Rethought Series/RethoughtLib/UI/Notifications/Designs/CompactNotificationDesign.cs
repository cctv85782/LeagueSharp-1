﻿namespace RethoughtLib.UI.Notifications.Designs
{
    using System;
    using Transitions.Abstract_Base;

    internal class CompactNotificationDesign : NotificationDesign
    {
        /// <summary>
        ///     Gets or sets the height.
        /// </summary>
        /// <value>
        ///     The height.
        /// </value>
        public override int Height { get; set; }

        /// <summary>
        ///     Gets or sets the transition.
        /// </summary>
        /// <value>
        ///     The transition.
        /// </value>
        public override TransitionBase TransitionBase { get; set; }

        /// <summary>
        ///     Gets or sets the width.
        /// </summary>
        /// <value>
        ///     The width.
        /// </value>
        public override int Width { get; set; }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public override void Update<T>(T notification)
        {
            throw new NotImplementedException();
        }
    }
}
