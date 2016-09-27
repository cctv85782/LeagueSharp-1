namespace Rethought_Kayle.KayleV1.Combo
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Kayle.KayleV1.Spells;

    #endregion

    internal class W : OrbwalkingChild
    {
        #region Fields

        /// <summary>
        ///     The irelia q
        /// </summary>
        private readonly KayleW kayleW;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="W" /> class.
        /// </summary>
        /// <param name="kayleW">The kayle w.</param>
        public W(KayleW kayleW)
        {
            this.kayleW = kayleW;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "W";

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

            this.Menu.AddItem(
                new MenuItem(this.Path + ".prioritymode", "Priority").SetValue(
                    new StringList(new[] { "Priority", "Current Health" })));

            this.Menu.AddItem(new MenuItem("disclaimer", "Min Health % (per ally)"));

            foreach (var ally in HeroManager.Allies)
            {
                if (ally.ChampionName == ObjectManager.Player.ChampionName)
                {
                    this.Menu.AddItem(
                        new MenuItem(this.Path + ".minhealthally." + ally.ChampionName, "Yourself").SetValue(
                            new Slider(40, 0, 100)));
                    continue;
                }

                this.Menu.AddItem(
                    new MenuItem(this.Path + ".minhealthally." + ally.ChampionName, ally.ChampionName).SetValue(
                        new Slider(20, 0, 100)));
            }


            this.Menu.AddItem(new MenuItem(this.Path + ".speedup", "Min Health % (per ally)"));

            foreach (var ally in HeroManager.Allies)
            {
                if (ally.ChampionName == ObjectManager.Player.ChampionName)
                {
                    this.Menu.AddItem(
                        new MenuItem(this.Path + ".minhealthally." + ally.ChampionName, "Yourself").SetValue(
                            new Slider(40, 0, 100)));
                    continue;
                }

                this.Menu.AddItem(
                    new MenuItem(this.Path + ".minhealthally." + ally.ChampionName, ally.ChampionName).SetValue(
                        new Slider(20, 0, 100)));
            }
        }

        private void LogicSpeedup()
        {
            var targetList =
                HeroManager.Allies.Where(
                    x =>
                    x.Health <= this.Menu.Item(this.Path + ".speedup." + x.ChampionName).GetValue<Slider>().Value)
                    .ToList();

            if (!targetList.Any()) return;

            Obj_AI_Base target = null;

            switch (this.Menu.Item(this.Path + ".prioritymode").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    target = targetList.MaxOrDefault(TargetSelector.GetPriority);
                    break;
                case 1:
                    target = targetList.MinOrDefault(x => x.Health);
                    break;
            }

            if (target == null) return;

            this.kayleW.Spell.Cast(target);
        }

        // TODO: Think about HealthPrediction
        /// <summary>
        ///     Called when the game updates
        /// </summary>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnUpdate(EventArgs eventArgs)
        {
            if (!this.CheckGuardians()) return;

            var targetList =
                HeroManager.Allies.Where(
                    x =>
                    x.Health <= this.Menu.Item(this.Path + ".minhealthally." + x.ChampionName).GetValue<Slider>().Value)
                    .ToList();

            if (!targetList.Any()) return;

            Obj_AI_Base target = null;

            switch (this.Menu.Item(this.Path + ".prioritymode").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    target = targetList.MaxOrDefault(TargetSelector.GetPriority);
                    break;
                case 1:
                    target = targetList.MinOrDefault(x => x.Health);
                    break;
            }

            if (target == null) return;

            this.kayleW.Spell.Cast(target);
        }

        #endregion
    }
}