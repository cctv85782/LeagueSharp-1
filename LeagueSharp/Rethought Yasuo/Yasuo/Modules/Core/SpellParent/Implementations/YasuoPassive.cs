namespace Rethought_Yasuo.Yasuo.Modules.Core.SpellParent.Implementations
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Switches;

    #endregion

    internal class YasuoPassive : SpellChild
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Yasuo Passive (Flow & Shield)";

        /// <summary>
        ///     Gets or sets the spell.
        /// </summary>
        /// <value>
        ///     The spell.
        /// </value>
        public override Spell Spell { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Game.OnUpdate -= this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [OnEnable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Game.OnUpdate += this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [initialize].
        /// </summary>
        protected override void OnInitialize(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnInitialize(sender, featureBaseEventArgs);
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Spell = null;
        }

        /// <summary>
        ///     Initializes the menu
        /// </summary>
        protected override void SetMenu()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Switch = new BoolSwitch(this.Menu, "Auto-Updating", true, this);
        }

        private void GameOnOnUpdate(EventArgs args)
        {

        }

        #endregion
    }
}