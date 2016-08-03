namespace Rethought_Fiora.Champions.Fiora.Modules.Combo
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Fiora.Champions.Fiora.Modules.Core;
    using Rethought_Fiora.Champions.Fiora.Modules.Core.SpellsModule;
    using Rethought_Fiora.Champions.Fiora.Modules.Core.TargetSelectorModule;

    #endregion

    internal class Q : ChildBase, ITargetRequester, ISpellsRequester
    {
        #region Public Events

        /// <summary>
        ///     Occurs when [spell requested].
        /// </summary>
        public event EventHandler SpellRequested;

        /// <summary>
        ///     Occurs when [target requested].
        /// </summary>
        public event EventHandler<TargetRequestEventArgs> TargetRequested;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Q";

        /// <summary>
        ///     Gets or sets the spells.
        /// </summary>
        /// <value>
        ///     The spells.
        /// </value>
        public Dictionary<SpellSlot, Spell> Spells { get; set; }

        /// <summary>
        ///     Gets or sets the target.
        /// </summary>
        /// <value>
        ///     The target.
        /// </value>
        public Obj_AI_Hero Target { get; set; }

        /// <summary>
        ///     Gets or sets the target retrieve method.
        /// </summary>
        /// <value>
        ///     The target retrieve method.
        /// </value>
        public ITargetRetrieveMethod TargetRetrieveMethod { get; set; } = new QTargetRetrieveMethod();

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

        /// <summary>
        ///     Called when [spell requested].
        /// </summary>
        protected virtual void OnSpellRequested()
        {
            this.SpellRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Called when [target requested].
        /// </summary>
        protected virtual void OnTargetRequested()
        {
            this.TargetRequested?.Invoke(
                this,
                new TargetRequestEventArgs() { TargetRetrieveMethod = this.TargetRetrieveMethod });
        }

        private void GameOnOnUpdate(EventArgs args)
        {
            if (!this.Spells[SpellSlot.Q].IsReady()
                || OrbwalkerModule.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || ObjectManager.Player.IsWindingUp)
            {
                return;
            }
        }

        #endregion
    }

    internal class QTargetRetrieveMethod : ITargetRetrieveMethod, ISpellsRequester
    {
        #region Public Events

        public event EventHandler SpellRequested;

        #endregion

        #region Public Properties

        public Dictionary<SpellSlot, Spell> Spells { get; set; }

        #endregion

        #region Public Methods and Operators

        public Obj_AI_Hero GetTarget()
        {
            if (this.Spells == null)
            {
                this.OnSpellRequested();
            }

            return TargetSelector.GetTarget(this.Spells[SpellSlot.Q].Range, TargetSelector.DamageType.Physical);
        }

        #endregion

        #region Methods

        protected virtual void OnSpellRequested()
        {
            this.SpellRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}