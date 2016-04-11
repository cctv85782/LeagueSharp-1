namespace Yasuo.Common.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Xml.Schema;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;
    using SharpDX.Direct3D9;

    using Yasuo.Common.Algorithm.Djikstra;
    using Yasuo.Common.Algorithm.Media;
    using Yasuo.Common.Objects;

    using Point = Yasuo.Common.Algorithm.Djikstra.Point;

    public class SweepingBladeLogicProvider
    {
        #region Fields

        /// <summary>
        ///     The maximum range of calculating
        /// </summary>
        public float CalculationRange;

        public GridGenerator GridGenerator;

        public List<Point> CurrentPoints = new List<Point>(); 

        public List<Connection> CurrentConnections = new List<Connection>(); 

        public enum PathMode
        {
            TwoDirectional, OneDirectional
        }

        public enum Units
        {
            All, Minions, Champions, Mobs
        }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="calculationRange"></param>
        public SweepingBladeLogicProvider(float calculationRange = 10000)
        {
            CalculationRange = calculationRange;
            Drawing.OnDraw += DrawGrid;
        }

        #endregion

        /// <summary>
        ///     Generates a new Grid
        /// </summary>
        /// <param name="startPosition">from</param>
        /// <param name="endPosition">to</param>
        /// <param name="units">which type of units</param>
        public void GenerateGrid(Vector3 startPosition, Vector3 endPosition, Units units)
        {
            this.Reset();
            
            var unitsToProcess = new List<Obj_AI_Base>();

            switch (units)
            {
                case Units.All:
                    unitsToProcess = GetUnits(startPosition, true, true, true);
                break;
                case Units.Minions:
                    unitsToProcess = GetUnits(startPosition, true, false, false);
                break;
                case Units.Champions:
                    unitsToProcess = GetUnits(startPosition, false, true, false);
                break;
                case Units.Mobs:
                    unitsToProcess = GetUnits(startPosition, false, false, true);
                break;
            }

            if (Variables.Debug)
            {
                Console.WriteLine(@"[SweepingBladeLP] GenerateGrid > Generating Grid");
            }

            if (unitsToProcess.Count > 0)
            {
                GridGenerator = new GridGenerator(unitsToProcess, 1000, endPosition);
            }

            // Generate Basic Grid
            this.GridGenerator?.Generate();
        }

        /// <summary>
        ///     Removes invalid and unused connections and closes all points to the EndPoint
        /// </summary>
        public void FinalizeGrid()
        {
            GridGenerator.RemoveDisconnectedConnections();
            GridGenerator.ConnectAllPoints();
        }

        /// <summary>
        ///     Returns a path object that represents the shortest possible path to a given location
        /// </summary>
        /// <param name="endPosition">The vector to dash to</param>
        /// <param name="minions">dash over minions</param>
        /// <param name="champions">dash over champions</param>
        /// <param name="aroundSkillshots">dash around skillshots</param>
        // ReSharper disable once FunctionComplexityOverflow
        public Path GetPath(Vector3 endPosition, bool minions = true, bool champions = true, bool aroundSkillshots = false)
        {
            try
            {
                #region Calculator (Djikstra Algorithm)

                if (Variables.Debug)
                {
                    Console.WriteLine(@"[SweepingBladeLP] Getpath > Calculating Shortest Path");
                }

                if (GridGenerator.Grid == null || GridGenerator.Grid.Connections.Count == 0)
                {
                    if (Variables.Debug)
                    {
                        Console.WriteLine(@"[SweepingBladeLP] Getpath > Returned");
                    }

                    return null;
                }

                // Inputing the grid
                var calculator = new Dijkstra(GridGenerator.Grid);

                calculator.SetStart(GridGenerator.Grid.BasePoint);

                // Set end point and return result as path
                var points = calculator.GetPointsTo(this.GridGenerator.Grid.EndPoint);
                CurrentPoints = points;

                if (Variables.Debug)
                {
                    Console.WriteLine(@"[SweepingBladeLP] Getpath > Result Djikstra Algorithn: " + points.Count);
                }

                #endregion


                #region Converter (Temp-Solution)

                var connections = new List<Connection>();

                for (int i = 0; i < points.Count - 1; i++)
                {
                    var from = points[i];
                    var to = points[i + 1];

                    connections.Add(GridGenerator.Grid.FindConnection(from, to));
                }
                
                CurrentConnections = connections;

                if (Variables.Debug)
                {
                    Console.WriteLine(@"[SweepingBladeLP] GetPath > CurrentConnections.Count: " + CurrentConnections.Count);
                }

                #endregion


                if (CurrentConnections.Count > 0)
                {
                    return new Path(CurrentConnections);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"[GetPath]: " + ex);
            }
            return null;
        }

        private void DrawGrid(EventArgs args)
        {
            if (Variables.Debug)
            {
                GridGenerator?.Grid?.Draw();

                if (CurrentConnections != null)
                {
                    foreach (var connection in CurrentConnections)
                    {
                        if (connection.Over != null)
                        {
                            connection.Draw(3, System.Drawing.Color.Red);
                        }

                        if (connection.Over == null)
                        {
                            connection.Draw(3, System.Drawing.Color.Green);

                        }
                    }
                }

                if (CurrentPoints != null)
                {
                    for (int i = 0; i < CurrentPoints.Count; i++)
                    {
                        var point = CurrentPoints[i];
                        Drawing.DrawText(Drawing.WorldToScreen(point.Position).X, Drawing.WorldToScreen(point.Position).Y, System.Drawing.Color.Red, i.ToString());
                    }
                }

                var point2 = GridGenerator?.Grid?.BasePoint;

                point2?.Draw(60, 5, System.Drawing.Color.Violet);
            }
        }

        /// <summary>
        ///     Returns a list containing all units
        /// </summary>
        /// <param name="startPosition">start point (vector)</param>
        /// <param name="minions">bool</param>
        /// <param name="champions">bool</param>
        /// <returns>List(Obj_Ai_Base)</returns>
        public List<Obj_AI_Base> GetUnits(Vector3 startPosition, bool minions = true, bool champions = true, bool mobs = true)
        {
            try
            {
                // Add all units
                var units = new List<Obj_AI_Base>();

                if (minions)
                {
                    units.AddRange(
                        SebbyLib.Cache.GetMinions(startPosition, CalculationRange, MinionTeam.NotAlly));
                }

                if (champions)
                {
                    units.AddRange(HeroManager.Enemies.Where(x => x.Distance(startPosition) <= CalculationRange));
                }

                foreach (var x in
                    units.Where(
                        x =>
                        !x.IsValid || x.HasBuff("YasuoDashWrapper") || x.IsDead || x.Health == 0 || x.IsMe
                        || x.Distance(Variables.Player.ServerPosition) > CalculationRange).ToList())
                {
                    units.Remove(x);
                }

                return units;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }

        // TODO: HIGH PRIOTIRY
        /// <summary>
        ///     Returns Sweeping Blade speed
        /// </summary>
        /// <returns></returns>
        public float Speed()
        {
            return Variables.Player.MoveSpeed + 100;
        }

        // TODO: HIGH PRIORITY
        /// <summary>
        ///     Credits: Brian
        ///     Returns the correct amount of Damage on unit
        /// </summary>
        public double GetDamage(Obj_AI_Base unit)
        {
            return Variables.Player.CalcDamage(
                unit,
                Damage.DamageType.Magical,
                (50 + 20 * Variables.Spells[SpellSlot.E].Level)
                * (1 + Math.Max(0, Variables.Player.GetBuffCount("YasuoDashScalar") * 0.25))
                + 0.6 * Variables.Player.FlatMagicDamageMod);
        }

        private void Reset()
        {
            try
            {
                if (GridGenerator == null)
                {
                    return;
                }

                GridGenerator.Grid = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}