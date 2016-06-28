using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RethoughtLib.Notifications.Designs
{
    using RethoughtLib.Transitions;

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
        public override Transition Transition { get; set; }

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
