﻿namespace RethoughtLib.Algorithm.Djikstra.PathTypes
{
    #region Using Directives

    using System.Collections.Generic;

    using LeagueSharp.Common;

    using RethoughtLib.Algorithm.Djikstra.ConnectionTypes;

    #endregion

    public abstract class PathBaseClass<T, TV>
        where TV : Connection<T>
    {
        #region Fields

        /// <summary>
        ///     All connections
        /// </summary>
        public List<TV> Connections = new List<TV>();

        /// <summary>
        ///     Where the Path ends
        /// </summary>
        public T EndPosition;

        /// <summary>
        ///     Polygon that represents the PathBaseClass
        /// </summary>
        public Geometry.Polygon GeometryPath;

        /// <summary>
        ///     Where the Path starts
        /// </summary>
        public T StartPosition;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PathBaseClassClass{T,TV}" /> class.
        /// </summary>
        protected PathBaseClass()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PathBaseClassClass{T,TV}" /> class.
        /// </summary>
        /// <param name="connections">The connections.</param>
        protected PathBaseClass(List<TV> connections)
        {
            this.Connections = connections;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     The total time of executing the PathBaseClass
        /// </summary>
        public float PathCost { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Draws the path.
        /// </summary>
        /// <param name="multicolor">if set to <c>true</c> [multicolor].</param>
        public abstract void Draw(bool multicolor = true);

        /// <summary>
        /// Removes the connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public virtual void RemoveConnection(TV connection)
        {
            if (this.Connections.Contains(connection))
            {
                this.Connections.Remove(connection);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the PathBaseClass time.
        /// </summary>
        protected virtual void SetPathCost()
        {
            foreach (var connection in this.Connections)
            {
                this.PathCost += connection.Cost;
            }
        }

        #endregion
    }
}