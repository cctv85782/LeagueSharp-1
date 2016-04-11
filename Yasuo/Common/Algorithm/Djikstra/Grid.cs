namespace Yasuo.Common.Algorithm.Djikstra
{
    using System;
    using System.Collections.Generic;
    using System.Drawing.Drawing2D;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    public class Grid
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public Grid(List<Connection> connections, Point startPosition, Point endPosition = null)
        {
            try
            {
                if (connections != null)
                {
                    this.Connections = connections;
                }

                BasePoint = startPosition;
                StartPosition = startPosition.Position;

                EndPoint = endPosition;

                SetPoints(connections);

                Color = System.Drawing.Color.White;

                if (Variables.Debug)
                {
                    Console.WriteLine(@"[GridInfo] Setting up new Grid. Total Connection Amount: " + Connections.Count);
                    Console.WriteLine(@"[GridInfo] Setting up new Grid. Total Point Amount: " + Points.Count);
                }


            }
            catch (Exception ex)
            {
                if (Connections != null)
                {
                    Game.PrintChat("Failed creating a grid with: " + Connections.Count + "Connections");
                }
                Console.WriteLine(@"Exeption: " + ex);
            }
        }

        public Grid()
        {
            
        }

        /// <summary>
        ///     All Connections of the grid
        /// </summary>
        public List<Connection> Connections = new List<Connection>();

        /// <summary>
        ///     All Points inside the grid
        /// </summary>
        public List<Point> Points = new List<Point>();

        /// <summary>
        ///     Point where grid starts
        /// </summary>
        public Point BasePoint { get; private set; }

        /// <summary>
        ///     Point where the grid ends (can be null)
        /// </summary>
        public Point EndPoint { get; set; }

        /// <summary>
        ///     Vector where grid starts
        /// </summary>
        public Vector3 StartPosition { get; private set; }

        private void SetPoints(List<Connection> connections)
        {
            foreach (var connection in connections)
            {
                Points.Add(connection.To);
                Points.Add(connection.From);
            }
        }

        /// <summary>
        ///     Searches for a connection between from and to
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public Connection FindConnection(Point point1, Point point2)
        {
            return this.Connections.FirstOrDefault(connection => connection.From.Equals(point1) && connection.To.Equals(point2));
        }

        /// <summary>
        ///     Searched for all connections that either start or end in the Point around
        /// </summary>
        /// <param name="around"></param>
        /// <returns></returns>
        public List<Connection> FindConnections(Point around)
        {
            return this.Connections.Where(connection => connection.To.Equals(around) || connection.From.Equals(around)).ToList();
        }

        /// <summary>
        ///     Color of the grid (Drawings)
        /// </summary>
        public System.Drawing.Color Color;

        /// <summary>
        ///     Draws the Grid as lines in the world
        /// </summary>
        /// <param name="width"></param>
        public void Draw(int width = 1)
        {
            try
            {
                if (Connections == null || Connections.Count == 0)
                {
                    return;
                }

                foreach (var connection in Connections)
                {
                    //Console.WriteLine(@"Drawing Grid");
                    connection.Draw(width, Color);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
