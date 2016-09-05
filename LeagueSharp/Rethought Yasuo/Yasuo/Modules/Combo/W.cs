namespace Rethought_Yasuo.Yasuo.Modules.Combo
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;

    using RethoughtLib.CastManager.Abstract_Classes;

    using Rethought_Yasuo.Yasuo.Modules.Core.SpellParent;
    using Rethought_Yasuo.Yasuo.Modules.Core.SpellParent.Implementations;

    #endregion

    internal class W : OrbwalkingChild
    {
        #region Fields

        private readonly ISpellIndex spellParent;

        private readonly YasuoW yasuoW;

        #endregion

        #region Constructors and Destructors

        public W(ISpellIndex spellParent, ICastManager castManager)
            : base(castManager)
        {
            this.spellParent = spellParent;

            this.yasuoW = (YasuoW)this.spellParent[SpellSlot.W];
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

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [OnEnable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            throw new NotImplementedException();
        }

        private void GameOnOnUpdate(EventArgs args)
        {
            if (this.Guardians.Select(guardian => guardian.Invoke()).Any(result => result))
            {
                return;
            }
            throw new NotImplementedException();
        }

        #endregion
    }
}