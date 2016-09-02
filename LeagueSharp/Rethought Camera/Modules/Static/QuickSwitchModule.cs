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

    internal class QuickSwitchModule : ParentBase
    {
        #region Fields

        /// <summary>
        ///     The camera module
        /// </summary>
        private readonly CameraModule cameraModule;

        /// <summary>
        ///     Whether moving camera
        /// </summary>
        private bool executing;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="QuickSwitchModule" /> class.
        /// </summary>
        /// <param name="cameraModule">The camera module.</param>
        /// <param name="transitionsModule">The transitions module, can be null and a default will be attached</param>
        public QuickSwitchModule(CameraModule cameraModule, TransitionsModule transitionsModule = null)
        {
            if (transitionsModule == null)
            {
                this.TransitionsModule = new TransitionsModule("Transition Module");
                this.Add(this.TransitionsModule);
            }
            else
            {
                this.TransitionsModule = transitionsModule;
            }

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
        public override string Name { get; set; } = "QuickSwitch";

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
        /// <param name="sender"></param>
        /// <param name="featureBaseEventArgs"></param>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            var enemies = this.Menu.AddSubMenu(new Menu("Enemies", "enemies"));
            var allies = this.Menu.AddSubMenu(new Menu("Allies", "allies"));

            var index = 1;
            var index2 = 6;

            foreach (var hero in HeroManager.AllHeroes)
            {
                index++;
                if (hero.IsEnemy)
                {
                    enemies.AddItem(new MenuItem(hero.ChampionName, hero.ChampionName).SetValue(new KeyBind(index.ToString().ToCharArray()[0], KeyBindType.Press)))
                        .ValueChanged += (o, args) => { this.Execute(args, hero); };
                }
                else if (hero.IsAlly)
                {
                    allies.AddItem(new MenuItem(hero.ChampionName, hero.ChampionName).SetValue(new KeyBind(index2.ToString().ToCharArray()[0], KeyBindType.Press)))
                        .ValueChanged += (o, args) => { this.Execute(args, hero); };
                }
            }

            base.OnLoad(sender, featureBaseEventArgs);
        }

        /// <summary>
        ///     Send a move command to the camera module in order to move it.
        /// </summary>
        /// <param name="args">The <see cref="OnValueChangeEventArgs" /> instance containing the event data.</param>
        /// <param name="hero">The object.</param>
        private void Execute(OnValueChangeEventArgs args, GameObject hero)
        {
            if (this.executing || !this.Switch.Enabled)
            {
                return;
            }

            this.executing = true;

            this.TransitionsModule.ActiveTransitionBase.Start(Camera.Position, hero.Position, Utils.TickCount);

            Game.OnUpdate += this.GameOnOnUpdate;
        }

        /// <summary>
        /// OnGameUpdate
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
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