namespace AssemblyName.MediaLib.Algorithm.Djikstra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SharpDX;

    using Color = System.Drawing.Color;

    public class Grid
    {
        #region Fields

        /// <summary>
        ///     Color of the grid (Drawings)
        /// </summary>
        public Color Color;

        /// <summary>
        ///     All Connections of the grid
        /// </summary>
        public List<Connection> Connections = new List<Connection>();

        /// <summary>
        ///     All Points inside the grid
        /// </summary>
        public List<Point> Points = new List<Point>();

        #endregion

        #region Constructors and Destructors

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

                this.BasePoint = startPosition;
                this.StartPosition = startPosition.Position;

                this.EndPoint = endPosition;

                this.SetPoints(connections);

                this.Color = Color.White;

                if (GlobalVariables.Debug)
                {
                    Console.WriteLine(@"[GridInfo] Setting up new Grid. Total Connection Amount: " + this.Connections.Count);
                    Console.WriteLine(@"[GridInfo] Setting up new Grid. Total Point Amount: " + this.Points.Count);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Exeption: " + ex);
            }
        }

        #endregion

        #region Public Properties

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

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Draws the Grid as lines in the world
        /// </summary>
        /// <param name="width"></param>
        public void Draw(int width = 1)
        {
            try
            {
                if (this.Connections == null || this.Connections.Count == 0)
                {
                    return;
                }

                foreach (var connection in this.Connections)
                {
                    //Console.WriteLine(@"Drawing Grid");
                    connection.Draw(true, width, this.Color);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
            return
                this.Connections.FirstOrDefault(
                    connection => connection.From.Equals(point1) && connection.To.Equals(point2));
        }

        /// <summary>
        ///     Searched for all connections that either start or end in the Point around
        /// </summary>
        /// <param name="around"></param>
        /// <returns></returns>
        public List<Connection> FindConnections(Point around)
        {
            return
                this.Connections.Where(connection => connection.To.Equals(around) || connection.From.Equals(around))
                    .ToList();
        }

        #endregion

        #region Methods

        private void SetPoints(List<Connection> connections)
        {
            foreach (var connection in connections)
            {
                this.Points.Add(connection.To);
                this.Points.Add(connection.From);
            }
        }

        #endregion
    }
}