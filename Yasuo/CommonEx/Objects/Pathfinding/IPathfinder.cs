namespace Yasuo.CommonEx.Objects.Pathfinding
{
    public interface IPathfinder
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the path.
        /// </summary>
        /// <returns></returns>
        Path GeneratePath();

        void ExecutePath();

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        void Initialize();

        #endregion
    }
}