﻿namespace Rethought_Kayle.KayleV1.Combo
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Kayle.KayleV1.DamageCalculator;
    using Rethought_Kayle.KayleV1.Spells;

    #endregion

    internal class R : OrbwalkingChild
    {
        #region Fields

        private readonly KayleQ kayleQ;

        /// <summary>
        ///     The irelia r
        /// </summary>
        private readonly KayleR kayleR;

        /// <summary>
        ///     The hitchance
        /// </summary>
        private HitChance hitchance;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="W" /> class.
        /// </summary>
        /// <param name="kayleR">The irelia r.</param>
        /// <param name="kayleQ">The irelia q</param>
        /// <param name="damageCalculator">The damage calculator</param>
        public R(KayleR kayleR, KayleQ kayleQ, IDamageCalculator damageCalculator)
        {
            this.DamageCalculator = damageCalculator;
            this.kayleR = kayleR;
            this.kayleQ = kayleQ;
        }

        #endregion

        #region Public Properties

        public IDamageCalculator DamageCalculator { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "R";

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

            // Gets all HitChances as Type[]
            var values = Enum.GetValues(typeof(HitChance));

            // Converts Type[] to string[]
            var stringList = (from object value in values select value.ToString()).ToArray();

            // Puts all HitChances into the Menu
            var minHitChance =
                this.Menu.AddItem(
                    new MenuItem(this.Path + "." + "minhitchance", "Minimal Hitchance").SetValue(
                        new StringList(stringList, 4)));

            minHitChance.ValueChanged +=
                (o, args) => { this.hitchance = (HitChance)args.GetNewValue<StringList>().SelectedIndex; };

            this.hitchance = (HitChance)minHitChance.GetValue<StringList>().SelectedIndex;

            this.Menu.AddItem(new MenuItem(this.Path + "." + "usetoregen", "Use to regenerate health").SetValue(true));

            this.Menu.AddItem(
                new MenuItem(this.Path + "." + "usetoregen.minlife", " > Use when below X (percentage) health").SetValue(
                    new Slider(20, 0, 100)));

            this.Menu.AddItem(
                new MenuItem(this.Path + "." + "usetoregen.minlifeflat", " > Use when below X (flat) health").SetValue(
                    new Slider(200, 0, 5000)));
        }

        /// <summary>
        ///     Casts the spell on target with prediction.
        /// </summary>
        /// <param name="target">The target.</param>
        private void CastOnTarget(Obj_AI_Base target)
        {
            var prediction = this.kayleR.Spell.GetPrediction(
                target,
                true,
                collisionable: new[] { CollisionableObjects.YasuoWall });

            if (prediction != null && prediction.Hitchance >= this.hitchance)
            {
                this.kayleR.Spell.Cast(prediction.CastPosition);
            }
        }

        /// <summary>
        ///     Logic for executing R when killable by combo
        /// </summary>
        /// <param name="target">The target.</param>
        private void LogicAutoCombo(Obj_AI_Base target)
        {
            if (this.DamageCalculator.GetDamage(target) < target.Health
                || (this.kayleQ.GetPath(ObjectManager.Player.ServerPosition, target.ServerPosition) == null
                    && ObjectManager.Player.Distance(target) > this.kayleQ.Spell.Range
                    && this.kayleR.GetDamage(target) * this.kayleR.EstimatedAmountInOneCombo < target.Health)) return;

            this.CastOnTarget(target);
        }

        /// <summary>
        ///     Logic life regeneration
        /// </summary>
        /// <param name="target">The target.</param>
        private void LogicRegenerateHealth(Obj_AI_Base target)
        {
            if (!this.Menu.Item(this.Path + "." + "usetoregen").GetValue<bool>()) return;

            var enemyMeanHealth = 0f;
            var count = 0;

            foreach (var enemy in HeroManager.Enemies.Where(x => x.Distance(ObjectManager.Player) <= 1250))
            {
                count++;
                enemyMeanHealth += enemy.HealthPercent;
            }

            enemyMeanHealth /= count;

            if (ObjectManager.Player.HealthPercent
                <= this.Menu.Item(this.Path + "." + "usetoregen.minlife").GetValue<Slider>().Value
                || ObjectManager.Player.Health <= this.Menu.Item(this.Path + "." + "usetoregen.minlifeflat").GetValue<Slider>().Value
                && ObjectManager.Player.HealthPercent <= enemyMeanHealth + 5)
            {
                this.CastOnTarget(target);
            }
        }

        /// <summary>
        ///     Raises the <see cref="E:GameUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnGameUpdate(EventArgs args)
        {
            if (!this.CheckGuardians()) return;

            var target = TargetSelector.GetTarget(this.kayleR.Spell.Range, TargetSelector.DamageType.Physical);

            if (target == null) return;

            this.LogicRegenerateHealth(target);

            this.LogicAutoCombo(target);
        }

        #endregion
    }
}