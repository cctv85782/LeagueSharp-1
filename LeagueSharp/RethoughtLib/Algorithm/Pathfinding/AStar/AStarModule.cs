namespace RethoughtLib.Algorithm.Pathfinding.AStar
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp.Common;

    using RethoughtLib.Algorithm.Pathfinding.AStar.Heuristics;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    public class AStarModule<TNode, TEdge> : ChildBase
        where TEdge : AStarEdge<TNode> where TNode : AStarNode
    {
        #region Fields

        private readonly List<IHeuristic> heuristics = new List<IHeuristic>()
                                                           {
                                                               new HeuristicEuclidean(),
                                                               new HeuristicManhattan(),
                                                               new HeuristicEuclideanNoSqr(),
                                                               new HeuristicMaxDxdy()
                                                           };

        private AStar<TNode, TEdge> algorithm;

        private float heuristicEstimate;

        private IHeuristic heuristicFormula;

        private bool reopenClosedNodes;

        private bool tieBreaking;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "A-Star (A*) Algorithm";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the path. Returns null if no path found.
        /// </summary>
        /// <param name="edges">The edges.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public List<TNode> GetPath(List<TEdge> edges, TNode start, TNode end)
        {
            this.algorithm = new AStar<TNode, TEdge>(edges)
                                 {
                                     HeuristicFormula = this.heuristicFormula,
                                     ReopenCloseNodes = this.reopenClosedNodes,
                                     TieBreaker = this.tieBreaking
                                 };

            return this.algorithm.Run(start, end);
        }

        /// <summary>
        ///     Gets the path. Returns null if no path found.
        /// </summary>
        /// <param name="edges">The edges.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public List<TNode> GetPath(List<TEdge> edges, TNode start, TNode end, IHeuristic heuristic, bool reopenClosedNodes, bool tieBreaking)
        {
            this.algorithm = new AStar<TNode, TEdge>(edges)
            {
                HeuristicFormula = heuristic,
                ReopenCloseNodes = reopenClosedNodes,
                TieBreaker = tieBreaking
            };

            return this.algorithm.Run(start, end);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            var stringArray = new string[] { };

            var index = 0;

            foreach (var heuristic in this.heuristics)
            {
                index++;
                stringArray[index] = nameof(heuristic);
            }

            this.Menu.AddItem(
                new MenuItem("HeuristicFormula", "Heuristic Formula: ").SetValue(new StringList(stringArray)))
                .ValueChanged += (o, args) =>
                    {
                        var heuristic = args.GetNewValue<StringList>().SelectedValue;
                        this.heuristicFormula = this.heuristics.FirstOrDefault(x => nameof(x) == heuristic);
                    };
            var heuristic2 = this.Menu.Item("HeuristicFormula").GetValue<StringList>().SelectedValue;
            this.heuristicFormula = this.heuristics.FirstOrDefault(x => nameof(x) == heuristic2);

            // TODO
            this.Menu.AddItem(new MenuItem("HeuristicEstimate", "Heuristic Estimate: ").SetValue(new Slider(0, 0, 100)))
                .ValueChanged += (o, args) => { this.heuristicEstimate = args.GetNewValue<Slider>().Value; };
            this.heuristicEstimate = this.Menu.Item("HeuristicEstimate").GetValue<Slider>().Value;

            this.Menu.AddItem(new MenuItem("ReopenClosedNodes", "Reopen Closed Nodes").SetValue(false)).ValueChanged +=
                (o, args) => { this.reopenClosedNodes = args.GetNewValue<bool>(); };
            this.reopenClosedNodes = this.Menu.Item("ReopenClosedNodes").GetValue<bool>();

            this.Menu.AddItem(new MenuItem("TieBreaker", "Tie Breaking").SetValue(true)).ValueChanged +=
                (o, args) => { this.tieBreaking = args.GetNewValue<bool>(); };
            this.tieBreaking = this.Menu.Item("TieBreaker").GetValue<bool>();
        }

        #endregion
    }
}