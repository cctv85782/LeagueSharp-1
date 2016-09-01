namespace RethoughtLib.Algorithm.Pathfinding
{
    #region Using Directives

    using System.Collections.Generic;

    #endregion

    internal interface IPathfinder<TNode>
    {
        #region Public Methods and Operators

        List<TNode> Run(TNode start, TNode end);

        #endregion
    }
}