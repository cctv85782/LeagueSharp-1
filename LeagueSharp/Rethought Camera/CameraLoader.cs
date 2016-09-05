namespace Rethought_Camera
{
    #region Using Directives

    using System.Collections.Generic;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Camera.Modules;
    using Rethought_Camera.Modules.Camera;
    using Rethought_Camera.Modules.Dynamic;
    using Rethought_Camera.Modules.Static;

    #endregion

    internal class CameraLoader : LoadableBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name that will get displayed.
        /// </summary>
        /// <value>
        ///     The name of the displaying.
        /// </value>
        public override string DisplayName { get; set; } = "Rethought Camera";

        /// <summary>
        ///     Gets or sets the internal name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string InternalName { get; set; } = "Rethought_Camera";

        /// <summary>
        ///     Gets or sets the tags.
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public override IEnumerable<string> Tags { get; set; } = new[] { "Utility", "Always" };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public override void Load()
        {
            var superParent = new SuperParent(this.DisplayName);

            var cameraModule = new CameraModule();
            var dynamic = new DynamicCameraParent(cameraModule);
            var movetoMouse = new MoveToMouseModule(cameraModule, dynamic);
            var zoomhack = new ZoomHackModule(cameraModule);
            var quickswitch = new QuickSwitchModule(cameraModule);

            dynamic.Add(new MouseModule());
            dynamic.Add(new FarmModule());
            dynamic.Add(new ComboModule());

            superParent.Add(cameraModule);
            superParent.Add(movetoMouse);
            superParent.Add(zoomhack);
            superParent.Add(quickswitch);
            superParent.Add(dynamic);

            superParent.Load();
        }

        #endregion
    }
}