namespace Rethought_Camera.Modules.Static.DeathCam
{
    #region Using Directives

    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;

    #endregion

    internal abstract class DeathCamModule : Base
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <returns></returns>
        public abstract Vector3 GetPosition();

        #endregion

        public int Priority { get; set; } = 1;

        public int MaxPriority { get; set; } = 1;

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            var priority = this.Menu.AddItem(new MenuItem(this.Name + "priority", "Priority").SetValue(new Slider(0, 0, this.MaxPriority)));

            this.Priority = priority.GetValue<Slider>().Value;
        }
    }
}