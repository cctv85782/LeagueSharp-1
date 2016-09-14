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

    internal class Q : OrbwalkingChild
    {
        #region Constants

        /// <summary>
        ///     The graves transformation range
        /// </summary>
        /// // TODO
        public const int GravesTransformationRange = 900;

        /// <summary>
        ///     The extra automatic attack range on q
        /// </summary>
        private const int ExtraAutoAttackRangeOnQ = 50;

        #endregion

        #region Fields

        /// <summary>
        ///     Gets or sets the last rites logic provider.
        /// </summary>
        /// <value>
        ///     The logic provider.
        /// </value>
        private readonly LastRites lastRites;

        /// <summary>
        ///     Gets or sets the passive observer.
        /// </summary>
        /// <value>
        ///     The passive observer.
        /// </value>
        private readonly PassiveObserver passiveObserver;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Q" /> class.
        /// </summary>
        /// <param name="passiveObserver">The passive observer.</param>
        /// <param name="lastRites">The Q logic</param>
        public Q(PassiveObserver passiveObserver, LastRites lastRites)
        {
            this.lastRites = lastRites;

            this.passiveObserver = passiveObserver;
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

        /// <summary>
        ///     Gets or sets the spell priority.
        /// </summary>
        /// <value>
        ///     The spell priority.
        /// </value>
        public int SpellPriority { get; set; } = 4;

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Orbwalking.AfterAttack -= this.OrbwalkingOnAfterAttack;

            Game.OnUpdate -= this.OnGameUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Orbwalking.AfterAttack += this.OrbwalkingOnAfterAttack;

            Game.OnUpdate += this.OnGameUpdate;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(new MenuItem("afterattack", "Animation Canceling").SetValue(true));

            this.Menu.AddItem(
                new MenuItem("mingravetransformations", "Min Graves before using Awakening").SetValue(
                    new Slider(3, 0, 8)));
        }

        /// <summary>
        ///     Logic for AttackRange expansion
        /// </summary>
        private void LogicAttackRangeExpansion()
        {
            if (
                ObjectManager.Player.ServerPosition.Distance(
                    TargetSelector.GetTarget(this.lastRites.Spell.Range, TargetSelector.DamageType.Physical, false)
                    .ServerPosition) <= ObjectManager.Player.AttackRange + ExtraAutoAttackRangeOnQ)
            {
                this.lastRites.Spell.Cast();
            }
        }

        /// <summary>
        ///     Logic for awakening.
        /// </summary>
        private void LogicAwakening()
        {
            if (
                this.passiveObserver.Graves.Count(
                    x => x.Position.Distance(ObjectManager.Player.ServerPosition) <= GravesTransformationRange)
                > this.Menu.Item("mingravetransformations").GetValue<Slider>().Value)
            {
                this.ActionManager.Queque.Enqueue(this.SpellPriority, this.lastRites.Spell.Cast());
            }
        }

        /// <summary>
        ///     Raises the <see cref="E:GameUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnGameUpdate(EventArgs args)
        {
            if (!this.CheckGuardians()) return;

            this.LogicAttackRangeExpansion();

            if (this.lastRites.CurrentSpellState != LastRites.SpellState.Awakening
                && this.lastRites.CurrentSpellState != LastRites.SpellState.LastRiteAndAwakening)
            {
                return;
            }

            this.LogicAwakening();
        }

        /// <summary>
        ///     Triggers after an attack has been issued
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="target">The target.</param>
        private void OrbwalkingOnAfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (!this.CheckGuardians()) return;

            if (!this.Menu.Item("afterattack").GetValue<bool>())
            {
                return;
            }

            this.lastRites.Spell.Cast();
        }

        #endregion
    }
}