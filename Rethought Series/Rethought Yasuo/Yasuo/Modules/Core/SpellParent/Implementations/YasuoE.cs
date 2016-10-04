namespace Rethought_Yasuo.Yasuo.Modules.Core.SpellParent.Implementations
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Switches;
    using RethoughtLib.LogicProvider.Interfaces;
    using RethoughtLib.LogicProvider.Modules;

    using SharpDX;

    #endregion

    internal class YasuoE : SpellChild
    {
        #region Fields

        /// <summary>
        /// The wall dash logic provider module
        /// </summary>
        private readonly IWallLogicProvider wallDashLogicProviderModule;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="YasuoE"/> class.
        /// </summary>
        /// <param name="wallDashLogicProviderModule">The wall dash logic provider module.</param>
        public YasuoE(IWallLogicProvider wallDashLogicProviderModule)
        {
            this.wallDashLogicProviderModule = wallDashLogicProviderModule;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Sweeping Blade";

        /// <summary>
        ///     Gets or sets the spell.
        /// </summary>
        /// <value>
        ///     The spell.
        /// </value>
        public override Spell Spell { get; set; }

        #endregion

        #region Public Methods and Operators

        // TODO
        /// <summary>
        ///     Returns whether Yasuo can dash onto a specified unit
        /// </summary>
        /// <returns></returns>
        public bool CanBeDashed(Obj_AI_Base unit)
        {
            return true;
        }

        /// <summary>
        /// Returns the position after dash
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public Vector3 DashEndPosition(Vector3 start, Vector3 end)
        {
            return this.wallDashLogicProviderModule.PositionAfterDash(start, end);
        }

        /// <summary>
        ///     Returns whether Yasuo is dashing
        /// </summary>
        /// <returns></returns>
        public bool Dashing()
        {
            return ObjectManager.Player.IsDashing();
        }

        // TODO
        /// <summary>
        ///     Gets the e stacks.
        /// </summary>
        /// <returns></returns>
        public int GetStacks()
        {
            return 0;
        }

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

            this.Spell = new Spell(SpellSlot.E);
        }

        /// <summary>
        ///     Initializes the menu
        /// </summary>
        protected override void SetMenu()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Switch = new BoolSwitch(this.Menu, "Auto-Updating", true, this);
        }

        // TODO: Optimize
        private void GameOnOnUpdate(EventArgs args)
        {
            this.Spell = new Spell(SpellSlot.E, 475);
            this.Spell.SetTargetted(0, 1025);
        }

        #endregion
    }
}