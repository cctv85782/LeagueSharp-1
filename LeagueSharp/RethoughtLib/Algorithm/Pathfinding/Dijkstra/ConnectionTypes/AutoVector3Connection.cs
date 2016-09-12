﻿namespace RethoughtLib.Algorithm.Pathfinding.Dijkstra.ConnectionTypes
{
    #region Using Directives

    using LeagueSharp.Common;

    using SharpDX;

    #endregion

    public class AutoVector3Edge : Edge<Vector2>
    {
        #region Constructors and Destructors

        public AutoVector3Edge(Vector2 start, Vector2 end)
        {
            this.Start = start;
            this.End = end;
            this.Cost = this.Start.Distance(end);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the cost.
        /// </summary>
        /// <value>
        ///     The cost.
        /// </value>
        public sealed override float Cost { get; set; }

        /// <summary>
        ///     Gets or sets the end.
        /// </summary>
        /// <value>
        ///     The end.
        /// </value>
        public sealed override Vector2 End { get; set; }

        /// <summary>
        ///     Gets or sets the start.
        /// </summary>
        /// <value>
        ///     The start.
        /// </value>
        public sealed override Vector2 Start { get; set; }

        #endregion
    }
}