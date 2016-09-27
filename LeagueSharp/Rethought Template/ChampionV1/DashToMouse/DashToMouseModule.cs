namespace Rethought_Kayle.KayleV1.DashToMouse
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Kayle.KayleV1.Spells;

    #endregion

    internal class DashToMouseModule : ChildBase
    {
        #region Fields

        private readonly KayleQ kayleQ;

        #endregion

        #region Constructors and Destructors

        public DashToMouseModule(KayleQ kayleQ)
        {
            this.kayleQ = kayleQ;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Dash To Mouse";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Game.OnUpdate -= this.OnUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Game.OnUpdate += this.OnUpdate;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(new MenuItem("keybind", "Keybind").SetValue(new KeyBind('A', KeyBindType.Press)));
        }

        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void OnUpdate(EventArgs args)
        {
            if (!this.Menu.Item("keybind").GetValue<KeyBind>().Active) return;

            ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            var path = this.kayleQ.GetPath(ObjectManager.Player.ServerPosition, Game.CursorPos);

            if (path == null || !path.Any())
            {
                return;
            }

            this.kayleQ.Spell.Cast(path.FirstOrDefault());
        }

        #endregion
    }
}