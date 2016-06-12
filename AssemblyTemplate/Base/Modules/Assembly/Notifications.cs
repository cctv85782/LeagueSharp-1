namespace AssemblyName.Base.Modules.Assembly
{
    #region Using Directives

    using AssemblyName.MediaLib.Classes.Feature;
    using AssemblyName.Properties;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    #endregion

    internal class Notifications : FeatureChild<Assembly>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Debug" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public Notifications(Assembly parent)
            : base(parent)
        {
            this.OnLoad();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name => "Notifications";

        #endregion

        #region Methods

        // TODO
        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(false));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        /// <summary>
        ///     Method to display a banner and a text
        /// </summary>
        /// <param name="displayTime">Time of displaying</param>
        private static void DrawBanner(int displayTime)
        {
            LeagueSharp.Common.Notifications.AddNotification(
                $"{GlobalVariables.DisplayName} - loaded successfully!",
                displayTime,
                true);

            if (!(Game.Time <= 1000)) return;

            var banner = new Render.Sprite(Resources.BannerLoading, new Vector2())
                             { Scale = new Vector2(1 / (Drawing.Width / 3), 1 / (Drawing.Height / 3)).Normalized() };

            var position = new Vector2(
                (Drawing.Width / 2) - banner.Width / 2,
                (Drawing.Height / 2) - banner.Height / 2 - 50);

            banner.Position = position;

            banner.Add(0);

            banner.OnDraw();

            Utility.DelayAction.Add(displayTime, () => banner.Remove());
        }

        #endregion
    }
}