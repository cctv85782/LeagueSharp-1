namespace Rethought_Irelia.IreliaV1.LastHit
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Irelia.IreliaV1.Spells;

    #endregion

    internal class Q : OrbwalkingChild
    {
        #region Fields


        /// <summary>
        /// The irelia q
        /// </summary>
        private readonly IreliaQ ireliaQ;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Q" /> class.
        /// </summary>
        /// <param name="ireliaQ">The Q logic</param>
        public Q(IreliaQ ireliaQ)
        {
            this.ireliaQ = ireliaQ;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Q";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Game.OnUpdate -= this.OnGameUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Game.OnUpdate += this.OnGameUpdate;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(
                new MenuItem(this.Name + "clearmode", "Clear Mode: ").SetValue(
                    new StringList(new[] { "Unkillable", "Always" })));
        }

        /// <summary>
        ///     Raises the <see cref="E:GameUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnGameUpdate(EventArgs args)
        {
            if (!this.CheckGuardians()) return;

            var units =
                MinionManager.GetMinions(
                    this.ireliaQ.Spell.Range,
                    MinionTypes.All,
                    MinionTeam.NotAlly,
                    MinionOrderTypes.Health).Where(x => this.ireliaQ.WillReset(x));

            switch (this.Menu.Item(this.Name + "clearmode").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    this.ireliaQ.Spell.Cast(
                        units.FirstOrDefault(
                            x =>
                            x.Distance(ObjectManager.Player) >= ObjectManager.Player.AttackRange + 200 && x.HealthPercent <= 20));
                    break;
                case 1:
                    this.ireliaQ.Spell.Cast(units.FirstOrDefault());
                    break;
            }
        }

        #endregion
    }
}