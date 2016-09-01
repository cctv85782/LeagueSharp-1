namespace RethoughtLib.Algorithm.Graphs
{
    using RethoughtLib.Algorithm.Pathfinding;

    public interface IGraphGenerator<TNode, TEdge>
        where TEdge : EdgeBase<TNode> where TNode : NodeBase
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        TNode Start { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        TNode End { get; set; }

        /// <summary>
        /// Generates the specified start to end grid
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        Graph<TNode, TEdge> Generate(TNode start, TNode end);

        #endregion
    }
}