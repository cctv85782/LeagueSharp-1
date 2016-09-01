namespace RethoughtLib.CastManager.Abstract_Classes
{
    using System;
    using System.Linq;

    using global::RethoughtLib.PriorityQuequeV2;

    public interface ICastManager
    {
        /// <summary>
        /// Gets or sets the queque.
        /// </summary>
        /// <value>
        /// The queque.
        /// </value>
        PriorityQueue<int, Action> Queque { get; set; }

        /// <summary>
        ///     Processes all items that are supposed to get casted.
        /// </summary>
        void Process();
    }
}