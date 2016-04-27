namespace Yasuo.Common.Algorithm.Djikstra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // TODO: Move away from a point system. On the other side it makes things easy.. 
    // TODO: Take Speed of Connection in Consideration
    // Just going to completely rewrite that shit

    /// <summary>
    ///     Shortest Path Algorithm
    /// </summary>
    internal class Dijkstra
    {
        #region Fields

        public List<Point> Base = new List<Point>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Constructor
        /// </summary>
        public Dijkstra(Grid grid)
        {
            try
            {
                this.Connections = grid.Connections;
                //this.Base = grid.Points;

                foreach (var connection in Connections)
                {
                    if (!Base.Contains(connection.From))
                    {
                        Base.Add(connection.From);
                    }

                    if (!Base.Contains(connection.To))
                    {
                        Base.Add(connection.To);
                    }
                }

                this.Speed = new Dictionary<Point, float>();
                this.Previous = new Dictionary<Point, Point>();

                foreach (var point in this.Base.Where(x => !this.Speed.ContainsKey(x)))
                {
                    this.Speed.Add(point, float.MaxValue);
                    this.Previous.Add(point, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"[Error while using Djikstra Algorithm]: " + ex);
            }
        }

        #endregion

        #region Public Properties

        public List<Connection> Connections { get; set; }

        public Dictionary<Point, Point> Previous { get; set; }

        public Dictionary<Point, float> Speed { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gives all neighbors that are still in the base
        /// </summary>
        /// <param name="point">Points</param>
        /// <returns></returns>
        public List<Point> GetNeighbors(Point point)
        {
            var neighbors = new List<Point>();

            foreach (var connection in this.Connections)
            {
                if (connection.From.Equals(point) && this.Base.Contains(point))
                {
                    neighbors.Add(connection.To);
                }
            }

            return neighbors;
        }

        /// <summary>
        ///     Gets the Point with the shortest distance
        /// </summary>
        /// <returns></returns>
        public Point GetPointSmallestDistance()
        {
            var speed = float.MaxValue;
            Point smallest = null;

            foreach (var n in this.Base)
            {
                if (this.Speed[n] < speed)
                {
                    speed = this.Speed[n];
                    smallest = n;
                }
            }

            return smallest;
        }

        /// <summary>
        ///     Calculates the Path to the Point d (d = target)
        /// </summary>
        /// <param name="point">Targeted Point</param>
        /// <returns>point path</returns>
        public List<Point> GetPointsTo(Point point)
        {
            var path = new List<Point>();

            path.Insert(0, point);

            while (this.Previous[point] != null)
            {
                point = this.Previous[point];
                path.Insert(0, point);
            }

            return path;
        }

        /// <summary>
        ///     Gives the distance between 2 Points
        /// </summary>
        /// <param name="point1">Start Point</param>
        /// <param name="point2">End Point</param>
        /// <returns></returns>
        public float GetValueBetween(Point point1, Point point2)
        {
            foreach (var connection in this.Connections)
            {
                if (connection.From.Equals(point1) && connection.To.Equals(point2))
                {
                    return connection.Time;
                }
            }

            return 0;
        }

        /// <summary>
        ///     Calculates the shortest distance from the Start Point to all other Points
        /// </summary>
        /// <param name="start">Startknoten</param>
        public void SetStart(Point start)
        {
            this.Speed[start] = 0;

            // while we have points to process
            while (this.Base.Count > 0)
            {
                var point = this.GetPointSmallestDistance();

                if (point == null)
                {
                    this.Base.Clear();
                }

                else
                {
                    foreach (var neighbour in this.GetNeighbors(point))
                    {
                        var threshholderDistance = this.Speed[point] + this.GetValueBetween(point, neighbour);

                        if (threshholderDistance < this.Speed[neighbour])
                        {
                            this.Speed[neighbour] = threshholderDistance;
                            this.Previous[neighbour] = point;
                        }
                    }
                    this.Base.Remove(point);
                }
            }
        }

        #endregion
    }
}