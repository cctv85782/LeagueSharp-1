﻿namespace Yasuo.CommonEx.Algorithm.Djikstra.ConnectionTypes
{
    #region Using Directives

    using global::Yasuo.CommonEx.Algorithm.Djikstra.PointTypes;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Dash = Objects.Dash;

    #endregion

    public class YasuoBlinkConnection : ConnectionBase<Point>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Constructor for a connection
        /// </summary>
        /// <param name="start">start point</param>
        /// <param name="end">end point</param>
        /// <param name="unit"></param>
        public YasuoBlinkConnection(Point start, Point end, Obj_AI_Hero unit)
        {
            this.Start = start;
            this.End = end;
            this.Unit = unit;

            this.Cost = GlobalVariables.Spells[SpellSlot.R].Speed;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the unit.
        /// </summary>
        /// <value>
        ///     The unit.
        /// </value>
        public Obj_AI_Hero Unit { get; set; }

        #endregion
    }
}