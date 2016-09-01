namespace RethoughtLib.CastManager.Implementations
{
    #region Using Directives

    using System;
    using System.Linq;

    using global::RethoughtLib.CastManager.Abstract_Classes;
    using global::RethoughtLib.Events;
    using global::RethoughtLib.FeatureSystem.Abstract_Classes;

    using RethoughtLib.PriorityQuequeV2;

    #endregion

    public class CastManagerModule : ChildBase, ICastManager
    {
        #region Public Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Cast Manager";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            Events.OnPostUpdate -= this.OnPostUpdate;
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Events.OnPostUpdate += this.OnPostUpdate;
        }

        /// <summary>
        ///     Raises the <see cref="E:PostUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnPostUpdate(EventArgs args)
        {
            this.Process();
        }

        #endregion

        /// <summary>
        /// Gets or sets the queque.
        /// </summary>
        /// <value>
        /// The queque.
        /// </value>
        public PriorityQueue<int, Action> Queque { get; set; }

        /// <summary>
        ///     Processes all items that are supposed to get casted.
        /// </summary>
        public void Process()
        {
            try
            {
                for (var i = 0; i < this.Queque.Dictionary.ToList().Count; i++)
                {
                    var action = this.Queque.Dequeue();

                    action?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex: " + ex);
            }
        }
    }
}