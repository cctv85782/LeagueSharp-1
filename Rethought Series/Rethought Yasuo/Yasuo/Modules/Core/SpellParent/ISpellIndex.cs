namespace Rethought_Yasuo.Yasuo.Modules.Core.SpellParent
{
    #region Using Directives

    using LeagueSharp;

    #endregion

    internal interface ISpellIndex
    {
        #region Public Indexers

        /// <summary>
        ///     Gets or sets the <see cref="SpellChild" /> with the specified spell slot.
        /// </summary>
        /// <value>
        ///     The <see cref="SpellChild" />.
        /// </value>
        /// <param name="spellSlot">The spell slot.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Can't return a SpellChild for a SpellSlot that is non-existing</exception>
        SpellChild this[SpellSlot spellSlot] { get; }

        #endregion
    }
}