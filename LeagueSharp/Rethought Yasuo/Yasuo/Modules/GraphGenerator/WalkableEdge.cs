namespace Rethought_Yasuo.Yasuo.Modules.GraphGenerator
{
    #region Using Directives

    using LeagueSharp.Common;

    using RethoughtLib.Algorithm.Pathfinding;

    #endregion

    internal class WalkableEdge : EdgeBase<Node>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WalkableEdge" /> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="cost">The cost.</param>
        public WalkableEdge(Node start, Node end, float cost)
        {
            this.Start = start;
            this.End = end;
            this.Cost = start.Position.Distance(end.Position) / cost;
        }

        #endregion
    }
}