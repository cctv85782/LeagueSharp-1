namespace Rethought_Camera.Modules.Static
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Camera.Modules.Camera;
    using Rethought_Camera.Modules.Transitions;

    #endregion

    internal class MoveToMouseModule : ParentBase
    {
        #region Fields

        private readonly CameraModule cameraModule;

        private bool executing;

        #endregion

        #region Constructors and Destructors

        public MoveToMouseModule(CameraModule cameraModule, TransitionsModule transitionsModule = null)
        {
            this.TransitionsModule = transitionsModule ?? new TransitionsModule("Transition Module");

            this.Add(this.TransitionsModule);

            this.cameraModule = cameraModule;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Move to Mouse";

        /// <summary>
        ///     Gets or sets the transitions module.
        /// </summary>
        /// <value>
        ///     The transitions module.
        /// </value>
        public TransitionsModule TransitionsModule { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(new MenuItem("keybind", "Keybind: ").SetValue(new KeyBind('A', KeyBindType.Press)))
                .ValueChanged += (o, args) =>
                    {
                        if (args.GetNewValue<KeyBind>().Active) this.Execute(args);
                    };
        }

        /// <summary>
        ///     Send a move command to the camera module in order to move it.
        /// </summary>
        /// <param name="args">The <see cref="OnValueChangeEventArgs" /> instance containing the event data.</param>
        /// <param name="hero">The object.</param>
        private void Execute(OnValueChangeEventArgs args)
        {
            if (this.executing)
            {
                return;
            }

            this.executing = true;

            if (!args.GetNewValue<KeyBind>().Active) return;

            this.TransitionsModule.ActiveTransitionBase.Start(Camera.Position, Game.CursorPos);

            Game.OnUpdate += this.GameOnOnUpdate;
        }

        private void GameOnOnUpdate(EventArgs args)
        {
            if (this.TransitionsModule.ActiveTransitionBase.Moving)
            {
                this.cameraModule.SetPosition(this.TransitionsModule.ActiveTransitionBase.GetPosition(), 0);
            }
            else
            {
                this.executing = false;
                Game.OnUpdate -= this.GameOnOnUpdate;
            }
        }

        #endregion
    }
}