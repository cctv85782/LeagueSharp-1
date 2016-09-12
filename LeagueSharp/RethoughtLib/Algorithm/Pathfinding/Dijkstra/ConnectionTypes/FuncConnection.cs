namespace RethoughtLib.Algorithm.Pathfinding.Dijkstra.ConnectionTypes
{
    #region Using Directives

    using System;

    #endregion

    public class FuncEdge<T> : Edge<T>
    {
        #region Constructors and Destructors

        public FuncEdge(T start, T end, Func<T, T, float> funcCost)
        {
            this.Start = start;
            this.End = end;

            this.Cost = funcCost.Invoke(this.Start, this.End);
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
        public sealed override T End { get; set; }

        /// <summary>
        ///     Gets or sets the start.
        /// </summary>
        /// <value>
        ///     The start.
        /// </value>
        public sealed override T Start { get; set; }

        #endregion
    }
}