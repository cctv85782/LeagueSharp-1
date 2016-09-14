namespace Rethought_Camera.Modules.Dynamic
{
    #region Using Directives

    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Utility;

    using SharpDX;

    #endregion

    internal class FarmModule : DynamicCameraChild
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "LaneClear / LastHit";

        #endregion

        #region Public Methods and Operators

        public override Vector3 GetPosition()
        {
            if (!this.InternalEnabled) return Vector3.Zero;

            var minions = MinionManager.GetMinions(
                ObjectManager.Player.ServerPosition,
                this.Menu.Item("range").GetValue<Slider>().Value,
                MinionTypes.All,
                MinionTeam.NotAlly);

            if (!minions.Any()) return Vector3.Zero;

            var focus = Math.MeanVector3(minions);

            return this.Lerp(
                Camera.Position,
                focus,
                ObjectManager.Player.Distance(focus) / this.Menu.Item("rangedivider").GetValue<Slider>().Value);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(new MenuItem("keybind1", "Keybind").SetValue(new KeyBind('V', KeyBindType.Press)))
                .ValueChanged += (o, args) => { this.ProcessKeybind(args); };

            this.Menu.AddItem(
                new MenuItem("keybind2", "Alternative Keybind").SetValue(new KeyBind('X', KeyBindType.Press)))
                .ValueChanged += (o, args) => { this.ProcessKeybind(args); };

            this.Menu.AddItem(
                new MenuItem("keybind3", "Alternative Keybind").SetValue(new KeyBind('C', KeyBindType.Press)))
                .ValueChanged += (o, args) => { this.ProcessKeybind(args); };

            this.Menu.AddItem(
                new MenuItem("range", "Range").SetValue(new Slider(1000, 100, 1500))
                    .SetTooltip("Sets the boundaries. You still want more view? Leave me a comment on the thread! :)"));

            this.Menu.AddItem(
                new MenuItem("rangedivider", "Resistance").SetValue(new Slider(57000, 1000, 70000))
                    .SetTooltip("How hard the camera will be slowed"));
        }


        #endregion
    }
}