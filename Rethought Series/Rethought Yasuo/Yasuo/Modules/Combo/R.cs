﻿namespace Rethought_Yasuo.Yasuo.Modules.Combo
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.ActionManager.Abstract_Classes;

    using Rethought_Yasuo.Yasuo.Modules.Core.SpellParent;
    using Rethought_Yasuo.Yasuo.Modules.Core.SpellParent.Implementations;

    #endregion

    internal class R : OrbwalkingChild
    {
        #region Fields

        private readonly ISpellIndex spellParent;

        private readonly YasuoR yasuoR;

        #endregion

        #region Constructors and Destructors

        public R(ISpellIndex spellParent, IActionManager actionManager)
            : base(actionManager)
        {
            this.spellParent = spellParent;
            this.yasuoR = (YasuoR)this.spellParent[SpellSlot.R];
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "R";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Orbwalking.AfterAttack -= this.OrbwalkingAfterAttack;
        }

        /// <summary>
        ///     Called when [OnEnable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Orbwalking.AfterAttack += this.OrbwalkingAfterAttack;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            throw new NotImplementedException();
        }

        private void OrbwalkingAfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (this.Guardians.Select(guardian => guardian.Invoke()).Any(result => result))
            {
                return;
            }
        }

        #endregion
    }
}