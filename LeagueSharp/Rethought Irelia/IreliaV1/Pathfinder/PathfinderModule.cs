namespace Rethought_Irelia.IreliaV1.Pathfinder
{
    #region Using Directives

    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Algorithm.Graphs;
    using RethoughtLib.Algorithm.Pathfinding;
    using RethoughtLib.Algorithm.Pathfinding.Dijkstra;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Irelia.IreliaV1.GridGenerator;

    #endregion

    internal class PathfinderModule : ChildBase
    {
        #region Constants

        /// <summary>
        ///     The prioritization multiplicand
        /// </summary>
        private const float PrioritizationMultiplicand = 0.5f;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Pathfinder";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the path.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public List<NodeBase> GetPath(Graph<NodeBase, EdgeBase<NodeBase>> graph, NodeBase from, NodeBase to)
        {
            if (this.Menu.Item("prioritizechampion").GetValue<bool>())
            {
                foreach (var edgeBase in graph.Edges)
                {
                    var node = edgeBase.End as UnitNode;

                    var unitNode = node;

                    if (!(unitNode?.Unit is Obj_AI_Hero))
                    {
                        continue;
                    }

                    edgeBase.Cost = edgeBase.Cost *= PrioritizationMultiplicand;
                }

            }

            var dijkstra = new Dijkstra<NodeBase, EdgeBase<NodeBase>>(graph.Edges);

            dijkstra.SetStart(from);

            return dijkstra.GetNodesTo(to);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(new MenuItem("prioritizechampion", "Prioritize Champions").SetValue(true));
        }

        #endregion
    }
}