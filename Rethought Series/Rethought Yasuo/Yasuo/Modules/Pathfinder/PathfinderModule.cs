using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rethought_Yasuo.Yasuo.Modules.Pathfinder
{
    using RethoughtLib.Algorithm.Graphs;
    using RethoughtLib.Algorithm.Pathfinding;
    using RethoughtLib.Algorithm.Pathfinding.Dijkstra;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;

    internal class PathfinderModule : ChildBase
    {
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Pathfinder";

        public List<NodeBase> GetPath(Graph<NodeBase, EdgeBase<NodeBase>> graph, Vector3 from, Vector3 to)
        {
            var djikstra = new Dijkstra<NodeBase, EdgeBase<NodeBase>>(graph.Edges);
        }
    }
}
