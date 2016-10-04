//     Copyright (C) 2016 Rethought
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
//     Created: 04.10.2016 1:05 PM
//     Last Edited: 04.10.2016 1:44 PM

namespace Rethought_Camera.Modules.Static
{
    #region Using Directives

    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Switches;

    using Rethought_Camera.Modules.Camera;

    #endregion

    internal class ZoomHackModule : ChildBase
    {
        #region Fields

        /// <summary>
        ///     The camera module
        /// </summary>
        private readonly CameraModule cameraModule;

        #endregion

        #region Constructors and Destructors

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ZoomHackModule" /> class.
        /// </summary>
        /// <param name="cameraModule">The camera module.</param>
        public ZoomHackModule(CameraModule cameraModule)
        {
            this.cameraModule = cameraModule;
        }

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "ZoomHack";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            this.cameraModule.ZoomHack = false;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            this.cameraModule.ZoomHack = true;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="featureBaseEventArgs"></param>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            //var maxZoom = this.Menu.AddItem(new MenuItem("maxzoom", "Max Zoom").SetValue(new Slider(100, 1, 10000)));

            //maxZoom.ValueChanged += (o, args) => { this.cameraModule.MaxZoom = args.GetNewValue<Slider>().Value; };

            //this.cameraModule.MaxZoom = maxZoom.GetValue<Slider>().Value;

            //var zoom = this.Menu.AddItem(new MenuItem("zoom", "Zoom").SetValue(new Slider(100, 1, 10000)));

            //zoom.ValueChanged += (o, args) => { this.cameraModule.MaxZoom = args.GetNewValue<Slider>().Value; };

            //this.cameraModule.Zoom = zoom.GetValue<Slider>().Value;

            this.Menu.AddItem(
                new MenuItem("warning", "Use At Your Own Risk").SetTooltip(
                    "ZoomHack is more likely detectable, USE IT AT YOUR OWN RISK"));

            this.Menu.AddItem(
                new MenuItem("tip", "Pro-Tip: Use it with the Dynamic Camera!").SetTooltip(
                    "ZoomHack combined with the Dynamic Camera has a great effect and ensures a lot of vision!"));
        }

        /// <summary>
        ///     Sets the switch.
        /// </summary>
        protected override void SetSwitch()
        {
            this.Switch = new BoolSwitch(this.Menu, "Enabled", false, this);
        }

        #endregion
    }
}