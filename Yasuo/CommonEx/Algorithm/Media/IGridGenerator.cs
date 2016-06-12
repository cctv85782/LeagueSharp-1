namespace Yasuo.CommonEx.Algorithm.Media
{
    #region Using Directives

    using System.Collections.Generic;

    using global::Yasuo.CommonEx.Algorithm.Djikstra;

    using LeagueSharp;

    using SharpDX;

    #endregion

    internal interface IGridGenerator
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        Vector3 From { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        Vector3 To { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Generates this instance.
        /// </summary>
        /// <returns></returns>
        Grid Generate();

        /// <summary>
        ///     Updates the specified settings.
        /// </summary>
        /// <param name="units">The units.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        void Update(List<Obj_AI_Base> units, Vector3 from, Vector3 to);

        #endregion
    }
}