﻿namespace Rethought_Kayle.KayleV1.LastHit
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Kayle.KayleV1.Spells;

    #endregion

    internal class Q : OrbwalkingChild
    {
        #region Fields

        /// <summary>
        ///     The irelia q
        /// </summary>
        private readonly KayleQ kayleQ;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Q" /> class.
        /// </summary>
        /// <param name="kayleQ">The Q logic</param>
        public Q(KayleQ kayleQ)
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
                new MenuItem(this.Path + "." + "clearmode", "Clear Mode: ").SetValue(
                    new StringList(new[] { "Unkillable", "Always" })));

            this.Menu.AddItem(new MenuItem(this.Path + "." + "noturretdive", "Don't dive turrets").SetValue(true));

            this.Menu.AddItem(new MenuItem("eplxaination", "Don't get closer than that to: "));

            foreach (var enemy in HeroManager.Enemies)
            {
                this.Menu.AddItem(
                    new MenuItem(this.Path + "." + enemy.ChampionName, enemy.ChampionName).SetValue(
                        new Slider(300, 0, 1000)));
            }
        }

        private void LogicJungleLastHit()
        {
            var units =
                MinionManager.GetMinions(
                    this.kayleQ.Spell.Range,
                    MinionTypes.All,
                    MinionTeam.Neutral,
                    MinionOrderTypes.Health).Where(x => this.kayleQ.WillReset(x)).ToList();
            var unit = units.FirstOrDefault();

            if (unit == null) return;

            this.kayleQ.Spell.Cast(unit);
        }

        private void LogicLaneLastHit()
        {
            var units =
                MinionManager.GetMinions(
                    this.kayleQ.Spell.Range,
                    MinionTypes.All,
                    MinionTeam.NotAlly).Where(x => this.kayleQ.WillReset(x)).ToList();

            if (this.Menu.Item(this.Path + "." + "noturretdive").GetValue<bool>())
            {
                foreach (var unit2 in units.ToList())
                {
                    if (unit2.UnderTurret(true))
                    {
                        units.Remove(unit2);
                    }
                }
            }

            foreach (var enemy in
                HeroManager.Enemies.Where(x => !x.IsDead && x.Distance(ObjectManager.Player) <= 1000 + this.kayleQ.Spell.Range))
            {
                foreach (var entry in units.ToList())
                {
                    if (entry.Distance(enemy)
                        <= this.Menu.Item(this.Path + "." + enemy.ChampionName).GetValue<Slider>().Value)
                    {
                        units.Remove(entry);
                    }
                }
            }

            Obj_AI_Base unit = null;

            switch (this.Menu.Item(this.Path + "." + "clearmode").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    unit =
                        units.FirstOrDefault(
                            x =>
                            x.Distance(ObjectManager.Player) >= ObjectManager.Player.AttackRange
                            && x.HealthPercent <= 20);
                    break;
                case 1:
                    unit = units.FirstOrDefault();
                    break;
            }

            if (unit != null)
            {
                this.kayleQ.Spell.Cast(unit);
            }
        }

        /// <summary>
        ///     Raises the <see cref="E:GameUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnGameUpdate(EventArgs args)
        {
            if (!this.CheckGuardians()) return;

            this.LogicLaneLastHit();

            this.LogicJungleLastHit();
        }

        #endregion
    }
}