namespace Rethought_Camera.Modules.Dynamic
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Math = RethoughtLib.Utility.Math;

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
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Game.OnUpdate -= this.OnUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Game.OnUpdate += this.OnUpdate;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(new MenuItem("keybind1", "Keybind").SetValue(new KeyBind('\n', KeyBindType.Press)));

            this.Menu.AddItem(
                new MenuItem("keybind2", "Alternative Keybind").SetValue(new KeyBind('C', KeyBindType.Press)));

            this.Menu.AddItem(
                new MenuItem("range", "Range").SetValue(new Slider(1000, 100, 1500))
                    .SetTooltip("Sets the boundaries. You still want more view? Leave me a comment on the thread! :)"));

            this.Menu.AddItem(
                new MenuItem("rangedivider", "Resistance").SetValue(new Slider(57000, 1000, 70000))
                    .SetTooltip("How hard the camera will be slowed"));
        }

        private void OnUpdate(EventArgs args)
        {
            if (!this.Menu.Item("keybind1").GetValue<KeyBind>().Active
                && !this.Menu.Item("keybind2").GetValue<KeyBind>().Active)
            {
                this.Switch.Enabled = false;
            }
            else
            {
                this.Switch.Enabled = true;
            }
        }

        #endregion
    }
}