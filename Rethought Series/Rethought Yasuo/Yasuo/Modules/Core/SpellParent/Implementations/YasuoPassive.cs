namespace Rethought_Yasuo.Yasuo.Modules.Core.SpellParent.Implementations
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;
    using RethoughtLib.FeatureSystem.Switches;
    using System;

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
        /// Called when [load].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">the contextual information</param>
        protected override void OnLoad(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnLoad(sender, eventArgs);

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

        #endregion
    }
}