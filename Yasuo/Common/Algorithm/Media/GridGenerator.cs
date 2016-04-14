namespace Yasuo.Common.Algorithm.Media
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.SDK;

    using SharpDX;

    using Yasuo.Common.Algorithm.Djikstra;
    using Yasuo.Common.Objects;

    using Geometry = LeagueSharp.Common.Geometry;
    using Point = Yasuo.Common.Algorithm.Djikstra.Point;
    using Variables = Yasuo.Variables;

    // TODO: PRIORITY LOW > Adding offset, different dash types - to make the logic behind this usable for every other dash in the game too
    // TODO: PRIORITY LOW > Making GridGenerator universal

    /// <summary>
    ///     Grid or "Connection" Generator: http://i.imgur.com/XomUJvK.png
    /// </summary>
    public class GridGenerator
    {
        #region Fields

        /// <summary>
        ///     The point where you start everything from. Usually the player position.
        /// </summary>
        public Point BasePoint = new Point(Variables.Player.ServerPosition);

        /// <summary>
        ///     All possible pathes
        /// </summary>
        public Grid Grid;

        /// <summary>
        ///     Thresholder
        ///     Every connection that got created
        /// </summary>
        public List<Connection> SharedConnections = new List<Connection>();

        /// <summary>
        ///     Every point is definitely a dash end position and a possible dash start position
        /// </summary>
        public List<Point> SharedPoints = new List<Point>();

        private enum DashRange
        {
            Fixed, Dynamic
        }

        private enum DashType
        {
            Unit, Dynamic
        }

        private enum UnitType
        {
            All, Allied, NotAllied, Enemy, NotAllyForEnemy, Neutral
        }

        #endregion

        #region Constructors and Destructors

        // TODO: Add HealthPredictions on Minions/Units

        /// <summary>
        ///     Intializes the grid generator and creates a new thread
        /// </summary>
        /// <param name="units">units that get taken into calculations</param>
        /// <param name="maxConnections">maximum amount of connections</param>
        /// <param name="endPosition">Information for the grid</param>
        internal GridGenerator(List<Obj_AI_Base> units, int maxConnections, Vector3 endPosition)
        {
            Units = units;

            if (Units.Contains(Variables.Player))
            {
                Units.Remove(Variables.Player);
            }

            // Setting default settings
            MaxConnections = maxConnections;
            PathDeepness = 20;

            EndPosition = endPosition;
            EndPoint = new Point(EndPosition);
        }

        #endregion

        #region Public Properties

        public Point EndPoint { get; }

        public Vector3 EndPosition { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///     Maximum amount of connections
        /// </summary>
        internal int MaxConnections { get; }

        /// <summary>
        ///     Maximum amount of dashes per path
        /// </summary>
        internal int PathDeepness { get; }

        /// <summary>
        ///     All units
        /// </summary>
        internal List<Obj_AI_Base> Units { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Generates a new connection for every open connection to "to"
        /// </summary>
        public void ConnectAllPoints()
        {
            try
            {
                if (Variables.Debug)
                {
                    Console.WriteLine(@"GridGenerator.Cs > ConnectAllPoints()");
                }

                foreach (var x in Grid.Connections.ToList())
                {
                    var path = Variables.Player.GetPath(x.To.Position, EndPosition);

                    var firstpoint = x.To;

                    for (var i = 0; i < path.Count() - 1; i++)
                    {
                        var start = new Point(path[i]);

                        if (i == 0)
                        {
                            start = firstpoint;
                        }

                        var end = new Point(path[i + 1]);

                        if (i == path.Count() - 2)
                        {
                            end = EndPoint;
                        }

                        var connection = new Connection(start, end);

                        if (!Grid.Connections.Contains(connection))
                        {
                            Grid.Connections.Add(connection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"[GridManager]: " + ex);
            }
        }

        // TODO: PRIORITY MEDIUM > Walk behind minion to dash over it
        /// <summary>
        ///     Creates a new unique grid for every unit in dash range
        /// </summary>
        public void Generate()
        {
            try
            {
                if (Variables.Debug)
                {
                    Console.WriteLine(@"GridGenerator.Cs > Generate()");
                }

                // Setting up the first points
                foreach (var unit in Units.Where(x => Geometry.Distance(x, BasePoint.Position) <= Variables.Spells[SpellSlot.E].Range))
                {
                    var pointToAdd = new Point(new Dash(Variables.Player.ServerPosition, unit).EndPosition);
                    SharedPoints.Add(pointToAdd);
                    SharedConnections.Add(new Connection(BasePoint, pointToAdd, unit));
                }

                // Connectin StartPoint to EndPoint
                var path2 = Variables.Player.GetPath(BasePoint.Position, EndPosition);

                for (var i = 0; i < path2.Count() - 1; i++)
                {
                    var start = new Point(path2[i]);

                    if (i == 0)
                    {
                        start = BasePoint;
                    }

                    var end = new Point(path2[i + 1]);

                    if (i == path2.Count() - 2)
                    {
                        end = EndPoint;
                    }

                    var connection = new Connection(start, end);

                    if (!SharedConnections.Contains(connection))
                    {
                        SharedConnections.Add(connection);
                    }
                }

                SharedPoints.Add(EndPoint);

                // Starts generating possible pathes
                for (var i = 0; i < PathDeepness; i++)
                {
                    if (!SharedPoints.Any())
                    {
                        break;
                    }

                    foreach (var point in SharedPoints.ToList())
                    {
                        var localBlacklist = Backtrace(point, MaxConnections);

                        var unitCount = this.Units.Where(unit => Geometry.Distance(unit, point.Position) <= Variables.Spells[SpellSlot.E].Range)
                                                     .Count(unit => !localBlacklist.Contains(unit));

                        // Remove point from list and continue because there are no valid dashes available around that point
                        if (unitCount == 0)
                        {
                            if (Variables.Debug)
                            {
                                Console.WriteLine(@"[GridGenerator] Removing Point because no dashes are available for that point anymore");
                            }

                            SharedPoints.Remove(point);
                            continue;
                        }

                        this.ProcessPoint(point, localBlacklist);
                        SharedPoints.Remove(point);
                    }
                }

                Grid = new Grid(SharedConnections, BasePoint, EndPoint);
            }
            catch (Exception ex)
            {
                // this.SoftReset();
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        ///     Searches for connections that do not belong to the grid anymore and removes them
        /// </summary>
        public void RemoveDisconnectedConnections()
        {
            if (Variables.Debug)
            {
                Console.WriteLine(@"GridGenerator.Cs > RemoveDisconnectedConnections()");
            }

            var toBeRemoved = new List<Connection>();

            foreach (var connection in Grid?.Connections)
            {
                var backtracedPath = Backtrace(connection.From);

                if (backtracedPath == null || backtracedPath.Count < 0)
                {
                    return;
                }

                var all = backtracedPath.All(x => x.From != this.Grid.BasePoint);

                if (all)
                {
                    toBeRemoved.Add(connection);
                }
            }

            foreach (var connection in toBeRemoved)
            {
                this.Grid?.Connections?.Remove(connection);
            }
        }

        /// <summary>
        ///     Removes a path completely from the grid and all following connections that got impossible to execute too
        /// </summary>
        /// <param name="point"></param>
        public void RemovePath(Point point)
        {
            var toGetRemoved = Backtrace(point);

            foreach (var item in toGetRemoved)
            {
                Grid?.Points?.Remove(item.To);
                Grid?.Connections?.Remove(item);
            }

            this.RemoveDisconnectedConnections();
        }

        /// <summary>
        ///     Removes a path completely from the grid and all following connections that got impossible to execute too
        /// </summary>
        /// <param name="connection"></param>
        public void RemovePath(Connection connection)
        {
            RemovePath(connection.To);
        }

        // TODO PRIORITY: MEDIUM - LOW
        /// <summary>
        ///     Removes every path that intersects with a skillshot
        /// </summary>
        public void RemovePathesThroughSkillshots(List<Skillshot> skillshots)
        {
            if (Variables.Debug)
            {
                Console.WriteLine(@"GridGenerator.Cs > RemovePathesThroughSkillshots()");
            }

            if (Grid.Connections.Count == 0)
            {
                return; 
            }
            var skillshotDict = new Dictionary<Skillshot, Geometry.Polygon>();

            if (skillshots.Count > 0)
            {
                foreach (var skillshot in skillshots)
                {
                    var polygon = new Geometry.Polygon();

                    switch (skillshot.SData.SpellType)
                    {
                        case SpellType.SkillshotLine:
                            polygon = new Geometry.Polygon.Rectangle(
                                skillshot.StartPosition,
                                skillshot.EndPosition,
                                skillshot.SData.Radius);
                            break;
                        case SpellType.SkillshotCircle:
                            polygon = new Geometry.Polygon.Circle(skillshot.EndPosition, skillshot.SData.Radius);
                            break;
                        case SpellType.SkillshotArc:
                            polygon = new Geometry.Polygon.Sector(
                                skillshot.StartPosition,
                                skillshot.Direction,
                                skillshot.SData.Angle,
                                skillshot.SData.Radius);
                            break;
                    }

                    skillshotDict.Add(skillshot, polygon);
                }
            }

            foreach (var skillshot in skillshotDict)
            {
                foreach (var point in Grid.Points)
                {
                    if (skillshot.Value.IsInside(point.Position))
                    {
                        RemovePath(point);
                    }
                }

                foreach (var connection in Grid.Connections)
                {
                    var clipperpath = skillshot.Value.ToClipperPath();
                    var connectionpolygon = new Geometry.Polygon.Line(connection.From.Position, connection.To.Position);
                    var connectionclipperpath = connectionpolygon.ToClipperPath();

                    if (clipperpath.Intersect(connectionclipperpath).Any())
                    {
                        RemovePath(connection);
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns all units from "from" to the "BasePoint"
        ///     Usefull for building Blacklists.
        /// </summary>
        /// <param name="from">the starting point</param>
        /// <param name="limit">limit of backtrace amount</param>
        /// <returns></returns>
        private List<Obj_AI_Base> Backtrace(Point from, int limit)
        {
            try
            {
                var result = new List<Obj_AI_Base>();

                // Thresholder
                var previousPoint = from;

                // limiter is used as a fallback method to prevent game crashes if there is an error somewhere.
                var limiter = 0;

                while (true)
                {
                    limiter++;

                    if (SharedConnections.Count == 0)
                    {
                        break;
                    }

                    // reached starting/base point
                    if (previousPoint == BasePoint)
                    {
                        #region Debug

                        if (Variables.Debug)
                        {
                            Console.WriteLine(@"[BT] FINISHED: Reached Base Point");
                        }

                        #endregion

                        break;
                    }

                    foreach (var x in SharedConnections.ToList())
                    {
                        if (previousPoint.Position == x.To.Position)
                        {
                            //Console.WriteLine(@"[BT] Adding result");
                            result.Add(x.Over);
                            previousPoint = x.From;
                        }
                    }

                    if (limiter == limit)
                    {
                        #region Debug

                        if (Variables.Debug)
                        {
                            Console.WriteLine(@"[BT] FINISHED: Reached Limit");
                        }

                        #endregion

                        break;
                    }
                }

                return result;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        /// <summary>
        ///     Returns all units from "from" to the "BasePoint"
        /// </summary>
        /// <param name="from">the starting point</param>
        /// <returns></returns>
        private List<Connection> Backtrace(Point from)
        {
            try
            {
                var result = new List<Connection>();

                // Thresholder
                var previousPoint = from;

                for (var i = 0; i < Grid.Connections.Count; i++)
                {
                    if (previousPoint.Position == Grid.Connections[i].To.Position)
                    {
                        //Console.WriteLine(@"[BT] Adding result");
                        result.Add(Grid.Connections[i]);
                        previousPoint = Grid.Connections[i].From;
                    }

                    if (1 == Grid.Connections.Count - 1)
                    {
                        return result;
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }

        /// <summary>
        ///     Searches for possible dashes around "point".
        ///     Ignores all units from the blacklist.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="blacklist"></param>
        private void ProcessPoint(Point point, List<Obj_AI_Base> blacklist)
        {
            foreach (
                var unit in
                    this.Units.Where(
                        unit => Geometry.Distance(unit, point.Position) <= Variables.Spells[SpellSlot.E].Range))
            {
                // Blacklist Check
                if (blacklist.Count > 0 && blacklist.Contains(unit))
                {
                    continue;
                }

                // Checking for wall
                var dashObj = new Dash(point.Position, unit);

                var endPoint = new Point(dashObj.EndPosition);

                // Overriding Endpoint. Connection class does not contain any wallcheck. Dash class does.
                var tempConnection = new Connection(point, endPoint, unit) { To = endPoint };

                if (Variables.Debug)
                {
                    Console.WriteLine(@"Adding new Point to SharedPoints: " + endPoint.Position);
                }

                SharedConnections.Add(tempConnection);
            }
        }

        /// <summary>
        ///     Resets all objects to default.
        /// </summary>
        private void Reset()
        {
            SharedConnections = new List<Connection>();
            SharedPoints = new List<Point>();

            Units = new List<Obj_AI_Base>();
            Grid = new Grid(new List<Connection>(), null, null);

            if (Variables.Debug)
            {
                Console.WriteLine(@"[Reset] Reseted");
            }
        }

        /// <summary>
        ///     Resets everything to default but Units and Grids.
        /// </summary>
        private void SoftReset()
        {
            SharedConnections = new List<Connection>();
            SharedPoints = new List<Point>();

            if (Variables.Debug)
            {
                Console.WriteLine(@"[SoftReset] Reseted");
            }
        }

        #endregion
    }
}