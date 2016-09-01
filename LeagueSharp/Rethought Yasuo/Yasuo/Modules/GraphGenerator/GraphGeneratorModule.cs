namespace Rethought_Yasuo.Yasuo.Modules.GraphGenerator
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Algorithm.Graphs;
    using RethoughtLib.Algorithm.Pathfinding;

    using Rethought_Yasuo.Yasuo.Modules.Core.SpellParent;
    using Rethought_Yasuo.Yasuo.Modules.Core.SpellParent.Implementations;

    using SharpDX;

    #endregion

    internal class GraphGeneratorModule : IGraphGenerator<Node, EdgeBase<Node>>
    {
        #region Fields

        private readonly ISpellIndex spellIndex;

        private readonly YasuoE yasuoE;

        private List<Node> nodes = new List<Node>();

        private List<EdgeBase<Node>> edges = new List<EdgeBase<Node>>();

        #endregion

        #region Constructors and Destructors

        public GraphGeneratorModule(ISpellIndex spellIndex)
        {
            this.spellIndex = spellIndex;

            this.yasuoE = (YasuoE)spellIndex[SpellSlot.E];
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the end.
        /// </summary>
        /// <value>
        ///     The end.
        /// </value>
        public Node End { get; set; }

        /// <summary>
        ///     Gets or sets the start.
        /// </summary>
        /// <value>
        ///     The start.
        /// </value>
        public Node Start { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Creates a new unique grid for every unit in dash range
        /// </summary>
        public Graph<Node, EdgeBase<Node>> Generate(Node start, Node end)
        {
            try
            {
                this.Reset();

                // Setting up the first points
                foreach (var unit in
                    // TODO spellModule for range
                    MinionManager.GetMinions(
                        start.Position,
                        this.spellIndex[SpellSlot.E].Spell.Range,
                        MinionTypes.All,
                        MinionTeam.NotAlly)
                        .Where(x => x.Distance(start.Position) <= this.spellIndex[SpellSlot.E].Spell.Range))
                {
                    var nodeToAdd = new Node(this.yasuoE.DashEndPosition(start.Position, start.Position.Extend(unit.ServerPosition, this.yasuoE.Spell.Range)));

                    this.nodes.Add(nodeToAdd);
                    this.edges.Add(new DashableEdge(start, nodeToAdd, this.yasuoE.Spell.Speed, unit));
                }

                // Connecting StartPoint to EndPoint
                var path2 = ObjectManager.Player.GetPath(start.Position, end.Position);

                if (path2 != null && path2.Length > 0)
                {
                    for (var i = 0; i < path2.Count() - 1; i++)
                    {
                        var point = new Node(path2[i]);

                        if (i == 0)
                        {
                            point = start;
                        }

                        var point1 = new Node(path2[i + 1]);

                        if (i == path2.Count() - 2)
                        {
                            point1 = end;
                        }

                        var connection = new WalkableEdge(point, point1, point1.Position.Distance(point.Position) / ObjectManager.Player.MoveSpeed);

                        if (!this.edges.Contains(connection))
                        {
                            this.edges.Add(connection);
                        }
                    }
                }
                else
                {
                    this.edges.Add(
                        new WalkableEdge(
                            start,
                            end,
                            start.Position.Distance(end.Position) / ObjectManager.Player.MoveSpeed));
                }

                // Starts generating possible pathes
                for (var i = 0; i < this.PathDeepness; i++)
                {
                    if (!this.sharedPoints.Any())
                    {
                        break;
                    }

                    foreach (var point in this.sharedPoints.ToList())
                    {
                        var localBlacklist = this.Backtrace(point, this.MaxConnections);

                        var unitCount =
                            this.Units.Where(
                                unit =>
                                Geometry.Distance(unit, point.Position) <= GlobalVariables.Spells[SpellSlot.E].Range)
                                .Count(unit => !localBlacklist.Contains(unit));

                        // Remove point from list and continue because there are no valid dashes available around that point
                        if (unitCount == 0)
                        {
                            this.sharedPoints.Remove(point);
                            continue;
                        }

                        this.ProcessPoint(point, localBlacklist);
                        this.sharedPoints.Remove(point);
                    }
                }

                this.sharedPoints.Add(end);

                return new Graph<Node, EdgeBase<Node>>(this.edges);
            }
            catch (Exception ex)
            {
                this.Reset();
                Console.WriteLine(ex);
            }
        }

        #endregion

        #region Methods

        private List<EdgeBase<NodeBase>>

        private void Reset()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}