namespace Rethought_Yasuo.Yasuo.Modules.Combo
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;

    using RethoughtLib.Algorithm.Dijkstra;
    using RethoughtLib.Algorithm.Dijkstra.ConnectionTypes;
    using RethoughtLib.CastManager.Abstract_Classes;

    using Rethought_Yasuo.Yasuo.Modules.Core.GridGenerator;
    using Rethought_Yasuo.Yasuo.Modules.Core.SpellParent;
    using Rethought_Yasuo.Yasuo.Modules.Core.SpellParent.Implementations;

    using SharpDX;

    #endregion

    internal class E : OrbwalkingChild
    {
        #region Fields

        private readonly IGridGenerator gridGenerator;

        /// <summary>
        ///     The spells module
        /// </summary>
        private readonly ISpellIndex spellParent;

        private readonly YasuoE yasuoE;

        private Dijkstra<Vector2, Edge<Vector2>> dijkstra = null;

        private Graph<Vector2, Edge<Vector2>> grid;

        #endregion

        #region Constructors and Destructors

        // TODO: Make pathfinding algorithm changeable (abstraction, ioC)
        /// <summary>
        ///     Initializes a new instance of the <see cref="E" /> class.
        /// </summary>
        /// <param name="spellParent">The spells module.</param>
        /// <param name="castManager">The cast manager</param>
        /// <param name="gridGenerator">The grid generator used to generate possible pathes</param>
        public E(ISpellIndex spellParent, ICastManager castManager, IGridGenerator gridGenerator)
            : base(castManager)
        {
            this.spellParent = spellParent;
            this.gridGenerator = gridGenerator;

            this.yasuoE = (YasuoE)this.spellParent[SpellSlot.E];
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "E";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="featureBaseEventArgs"></param>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += this.GameOnOnUpdate;
        }

        private void GameOnOnUpdate(EventArgs args)
        {
            if (this.Guardians.Select(guardian => guardian.Invoke()).Any(result => result))
            {
                return;
            }

            this.grid = this.gridGenerator.Generate<Vector2, Edge<Vector2>>();

            if (this.grid != null)
            {
                this.dijkstra = new Dijkstra<Vector2, Edge<Vector2>>(this.grid.Edges);
            }
        }

        #endregion
    }
}