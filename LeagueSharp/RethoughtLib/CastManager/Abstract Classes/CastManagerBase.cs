namespace RethoughtLib.CastManager.Abstract_Classes
{
    using System;
    using System.Linq;

    using global::RethoughtLib.PriorityQuequeV2;

    public abstract class CastManagerBase
    {
        public PriorityQueue<int, Action> Queque = new PriorityQueue<int, Action>();

        /// <summary>
        ///     Forces the action if there are no extremely high valued actions in the queue
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="priority">If an action with a "higher" priority than specified is queued the action won't get forced</param>
        public void ForceAction(Action action, int priority)
        {
            if (this.Queque.Dictionary.Any(x => x.Key == 0))
            {
#if DEBUG
                Console.WriteLine(
                    "CastManager: ForceAction(Action) > Returned and queued because there was a prioritized action");
#endif

                this.Queque.Enqueue(priority, action);

                return;
            }

            action.Invoke();
        }

        /// <summary>
        ///     Processes all items that are supposed to get casted.
        /// </summary>
        public abstract void Process();
    }
}