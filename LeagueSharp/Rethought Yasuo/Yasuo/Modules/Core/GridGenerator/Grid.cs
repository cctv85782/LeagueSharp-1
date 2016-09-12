namespace Rethought_Yasuo.Yasuo.Modules.Core.GridGenerator
{
    #region Using Directives

    using System.Collections.Generic;

    using RethoughtLib.Algorithm.Pathfinding.Dijkstra.ConnectionTypes;

    #endregion

    public class Graph<T, TV>
        where TV : Edge<T>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public Graph(List<TV> connections)
        {
            if (connections != null)
            {
                this.Edges = connections;
            }

            this.EdgesToPoints(connections);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the connections.
        /// </summary>
        /// <value>
        ///     The connections.
        /// </value>
        public List<TV> Edges { get; set; } = new List<TV>();

        /// <summary>
        ///     Point where the grid ends
        /// </summary>
        public T EndNode { get; set; }

        /// <summary>
        ///     Gets or sets the points.
        /// </summary>
        /// <value>
        ///     The points.
        /// </value>
        public List<T> Nodes { get; set; } = new List<T>();

        /// <summary>
        ///     Point where grid starts
        /// </summary>
        public T StartNode { get; set; }

        #endregion

        #region Methods

        private void EdgesToPoints(IEnumerable<TV> connections)
        {
            foreach (var connection in connections)
            {
                if (!this.Nodes.Contains(connection.End))
                {
                    this.Nodes.Add(connection.End);
                }

                if (!this.Nodes.Contains(connection.Start))
                {
                    this.Nodes.Add(connection.Start);
                }
            }
        }

        #endregion
    }
}