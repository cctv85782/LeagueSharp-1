namespace RethoughtLib.Algorithm.Pathfinding.AStar.Heuristics
{
    #region Using Directives

    using System;

    #endregion

    public class HeuristicManhattan : IHeuristic
    {
        #region Public Methods and Operators

        public float Result(NodeBase node1, NodeBase node2)
        {
            Console.WriteLine("node1 X: " + node1.Position.X);
            Console.WriteLine("node2 X: " + node2.Position.X);
            Console.WriteLine("node1 Y: " + node1.Position.Y);
            Console.WriteLine("node2 Y: " + node2.Position.Y);

            var result = Math.Abs(node1.Position.X - node2.Position.X) + Math.Abs(node1.Position.Y - node2.Position.Y);

            Console.WriteLine(result);

            return result;
        }

        #endregion
    }
}