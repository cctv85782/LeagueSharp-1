namespace Rethought_Yorick.YorickV1.Combo
{
    #region Using Directives

    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Yorick.YorickV1.Spells;

    #endregion

    internal class R : OrbwalkingChild
    {
        #region Fields

        /// <summary>
        ///     The dark procession logic
        /// </summary>
        private readonly DarkProcession darkProcession;

        /// <summary>
        ///     The hitchance
        /// </summary>
        private HitChance hitchance;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="W" /> class.
        /// </summary>
        /// <param name="darkProcession">The dark procession logic.</param>
        public R(DarkProcession darkProcession)
        {
            this.darkProcession = darkProcession;
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
                    new MenuItem("minhitchance", "Minimal Hitchance").SetValue(new StringList(stringList, 4)));

            minHitChance.ValueChanged +=
                (o, args) => { this.hitchance = (HitChance)args.GetNewValue<StringList>().SelectedIndex; };

            this.hitchance = (HitChance)minHitChance.GetValue<StringList>().SelectedIndex;
        }

        /// <summary>
        ///     Raises the <see cref="E:GameUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnGameUpdate(EventArgs args)
        {
            if (!this.CheckGuardians()) return;

            var target = TargetSelector.GetTarget(
                this.darkProcession.Spell.Range + this.darkProcession.Spell.Width,
                TargetSelector.DamageType.Physical);

            var prediction = this.darkProcession.Spell.GetPrediction(
                target,
                true,
                collisionable: new[] { CollisionableObjects.Walls });

            if (prediction != null && prediction.Hitchance >= this.hitchance)
            {
                this.darkProcession.Spell.Cast(prediction.CastPosition);
            }
        }

        #endregion
    }
}