using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RethoughtLib.Notifications
{
    using RethoughtLib.Classes.Observer;

    /// <summary>
    ///     Observer implementation to handle changes in values
    /// </summary>
    /// <seealso cref="RethoughtLib.Classes.Observer.Observer" />
    public class DefaultNotificationObserver : Observer
    {
        private readonly DefaultNotification subject;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultNotificationObserver"/> class.
        /// </summary>
        /// <param name="subject">The subject.</param>
        public DefaultNotificationObserver(DefaultNotification subject)
        {
            this.subject = subject;

            if (GlobalVariables.Debug)
            {
                Console.WriteLine($"Observer attached to {subject}");
            }
        }

        public override void Update()
        {
            this.subject.Design.Update(this.subject);

            if (GlobalVariables.Debug)
            {
                Console.WriteLine($"Adjusted {this.subject}'s texts to the new size");
            }
        }
    }
}
