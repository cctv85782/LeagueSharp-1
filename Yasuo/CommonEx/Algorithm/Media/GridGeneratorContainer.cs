namespace Yasuo.CommonEx.Algorithm.Media
{
    #region Using Directives

    using global::Yasuo.CommonEx.Algorithm.Djikstra;

    #endregion

    internal class GridGeneratorContainer
    {
        #region Fields

        /// <summary>
        ///     The grid
        /// </summary>
        internal Grid Grid;

        /// <summary>
        ///     The implementation
        /// </summary>
        internal IGridGenerator Implementation;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        public GridGeneratorContainer(IGridGenerator implementation)
        {
            this.Implementation = implementation;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Generates this instance.
        /// </summary>
        public void Generate()
        {
            this.Grid = this.Implementation.Generate();
        }

        #endregion
    }
}