namespace Rethought_Camera
{
    #region Using Directives

    using System.Collections.Generic;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Camera.Modules.Camera;
    using Rethought_Camera.Modules.Dynamic;
    using Rethought_Camera.Modules.Static;

    #endregion

    public class RethoughtCameraV1 : LoadableBase
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
        public override string InternalName { get; set; } = "Rethought_Camera_1";

        /// <summary>
        ///     Gets or sets the tags.
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public override IEnumerable<string> Tags { get; set; } = new[] { "Version_1" };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public override void Load()
        {
            // root
            var superParent = new SuperParent(this.DisplayName);

            // essential
            var cameraModule = new CameraModule();

            // dynamic
            var dynamic = new DynamicCameraParent(cameraModule);
            dynamic.Add(new MouseModule());
            dynamic.Add(new FarmModule());
            dynamic.Add(new ComboModule());

            // static
            var movetoMouse = new MoveToMouseModule(cameraModule, dynamic);
            var zoomhack = new ZoomHackModule(cameraModule);
            var quickswitch = new QuickSwitchModule(cameraModule, dynamic);

            // root assignments
            superParent.Add(cameraModule);
            superParent.Add(movetoMouse);
            superParent.Add(zoomhack);
            superParent.Add(quickswitch);
            superParent.Add(dynamic);

            superParent.Load();

            this.RootMenu = superParent.Menu;
        }

        #endregion
    }
}