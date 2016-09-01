namespace ConsoleTest
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RethoughtLib.Algorithm.Pathfinding.AStar;

    using SharpDX;

    #endregion

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            var node1 = new AStarNode(new Vector3(0));
            var node2 = new AStarNode(new Vector3(14));
            var node3 = new AStarNode(new Vector3(15));
            var node4 = new AStarNode(new Vector3(6));
            var node5 = new AStarNode(new Vector3(1));
            var node6 = new AStarNode(new Vector3(8));

            var nodes = new List<AStarNode>() { node1, node2, node3, node4, node5, node6 };

            var edges = new List<AStarEdge<AStarNode>>();

            edges.Add(new AStarEdge<AStarNode>() {Cost = 10, Start = node1, End = node6});
            edges.Add(new AStarEdge<AStarNode>() { Cost = 5, Start = node5, End = node6 });
            edges.Add(new AStarEdge<AStarNode>() { Cost = 7, Start = node3, End = node6 });
            edges.Add(new AStarEdge<AStarNode>() { Cost = 1, Start = node2, End = node5 });
            edges.Add(new AStarEdge<AStarNode>() { Cost = 6, Start = node4, End = node4 });
            edges.Add(new AStarEdge<AStarNode>() { Cost = 4, Start = node6, End = node3 });
            edges.Add(new AStarEdge<AStarNode>() { Cost = 5, Start = node1, End = node2 });


            var astar = new AStar<AStarNode, AStarEdge<AStarNode>>(edges);

            var result = astar.Run(node1, node6);

            if (result == null)
            {
                Console.WriteLine("No Path Found");
            }

            if (result != null)
            {
                Console.WriteLine("Got " + result.Count);
                foreach (var entry in result)
                {
                    Console.Write($"{entry.Position} G: {entry.G}> ");
                }
                Console.WriteLine("Total Cost: " + result.Sum(x => x.G));
            }
            Console.ReadKey();
        }

        #endregion
    }
}