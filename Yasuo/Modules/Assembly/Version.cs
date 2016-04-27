namespace Yasuo.Modules.Assembly
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Utility;

    internal class Version : Child<Assembly>
    {
        #region Fields

        /// <summary>
        ///     last checked
        /// </summary>
        private float lastChecked;

        /// <summary>
        ///     notified
        /// </summary>
        private bool notified;

        /// <summary>
        ///     The version checker
        /// </summary>
        private VersionChecker versionChecker;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Version" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public Version(Assembly parent)
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
        public override string Name => "Version";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Draws this instance.
        /// </summary>
        public void Draw()
        {
            // TODO: PRIORITY LOW
        }

        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnUpdate(EventArgs args)
        {
            if (Game.Time - 300 > this.lastChecked)
            {
                this.versionChecker.CheckNewVersion(GlobalVariables.GitHubPath);

                if (versionChecker.UpdateAvailable)
                {
                    if (versionChecker.ForceUpdate)
                    {
                        Notifications.AddNotification(GlobalVariables.Name + "- IMPORTANT UPDATE!", dispose: false);
                    }
                    else
                    {
                        if (this.Menu.Item(this.Name + "NotifyNewVersion").GetValue<bool>())
                        {
                            if (!notified)
                            {
                                Notifications.AddNotification(GlobalVariables.Name + "- Update Available!", 2500);

                                this.notified = true;
                                Draw();
                            }
                        }
                    }
                    this.Menu.Item(this.Name + "Version").DisplayName = "Version is outdated";
                }
                this.lastChecked = Game.Time;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable()
        {
            Game.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            Game.OnUpdate += this.OnUpdate;
            base.OnEnable();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override sealed void OnLoad()
        {
            this.versionChecker = new VersionChecker(GlobalVariables.GitHubPath);

            this.Menu = new Menu(this.Name, this.Name);
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            //this.AssemblyVersion.Check(Variables.GitHubPath);

            this.Menu.AddItem(
                this.versionChecker.UpdateAvailable
                    ? new MenuItem(this.Name + "Version", "Version: " + 1337)
                    : new MenuItem(this.Name + "Version", "Version is outdated"));

            this.Menu.AddItem(
                new MenuItem(this.Name + "LiveCheck", "Check For new Version").SetValue(true)
                    .SetTooltip(
                        "If this is enabled, the assembly will look every few minutes if a newer version is available"));

            this.Menu.AddItem(
                new MenuItem(this.Name + "NotifyNewVersion", "Notify if new Version available").SetValue(true)
                    .SetTooltip(
                        "If this is enabled, the assembly will notify you when a new version is available. It will always inform you about important updates."));

            this.Parent.Menu.AddSubMenu(this.Menu);

            this.lastChecked = Game.Time;
        }

        #endregion
    }
}