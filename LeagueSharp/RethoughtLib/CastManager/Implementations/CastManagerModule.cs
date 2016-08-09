namespace RethoughtLib.CastManager.Implementations
{
    #region Using Directives

    using System;

    using global::RethoughtLib.CastManager.Abstract_Classes;
    using global::RethoughtLib.Events;
    using global::RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class CastManagerModule : ChildBase
    {
        #region Fields

        /// <summary>
        ///     The cast manager
        /// </summary>
        public CastManagerBase CastManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CastManagerModule" /> class.
        /// </summary>
        /// <param name="castManager">The cast manager.</param>
        public CastManagerModule(CastManagerBase castManager)
        {
            this.CastManager = castManager;
        }

        #endregion

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
            this.CastManager.Process();
        }

        #endregion
    }
}