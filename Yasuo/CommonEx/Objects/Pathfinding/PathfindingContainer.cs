namespace Yasuo.CommonEx.Objects.Pathfinding
{
    /// <summary>
    ///     Container that initializes and manages pathfinder implementations (only yasuo)
    /// </summary>
    class PathfindingContainer
    {
        #region Fields

        /// <summary>
        ///     The pathfinder implementation
        /// </summary>
        private readonly IPathfinder pathfinderImplementation;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PathfindingContainer" /> class.
        /// </summary>
        /// <param name="pathfinderImplementation">The pathfinder implementation.</param>
        public PathfindingContainer(IPathfinder pathfinderImplementation)
        {
            this.pathfinderImplementation = pathfinderImplementation;
            this.pathfinderImplementation.Initialize();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the path.
        /// </summary>
        /// <returns></returns>
        public Path GetPath()
        {
            var path = this.pathfinderImplementation.GeneratePath();

            return path;
        }

        public void ExecutePath()
        {
            this.pathfinderImplementation.ExecutePath();
        }

        #endregion
    }
}