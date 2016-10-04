namespace Rethought_Yasuo.Yasuo.Modules.Guardians
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Rethought_Yasuo.Yasuo.Modules.Core.SpellParent;

    #endregion

    internal class SpellMustBeReady : IGuardian
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellMustBeReady"/> class.
        /// </summary>
        /// <param name="spellModule">The spells module.</param>
        /// <param name="spellSlot">The spell slot.</param>
        public SpellMustBeReady(ISpellIndex spellModule, SpellSlot spellSlot)
        {
            this.Func = () => !spellModule[spellSlot].Spell.IsReady();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the function.
        /// </summary>
        /// <value>
        ///     The function.
        /// </value>
        public Func<bool> Func { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Checks the function.
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            return this.Func.Invoke();
        }

        #endregion
    }
}