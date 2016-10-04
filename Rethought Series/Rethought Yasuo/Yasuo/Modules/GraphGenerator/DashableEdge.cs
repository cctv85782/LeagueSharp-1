namespace Rethought_Yasuo.Yasuo.Modules.GraphGenerator
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Algorithm.Pathfinding;

    #endregion

    internal class DashableEdge : EdgeBase<Node>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DashableEdge" /> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="dashSpeed">The dash speed.</param>
        /// <param name="dashUnit">The unit that gets dashed over</param>
        public DashableEdge(Node start, Node end, float dashSpeed, Obj_AI_Base dashUnit)
        {
            this.DashUnit = dashUnit;
            this.Start = start;
            this.End = end;
            this.Cost = start.Position.Distance(end.Position) / dashSpeed;
        }

        #endregion

        #region Public Properties

        public Obj_AI_Base DashUnit { get; set; }

        #endregion
    }
}