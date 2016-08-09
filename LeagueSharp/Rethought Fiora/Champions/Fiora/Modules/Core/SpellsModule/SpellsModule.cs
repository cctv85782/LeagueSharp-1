namespace Rethought_Fiora.Champions.Fiora.Modules.Core.SpellsModule
{
    #region Using Directives

    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class SpellsModule : ChildBase
    {
        #region Static Fields

        /// <summary>
        ///     The spells
        /// </summary>
        public Dictionary<SpellSlot, Spell> Spells;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Spells";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [on load event].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            this.Spells = new Dictionary<SpellSlot, Spell>();

            var q = new Spell(SpellSlot.Q, 400 + 350);
            q.SetSkillshot(0.25f, 0, 500, false, SkillshotType.SkillshotLine);

            var w = new Spell(SpellSlot.W, 750);
            w.SetSkillshot(0.5f, 70, 3200, false, SkillshotType.SkillshotLine);

            var e = new Spell(SpellSlot.E);

            var r = new Spell(SpellSlot.R, 500);
            r.SetTargetted(0.066f, 500);

            this.Spells.Add(q.Slot, q);
            this.Spells.Add(w.Slot, w);
            this.Spells.Add(e.Slot, e);
            this.Spells.Add(r.Slot, r);
        }

        #endregion
    }
}