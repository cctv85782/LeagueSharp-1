namespace Rethought_Camera.Modules.Dynamic
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    #endregion

    internal class MouseModule : DynamicCameraChild
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Mouse";

        #endregion

        #region Public Methods and Operators

        public override Vector3 GetPosition()
        {
            if (!this.InternalEnabled) return Vector3.Zero;

            var distanceFromPlayer = ObjectManager.Player.Position.Distance(Game.CursorPos);

            var distance = Math.Min(distanceFromPlayer, this.Menu.Item("range").GetValue<Slider>().Value);

            var focus = ObjectManager.Player.ServerPosition.Extend(Game.CursorPos, distance);

            return this.Lerp(Camera.Position, focus, distance / this.Menu.Item("rangedivider").GetValue<Slider>().Value);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(
                new MenuItem("range", "Range").SetValue(new Slider(900, 100, 1500))
                    .SetTooltip("Sets the boundaries. You still want more view? Leave me a comment on the thread! :)"));

            this.Menu.AddItem(
                new MenuItem("rangedivider", "Resistance").SetValue(new Slider(43000, 1000, 70000))
                    .SetTooltip("How hard the camera will be slowed"));

            this.InternalEnabled = true;
        }

        #endregion
    }
}