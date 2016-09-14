namespace Rethought_Irelia.IreliaV1.GridGenerator
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Algorithm.Graphs;
    using RethoughtLib.Algorithm.Pathfinding;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Switches;

    using Rethought_Irelia.IreliaV1.Spells;

    using SharpDX;

    #endregion

    internal class GraphGeneratorModule : ChildBase, IGraphGenerator<NodeBase, EdgeBase<NodeBase>>
    {
        #region Fields

        private IEnumerable<Obj_AI_Base> units = new List<Obj_AI_Base>();

        #endregion

        #region Constructors and Destructors

        public GraphGeneratorModule(IreliaQ ireliaQ)
        {
            this.IreliaQ = ireliaQ;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     The spell
        /// </summary>
        public IreliaQ IreliaQ { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "GraphGenerator";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Generates the specified start to end graph
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public Graph<NodeBase, EdgeBase<NodeBase>> Generate(NodeBase start, NodeBase end)
        {
            this.units =
                ObjectManager.Get<Obj_AI_Base>()
                    .Where(x => x.ServerPosition.Distance(start.Position) <= 10000 && this.IreliaQ.WillReset(x))
                    .ToList();

            var result = new Graph<NodeBase, EdgeBase<NodeBase>>(new List<NodeBase>(), new List<EdgeBase<NodeBase>>());

            var nodes = this.units.Select(unit => new UnitNode(unit.ServerPosition, unit)).ToList();

            foreach (var centerNode in nodes)
            {
                foreach (
                    var neighbor in nodes.Where(x => x.Unit.Distance(centerNode.Position) <= this.IreliaQ.Spell.Range))
                {
                    result.Edges.Add(
                        new EdgeBase<NodeBase>()
                            {
                                Start = centerNode,
                                End = neighbor,
                                Cost =
                                    centerNode.Position.Distance(neighbor.Position)
                                    / this.IreliaQ.Spell.Speed
                            });
                }
            }

            result.Nodes = new List<NodeBase>(nodes);

            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            //this.Menu.AddItem(new MenuItem("prioritizechampions", "Prioritize Champions").SetValue(true));
        }

        /// <summary>
        ///     Sets the switch.
        /// </summary>
        protected override void SetSwitch()
        {
            this.Switch = new UnreversibleSwitch(this.Menu);
        }

        #endregion
    }

    public class UnitNode : NodeBase
    {
        #region Constructors and Destructors

        public UnitNode(Vector3 position, Obj_AI_Base unit)
            : base(position)
        {
            this.Unit = unit;
        }

        #endregion

        #region Public Properties

        public Obj_AI_Base Unit { get; set; }

        #endregion
    }
}