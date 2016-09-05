namespace Rethought_Camera.Modules
{
    #region Using Directives

    using System;

    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Switches;

    using Rethought_Camera.Modules.Camera;

    #endregion

    internal class ZoomHackModule : ChildBase
    {
        #region Fields

        /// <summary>
        ///     The camera module
        /// </summary>
        private readonly CameraModule cameraModule;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ZoomHackModule" /> class.
        /// </summary>
        /// <param name="cameraModule">The camera module.</param>
        public ZoomHackModule(CameraModule cameraModule)
        {
            this.cameraModule = cameraModule;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "ZoomHack";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            //CustomEvents.Game.OnGameEnd -= this.OnGameEnd;

            this.cameraModule.ZoomHack = false;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            //CustomEvents.Game.OnGameEnd += this.OnGameEnd;

            this.cameraModule.ZoomHack = true;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="featureBaseEventArgs"></param>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            //this.Menu.AddItem(new MenuItem("tempfix", "Temp Fix: ZoomHack not working after reload").SetValue(true));

            this.Menu.AddItem(
                new MenuItem("warning", "Use At Your Own Risk").SetTooltip(
                    "ZoomHack is more likely detectable, USE IT AT YOUR OWN RISK"));

            this.Menu.AddItem(
                new MenuItem("tip", "Pro-Tip: Use it with the Dynamic Camera!").SetTooltip(
                    "ZoomHack combined with the Dynamic Camera has a great effect and ensures a lot of vision!"));
        }

        /// <summary>
        ///     Sets the switch.
        /// </summary>
        protected override void SetSwitch()
        {
            this.Switch = new BoolSwitch(this.Menu, "Enabled", false, this);
        }

        ///// <summary>
        /////     Raises the <see cref="E:GameEnd" /> event.
        ///// </summary>
        ///// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        //private void OnGameEnd(EventArgs args)
        //{
        //    if (!this.Menu.Item("tempfix").GetValue<bool>()) return;

        //    this.Disable();
        //}

        #endregion
    }
}