namespace RethoughtLib.Algorithm.Pathfinding
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    #endregion

    public abstract class PathfinderBase<TNode> : IPathfinder<TNode>
    {
        #region Public Methods and Operators

        public abstract List<TNode> Run(TNode node, TNode end1);

        #endregion
    }
}