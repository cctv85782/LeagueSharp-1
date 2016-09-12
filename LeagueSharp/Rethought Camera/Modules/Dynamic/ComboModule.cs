namespace Rethought_Camera.Modules.Dynamic
{
    #region Using Directives

    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Utility;

    using SharpDX;

    #endregion

    internal class ComboModule : DynamicCameraChild
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Combo / Mixed";

        #endregion

        #region Public Methods and Operators

        public override Vector3 GetPosition()
        {
            var units =
                HeroManager.Enemies.Where(
                    x => x.Distance(ObjectManager.Player.Position) <= this.Menu.Item("range").GetValue<Slider>().Value)
                    .Select(x => x.Position)
                    .ToList();

            if (!units.Any()) return Vector3.Zero;

            var focus = Math.MeanVector3(units);

            return this.Lerp(
                Camera.Position,
                focus,
                ObjectManager.Player.Distance(focus) / this.Menu.Item("rangedivider").GetValue<Slider>().Value);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(new MenuItem("keybind1", "Keybind").SetValue(new KeyBind('\n', KeyBindType.Press)))
                .ValueChanged += (o, args) => { this.ProcessKeybind(args); };

            this.Menu.AddItem(
                new MenuItem("keybind2", "Alternative Keybind").SetValue(new KeyBind('C', KeyBindType.Press)))
                .ValueChanged += (o, args) => { this.ProcessKeybind(args); };
            ;

            this.Menu.AddItem(
                new MenuItem("range", "Range").SetValue(new Slider(1000, 100, 1500))
                    .SetTooltip("Sets the boundaries. You still want more view? Leave me a comment on the thread! :)"));

            this.Menu.AddItem(
                new MenuItem("rangedivider", "Resistance").SetValue(new Slider(57000, 1000, 70000))
                    .SetTooltip("How hard the camera will be slowed"));

            this.Switch.InternalDisable(new FeatureBaseEventArgs(this));
        }

        private void ProcessKeybind(OnValueChangeEventArgs args)
        {
            if (args.GetNewValue<KeyBind>().Active)
            {
                this.Switch.InternalEnable(new FeatureBaseEventArgs(this));
            }
            else
            {
                this.Switch.InternalDisable(new FeatureBaseEventArgs(this));
            }
        }

        #endregion
    }
}