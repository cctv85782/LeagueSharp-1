namespace Rethought_Camera.Modules.Force
{
    internal class Force
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Force" /> class.
        /// </summary>
        /// <param name="mass">The mass.</param>
        /// <param name="acceleration">The acceleration.</param>
        public Force(double mass, double acceleration)
        {
            this.Mass = mass;
            this.Acceleration = acceleration;
        }

        #endregion

        #region Public Properties
            
        /// <summary>
        ///     Gets or sets the acceleration.
        /// </summary>
        /// <value>
        ///     The acceleration.
        /// </value>
        public double Acceleration { get; set; }

        /// <summary>
        ///     Gets or sets the mass.
        /// </summary>
        /// <value>
        ///     The mass.
        /// </value>
        public double Mass { get; set; }

        /// <summary>
        ///     Gets the result.
        /// </summary>
        /// <value>
        ///     The result.
        /// </value>
        public double Result => this.Mass * this.Acceleration;

        #endregion
    }
}