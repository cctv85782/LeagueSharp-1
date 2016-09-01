namespace RethoughtLib.Algorithm.Graphs
{
    #region Using Directives

    using RethoughtLib.Algorithm.Pathfinding;

    #endregion

    internal class GraphGenerator<TNode, TEdge> : IGraphGenerator<TNode, TEdge>
        where TNode : NodeBase where TEdge : EdgeBase<TNode>
    {
        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        public TNode Start { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public TNode End { get; set; }

        /// <summary>
        /// Generates the specified start to end grid
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public Graph<TNode, TEdge> Generate(TNode start, TNode end)
        {
            throw new System.NotImplementedException();
        }
    }
}