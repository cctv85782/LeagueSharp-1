// TODO: Add Dash End Positions as list. Maybe think about positive things when I change Obj_AI_Base to Connection or Point.
// TODO: Rework Calculations based on Dash End Positions.

namespace Yasuo.CommonEx.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Yasuo.CommonEx.Algorithm.Djikstra;
    using global::Yasuo.Yasuo.LogicProvider;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Color = System.Drawing.Color;

    public class Path
    {
        #region Fields

        /// <summary>
        ///     Color of the circles
        /// </summary>
        public Color CircleColor = Color.White;

        /// <summary>
        ///     Width of the circles (Drawings)
        /// </summary>
        public int CircleLineWidth = 1;

        /// <summary>
        ///     Radius of the circles (Drawings)
        /// </summary>
        public int CircleRadius = 40;

        /// <summary>
        ///     All connections
        /// </summary>
        public List<Connection> Connections = new List<Connection>();

        /// <summary>
        ///     Color of the dashes
        /// </summary>
        public Color DashColor = Color.White;

        /// <summary>
        ///     Width of dashes (Drawings)
        /// </summary>
        public int DashLineWidth = 1;

        /// <summary>
        ///     Where the path ends
        /// </summary>
        public Vector3 EndPosition;

        /// <summary>
        ///     Polygon that represents the path
        /// </summary>
        public Geometry.Polygon GeometryPath;

        /// <summary>
        ///     Where the path starts
        /// </summary>
        public Vector3 StartPosition;

        /// <summary>
        ///     All units
        /// </summary>
        public List<Obj_AI_Base> Units = new List<Obj_AI_Base>();

        /// <summary>
        ///     Color of the walking pathes
        /// </summary>
        public Color WalkColor = Color.White;

        /// <summary>
        ///     Width of lines (Drawings)
        /// </summary>
        public int WalkLineWidth = 1;

        /// <summary>
        ///     Provider for Flow logics
        /// </summary>
        private readonly FlowLogicProvider providerFlow;

        #endregion

        #region Constructors and Destructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Path"/> class.
        /// </summary>
        public Path()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Path"/> class.
        /// </summary>
        /// <param name="connections">The connections.</param>
        public Path(List<Connection> connections)
        {
            try
            {
                this.providerFlow = new FlowLogicProvider();

                foreach (var connection in connections)
                {
                    if (connection != null)
                    {
                        this.Connections.Add(connection);
                    }
                }

                foreach (var connection in this.Connections)
                {
                    this.Units.Add(connection.Unit);
                }

                this.SetDashLength();
                this.SetWalkLength();
                this.SetPathLengtht();
                this.SetDashTime();
                this.SetWalkTime();
                this.SetPathTime();

                this.CheckForShields();

                if (GlobalVariables.Debug)
                {
                    Drawing.DrawText(500, 520, Color.Red, "PathTime: " + this.PathTime);
                    Drawing.DrawText(500, 540, Color.Red, "PathLength: " + this.PathLenght);
                    Drawing.DrawText(500, 560, Color.Red, "DashLengt: " + this.DashLenght);
                    Drawing.DrawText(500, 580, Color.Red, "WalkLenght: " + this.WalkLenght);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #endregion

        #region Public Properties

        // TODO: PRIORITY LOW
        /// <summary>
        ///     Bool that represents if the path will build up the Yasuo Shield if it is finished
        /// </summary>
        public bool BuildsUpShield { get; private set; }

        /// <summary>
        ///     The average danger value of the path
        /// </summary>
        public int DangerValue { get; private set; }

        /// <summary>
        ///     The total lenght of all dashes
        /// </summary>
        public float DashLenght { get; private set; }

        /// <summary>
        ///     Time that is needed to execute all dashes
        /// </summary>
        public float DashTime { get; private set; }

        /// <summary>
        ///     The total path lenght
        /// </summary>
        public float PathLenght { get; private set; }

        /// <summary>
        ///     The total time of executing the path
        /// </summary>
        public float PathTime { get; private set; }

        /// <summary>
        ///     The total walk lenght
        /// </summary>
        public float WalkLenght { get; private set; }

        /// <summary>
        ///     The total time of walking
        /// </summary>
        public float WalkTime { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void Draw(bool multicolor = true, bool circles = true)
        {
            try
            {
                if (multicolor)
                {
                    foreach (var connection in this.Connections)
                    {
                        if (connection.IsDash)
                        {
                            connection.Draw(false, this.DashLineWidth, this.DashColor);

                            if (circles)
                            {
                                connection.From.Draw(this.CircleRadius, this.CircleLineWidth, this.CircleColor);
                                connection.To.Draw(this.CircleRadius, this.CircleLineWidth, this.CircleColor);
                            }
                        }
                        else
                        {
                            connection.Draw(false, this.WalkLineWidth, this.WalkColor);
                        }
                    }
                }
                else
                {
                    foreach (var connection in this.Connections)
                    {
                        connection.Draw(false, 2, Color.White);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Drawing path expection :" + ex);
            }
        }

        /// <summary>
        ///     Gets a position after time following the path
        /// </summary>
        /// <param name="time"></param>
        /// <returns>A vector</returns>
        public Vector3 GetPosition(float time)
        {
            var result = new Vector3();

            var time2 = time;

            foreach (var connection in this.Connections)
            {
                if (time2 > 0)
                {
                    time2 -= connection.Time;
                }

                if (time <= 0)
                {
                    if (connection.IsDash)
                    {
                        return connection.To.Position;
                    }
                    else
                    {
                        var minusTime = time * -1;

                        return connection.From.Position.Extend(
                            connection.To.Position,
                            (GlobalVariables.Player.MoveSpeed * 1000) / minusTime);
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     Removes a connection from the path
        /// </summary>
        /// <param name="connection"></param>
        public void RemoveConnection(Connection connection)
        {
            if (this.Connections.Contains(connection))
            {
                this.Connections.Remove(connection);
            }

            if (this.Units.Contains(connection.Unit))
            {
                this.Units.Remove(connection.Unit);
            }
        }

        /// <summary>
        ///     Splits the path logical into multiple segments
        /// </summary>
        /// <param name="segmentAmount">Amount of vectors</param>
        /// <returns>A vector-list</returns>
        public List<Vector3> SplitIntoVectors(float segmentAmount = 100)
        {
            var result = new List<Vector3>((int)segmentAmount);

            foreach (var connection in this.Connections)
            {
                if (connection.IsDash)
                {
                    if (!result.Contains(connection.From.Position))
                    {
                        result.Add(connection.From.Position);
                    }
                    if (!result.Contains(connection.To.Position))
                    {
                        result.Add(connection.To.Position);
                    }
                }
            }

            if (result.Count() < 100)
            {
                var nonDashConnections = this.Connections.Where(x => !x.IsDash).ToList();

                var segmentsPerConnection = segmentAmount / nonDashConnections.Count();

                if (segmentsPerConnection >= 1)
                {
                    var vectorsToAdd = new List<Vector3>((int)segmentsPerConnection);

                    foreach (var connection in nonDashConnections)
                    {
                        var steps = (int)connection.Lenght / (int)segmentsPerConnection;

                        vectorsToAdd.Add(connection.From.Position);

                        for (var i = 0; i < segmentsPerConnection; i += steps)
                        {
                            var vec = connection.From.Position.Extend(connection.To.Position, steps);
                            vectorsToAdd.Add(vec);
                        }
                    }

                    foreach (var vectors in vectorsToAdd)
                    {
                        if (!result.Contains(vectors))
                        {
                            result.Add(vectors);
                        }
                    }
                }
                else if (segmentsPerConnection < 1 && segmentsPerConnection > 0)
                {
                    var lastOrDefault = nonDashConnections.LastOrDefault();
                    var vector = new Vector3();

                    if (lastOrDefault != null)
                    {
                        vector = lastOrDefault.To.Position;
                    }

                    if (!result.Contains(vector))
                    {
                        result.Add(vector);
                    }
                }
            }

            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Checks for shields.
        /// </summary>
        private void CheckForShields()
        {
            if (this.PathLenght >= this.providerFlow.GetRemainingUnits())
            {
                this.BuildsUpShield = true;
            }
            else
            {
                this.BuildsUpShield = false;
            }
        }

        // TODO: PRIRORITY MEDIUM - LOW > Add Skillshots in Path (Based on Danger Level)
        /// <summary>
        ///     Sets the danger value.
        /// </summary>
        private void SetDangerValue()
        {
            foreach (
                var unit in this.Units.Where(x => x.CountEnemiesInRange(GlobalVariables.Spells[SpellSlot.E].Range) > 0))
            {
                foreach (
                    var hero in
                        HeroManager.Enemies.Where(y => y.Distance(unit) <= GlobalVariables.Spells[SpellSlot.E].Range))
                {
                    this.DangerValue += (int)TargetSelector.GetPriority(hero);
                }
                this.DangerValue += 1;
            }
        }

        /// <summary>
        ///     Sets the length of the dash.
        /// </summary>
        private void SetDashLength()
        {
            foreach (var connection in this.Connections.Where(connection => connection.IsDash))
            {
                this.DashLenght += connection.Lenght;
            }
        }

        /// <summary>
        ///     Sets the dash time.
        /// </summary>
        private void SetDashTime()
        {
            foreach (var connection in this.Connections.Where(connection => connection.IsDash))
            {
                this.DashTime += connection.Time;
            }
        }

        /// <summary>
        ///     Sets the path lengtht.
        /// </summary>
        private void SetPathLengtht()
        {
            this.PathLenght = this.WalkLenght + this.DashLenght;
        }

        /// <summary>
        ///     Sets the path time.
        /// </summary>
        private void SetPathTime()
        {
            this.PathTime = this.WalkTime + this.DashTime;
        }

        /// <summary>
        ///     Sets the length of the walk.
        /// </summary>
        private void SetWalkLength()
        {
            foreach (var connection in this.Connections.Where(connection => !connection.IsDash))
            {
                this.WalkLenght += connection.Lenght;
            }
        }

        /// <summary>
        ///     Sets the walk time.
        /// </summary>
        private void SetWalkTime()
        {
            foreach (var connection in this.Connections.Where(connection => !connection.IsDash))
            {
                this.WalkTime += connection.Time;
            }
        }

        #endregion
    }
}