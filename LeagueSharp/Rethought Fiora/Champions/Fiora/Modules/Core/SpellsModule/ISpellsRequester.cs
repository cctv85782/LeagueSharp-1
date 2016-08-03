namespace Rethought_Fiora.Champions.Fiora.Modules.Core.SpellsModule
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal interface ISpellsRequester
    {
        #region Public Events

        event EventHandler SpellRequested;

        #endregion

        #region Public Properties

        Dictionary<SpellSlot, Spell> Spells { get; set; }

        #endregion
    }
}