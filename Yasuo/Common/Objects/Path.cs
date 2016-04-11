// TODO: Add Dash End Positions as list. Maybe think about positive things when I change Obj_AI_Base to Connection or Point.
// TODO: Rework Calculations based on Dash End Positions.

namespace Yasuo.Common.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Yasuo.Common.Algorithm.Djikstra;
    using Yasuo.Common.Provider;

    using Color = System.Drawing.Color;

    public class Path
    {
        #region Fields

        /// <summary>
        ///     All connections
        /// </summary>
        public List<Connection> Connections = new List<Connection>();

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
        ///     Color of the dashes
        /// </summary>
        public System.Drawing.Color DashColor = System.Drawing.Color.White;

        /// <summary>
        ///     Color of the walking pathes
        /// </summary>
        public System.Drawing.Color WalkColor = System.Drawing.Color.White;

        /// <summary>
        ///     Width of dashes (Drawings)
        /// </summary>
        public int DashLineWidth = 1;

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
        ///     Constructor that creates an empty path
        /// </summary>
        public Path()
        {
        }

        /// <summary>
        ///     Constructor that creates a new valid path object
        /// </summary>
        /// <param name="connections">All dashes and walking paths</param>
        public Path(List<Connection> connections)
        {
            try
            {
                this.providerFlow = new FlowLogicProvider();

                foreach (var connection in connections)
                {
                    if (connection != null)
                    {
                        Connections.Add(connection);
                    }
                }

                foreach (var connection in Connections)
                {
                    Units.Add(connection.Over);
                }

                this.SetDashLength();
                this.SetWalkLength();
                this.SetPathLengtht();
                this.SetDashTime();
                this.SetWalkTime();
                this.SetPathTime();

                this.CheckForShield();

                if (Variables.Debug)
                {
                    Drawing.DrawText(500, 520, Color.Red, "PathTime: " + PathTime);
                    Drawing.DrawText(500, 540, Color.Red, "PathLength: " + PathLenght);
                    Drawing.DrawText(500, 560, Color.Red, "DashLengt: " + DashLenght);
                    Drawing.DrawText(500, 580, Color.Red, "WalkLenght: " + WalkLenght);
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

        public void Draw(bool multicolor = true)
        {
            try
            {
                if (multicolor)
                {
                    foreach (var connection in Connections)
                    {
                        if (connection.IsDash)
                        {
                            connection.Draw(DashLineWidth, DashColor);
                        }
                        else
                        {
                            connection.Draw(WalkLineWidth, WalkColor);
                        }
                    }
                }
                else
                {
                    foreach (var connection in Connections)
                    {
                        connection.Draw(2, Color.White);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Drawing path expection :" + ex);
            }
        }

        public void RemoveConnection(Connection connection)
        {
            if (Connections.Contains(connection))
            {
                Connections.Remove(connection);
            }

            if (Units.Contains(connection.Over))
            {
                Units.Remove(connection.Over);
            }
        }

        #endregion

        #region Methods

        private void CheckForShield()
        {
            if (PathLenght >= this.providerFlow.GetRemainingUnits())
            {
                this.BuildsUpShield = true;
            }
            else
            {
                this.BuildsUpShield = false;
            }
        }

        // TODO: PRIRORITY MEDIUM - LOW > Add Skillshots in Path (Based on Danger Level)
        private void SetDangerValue()
        {
            foreach (var unit in this.Units.Where(x => x.CountEnemiesInRange(Variables.Spells[SpellSlot.E].Range) > 0))
            {
                foreach (var hero in HeroManager.Enemies.Where(y => y.Distance(unit) <= Variables.Spells[SpellSlot.E].Range))
                {
                    this.DangerValue += (int)TargetSelector.GetPriority(hero);
                }
                this.DangerValue += 1;
            }
        }

        private void SetDashLength()
        {
            foreach (var connection in this.Connections.Where(connection => connection.IsDash))
            {
                this.DashLenght += connection.Lenght;
            }
        }

        private void SetDashTime()
        {
            foreach (var connection in Connections.Where(connection => connection.IsDash))
            {
                this.DashTime += connection.Time;
            }
        }

        private void SetPathLengtht()
        {
            this.PathLenght = this.WalkLenght + this.DashLenght;
        }

        private void SetPathTime()
        {
            this.PathTime = this.WalkTime + this.DashTime;
        }

        private void SetWalkLength()
        {
            foreach (var connection in Connections.Where(connection => !connection.IsDash))
            {
                this.WalkLenght += connection.Lenght;
            }
        }

        private void SetWalkTime()
        {
            foreach (var connection in Connections.Where(connection => !connection.IsDash))
            {
                this.WalkTime += connection.Time;
            }
        }

        #endregion
    }
}