namespace Rethought_Irelia.IreliaV1.LastHit
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Irelia.IreliaV1.Spells;

    #endregion

    internal class W : OrbwalkingChild
    {
        #region Fields

        /// <summary>
        /// The irelia w
        /// </summary>
        private readonly IreliaW ireliaW;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="W" /> class.
        /// </summary>
        /// <param name="ireliaW">The dark procession logic.</param>
        public W(IreliaW ireliaW)
        {
            this.ireliaW = ireliaW;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "W";

        #endregion
    }
}