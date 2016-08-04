namespace RethoughtLibTest.Champions.Yorick.Childs
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal sealed class ExampleChild : ChildBase
    {
        public ExampleChild(string name)
        {
            this.Name = name;
        }

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            /* Unsubscribe to events for example */

            Game.OnUpdate -= this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            /* Subscribe to events for example */

            Game.OnUpdate += this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(new MenuItem("sliderRange", "Min Distance").SetValue(new Slider(0, 0, 500)));
        }

        private void GameOnOnUpdate(EventArgs args)
        {
            /* Write your logic here */
        }

        #endregion
    }
}