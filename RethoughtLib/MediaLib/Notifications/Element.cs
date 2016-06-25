namespace RethoughtLib.Notifications
{
    #region Using Directives

    using RethoughtLib.Notifications.Designs;

    #endregion

    public abstract class Element<T> where T : Design
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the design.
        /// </summary>
        /// <value>
        /// The design.
        /// </value>
        public abstract T Design { get; set; }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        public virtual void Draw()
        {
                
        }

        #endregion
    }
}