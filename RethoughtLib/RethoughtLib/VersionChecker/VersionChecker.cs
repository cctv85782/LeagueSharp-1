//namespace RethoughtLib.VersionChecker
//{
//    #region Using Directives

//    using System;

//    using LeagueSharp;
//    using LeagueSharp.Common;

//    using global::RethoughtLib.Classes.Feature;
//    using global::RethoughtLib.Events;
//    using global::RethoughtLib.Utility;

//    #endregion

//    // TODO: PRIORITY LOW > Notify with a sound and a notification banner

//    internal class VersionChecker : 
//    {
//        public string AssemblyName { get; set; }

//        #region Fields

//        /// <summary>
//        ///     last checked
//        /// </summary>
//        private float lastChecked;

//        /// <summary>
//        ///     notified
//        /// </summary>
//        private bool notified;

//        /// <summary>
//        ///     The version checker
//        /// </summary>
//        private readonly global::RethoughtLib.Utility.VersionChecker versionChecker;

//        #endregion

//        #region Constructors and Destructors

//        /// <summary>
//        ///     Initializes a new instance of the <see cref="System.Version" /> class.
//        /// </summary>
//        /// <param name="parent">The parent.</param>
//        /// <param name="githubPath"></param>
//        /// <param name="assemblyName"></param>
//        public VersionChecker(FeatureParent parent, string githubPath, string assemblyName)
//            : base(parent)
//        {
//            this.AssemblyName = assemblyName;
//            this.versionChecker = new global::RethoughtLib.Utility.VersionChecker(githubPath, this.AssemblyName);
//            this.OnLoad();
//        }

//        #endregion

//        #region Public Properties

//        /// <summary>
//        ///     Gets the name.
//        /// </summary>
//        /// <value>
//        ///     The name.
//        /// </value>
//        public override string Name => "VersionChecker";

//        #endregion

//        #region Public Methods and Operators

//        /// <summary>
//        ///     Draws this instance.
//        /// </summary>
//        public void Draw()
//        {
//            // TODO: PRIORITY LOW
//        }

//        /// <summary>
//        ///     Raises the <see cref="E:Update" /> event.
//        /// </summary>
//        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
//        public void OnUpdate(EventArgs args)
//        {
//            if (Game.Time - 300 < this.lastChecked)
//            {
//                return;
//            }

//            this.versionChecker.CheckNewVersion();

//            if (this.versionChecker.UpdateAvailable)
//            {
//                if (this.versionChecker.ForceUpdate)
//                {
//                    Notifications.AddNotification(this.AssemblyName + "- IMPORTANT UPDATE!", dispose: false);
//                }
//                else
//                {
//                    if (this.Menu.Item(this.Name + "NotifyNewVersion").GetValue<bool>())
//                    {
//                        if (!this.notified)
//                        {
//                            Notifications.AddNotification(this.AssemblyName + "- Update Available!", 2500);

//                            this.notified = true;
//                            this.Draw();
//                        }
//                    }
//                }
//                this.Menu.Item(this.Name + "VersionChecker").DisplayName = "VersionChecker is outdated";
//            }
//            this.lastChecked = Game.Time;
//        }

//        #endregion

//        #region Methods

//        /// <summary>
//        ///     Called when [disable].
//        /// </summary>
//        protected override void OnDisable()
//        {
//            Events.OnUpdate -= this.OnUpdate;
//            base.OnDisable();
//        }

//        /// <summary>
//        ///     Called when [enable].
//        /// </summary>
//        protected override void OnEnable()
//        {
//            Events.OnUpdate += this.OnUpdate;
//            base.OnEnable();
//        }

//        /// <summary>
//        ///     Called when [load].
//        /// </summary>
//        protected sealed override void OnLoad()
//        {
//            this.Menu = new Menu(this.Name, this.Name);
//            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

//            //this.AssemblyVersion.Check(Variables.GitHubPath);

//            this.Menu.AddItem(
//                this.versionChecker.UpdateAvailable
//                    ? new MenuItem(this.Name + "VersionChecker", "VersionChecker: " + 1337)
//                    : new MenuItem(this.Name + "VersionChecker", "VersionChecker is outdated"));

//            this.Menu.AddItem(
//                new MenuItem(this.Name + "LiveCheck", "Check For new VersionChecker").SetValue(true)
//                    .SetTooltip(
//                        "If this is enabled, the assembly will look every few minutes if a newer version is available"));

//            this.Menu.AddItem(
//                new MenuItem(this.Name + "NotifyNewVersion", "Notify if new VersionChecker available").SetValue(true)
//                    .SetTooltip(
//                        "If this is enabled, the assembly will notify you when a new version is available. It will always inform you about important updates."));

//            this.Parent.Menu.AddSubMenu(this.Menu);

//            this.lastChecked = Game.Time;
//        }

//        #endregion
//    }
//}