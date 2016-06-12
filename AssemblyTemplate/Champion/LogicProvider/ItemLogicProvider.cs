namespace AssemblyName.Champion.LogicProvider
{
    #region Using Directives

    using LeagueSharp.Common;
    using LeagueSharp.Common.Data;

    #endregion

    /// <summary>
    ///     Logic Provider being responsible for item specific logics
    /// </summary>
    internal class ItemLogicProvider
    {
        #region Fields

        /// <summary>
        ///     The items
        /// </summary>
        public Items.Item Tiamat, Hydra, Shiv, InfinityEdge, TrinityForce, Sheen;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ItemLogicProvider" /> class.
        /// </summary>
        public ItemLogicProvider()
        {
            this.SetItems();
        }

        #endregion

        #region Public Methods and Operators

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the items.
        /// </summary>
        private void SetItems()
        {
            this.Tiamat = ItemData.Tiamat_Melee_Only.GetItem();
            this.Hydra = ItemData.Ravenous_Hydra_Melee_Only.GetItem();
            this.Sheen = ItemData.Sheen.GetItem();
            this.TrinityForce = ItemData.Trinity_Force.GetItem();
            this.Shiv = ItemData.Statikk_Shiv.GetItem();
            this.InfinityEdge = ItemData.Infinity_Edge.GetItem();
        }

        #endregion
    }
}