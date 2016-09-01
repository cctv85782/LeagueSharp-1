﻿namespace RethoughtLib.Algorithm.Pathfinding.AStar
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp.Common;

    using Priority_Queue;

    using RethoughtLib.Algorithm.Pathfinding.AStar.Heuristics;

    using SharpDX;

    #endregion

    public class AStar<TNode, TEdge> : PathfinderBase<TNode>
        where TNode : AStarNode where TEdge : AStarEdge<TNode>
    {
        #region Fields

        /// <summary>
        /// The edges
        /// </summary>
        private readonly List<TEdge> edges;

        /// <summary>
        /// The open nodes
        /// </summary>
        private readonly SimplePriorityQueue<TNode> openNodes = new SimplePriorityQueue<TNode>();

        /// <summary>
        /// The closed nodes
        /// </summary>
        private readonly List<TNode> closedNodes = new List<TNode>();

        /// <summary>
        ///     The heuristic estimate
        /// </summary>
        private float heuristicEstimate = 0f;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AStar" /> class.
        /// </summary>
        /// <param name="edges">The edges representing a graph.</param>
        public AStar(List<TEdge> edges)
        {
            this.edges = edges;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="AStar" /> is finished.
        /// </summary>
        /// <value>
        ///     <c>true</c> if finished; otherwise, <c>false</c>.
        /// </value>
        public bool Finished { get; set; } = true;

        /// <summary>
        ///     Gets or sets the heuristic formula.
        /// </summary>
        /// <value>
        ///     The heuristic formula.
        /// </value>
        public IHeuristic HeuristicFormula { get; set; } = new HeuristicManhattan();

        /// <summary>
        ///     The reopen close nodes bool
        /// </summary>
        public bool ReopenCloseNodes { get; set; } = false;

        public bool TieBreaker { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Runs the specified start.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override List<TNode> Run(TNode start, TNode end)
        {
            this.Finished = false;

            this.openNodes.Clear();
            this.closedNodes.Clear();

            start.H = this.heuristicEstimate;
            start.G = 0;
            start.F = start.G + start.H;

            this.openNodes.Enqueue(start, start.F);

            while (this.openNodes.Count > 0)
            {
                var parentNode = this.openNodes.Dequeue();

                // Possible Path Found
                if (parentNode.Position == end.Position)
                {
                    Console.WriteLine("Found Solution");
                    this.closedNodes.Add(parentNode);
                    this.Finished = true;
                    break;
                }

                // Foreach Connection
                foreach (var edge in this.edges.Where(x => x.Start.Equals(parentNode)))
                {
                    Console.WriteLine("Searching");

                    var newNode = edge.End;

                    newNode.G = edge.Cost;

                    if (Math.Abs(newNode.G - parentNode.G) < 0.1f)
                    {
                        Console.WriteLine("Parent and New are the same");
                        continue;
                    }

                    var iOpen = -1;

                    AStarNode resultNodeOpen = null;

                    foreach (var node in this.openNodes)
                    {
                        iOpen++;

                        if (node.Position != newNode.Position) continue;
                        resultNodeOpen = node;
                        break;
                    }

                    if (resultNodeOpen != null && iOpen != -1 && resultNodeOpen.G <= newNode.G) continue;

                    var iClosed = -1;

                    AStarNode resultNodeClosed = null;

                    foreach (var node in this.closedNodes)
                    {
                        iClosed++;

                        if (node.Position != newNode.Position) continue;
                        resultNodeClosed = node;
                        break;
                    }

                    if (resultNodeClosed != null
                        && (this.ReopenCloseNodes || iClosed != -1 && resultNodeClosed.G <= newNode.G)) continue;

                    newNode.ParentNode = parentNode;

                    newNode.H = this.heuristicEstimate * this.HeuristicFormula.Result(newNode, end);

                    if (this.TieBreaker)
                    {
                        var dx1 = parentNode.Position.X - end.Position.X;
                        var dy1 = parentNode.Position.Y - end.Position.Y;
                        var dx2 = start.Position.X - end.Position.X;
                        var dy2 = start.Position.Y - end.Position.Y;
                        var cross = Math.Abs(dx1 * dy2 - dx2 * dy1);
                        newNode.H = newNode.H + cross * 0.001f;
                    }

                    newNode.F = newNode.G + newNode.H;

                    this.openNodes.Enqueue(newNode, newNode.F);
                }

                this.closedNodes.Add(parentNode);
            }

            if (!this.Finished) return null;

            var fNode = this.closedNodes[this.closedNodes.Count - 1];

            for (var i = this.closedNodes.Count - 1; i >= 0; i--)
            {
                    if (Math.Abs(fNode.ParentNode.Position.X - this.closedNodes[i].Position.X) < 0.1f
                    && Math.Abs(fNode.ParentNode.Position.Y - this.closedNodes[i].Position.Y) < 0.1f
                    || i == this.closedNodes.Count - 1)
                {
                    fNode = this.closedNodes[i];
                }
                else this.closedNodes.RemoveAt(i);
            }

            return this.closedNodes;
        }

        #endregion
    }
}