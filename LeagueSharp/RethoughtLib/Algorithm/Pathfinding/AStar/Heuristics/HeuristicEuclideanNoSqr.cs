﻿namespace RethoughtLib.Algorithm.Pathfinding.AStar.Heuristics
{
    #region Using Directives

    using System;

    #endregion

    public class HeuristicEuclideanNoSqr : IHeuristic
    {
        #region Public Methods and Operators

        public float Result(NodeBase node1, NodeBase node2)
        {
            return
                (float)
                (Math.Pow(node1.Position.X - node2.Position.X, 2) + Math.Pow(node1.Position.Y - node2.Position.Y, 2));
        }

        #endregion
    }
}