namespace Rethought_Kayle.KayleV1.Combo
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Implementations;

    using Rethought_Kayle.KayleV1.Spells;

    #endregion

    internal class E : OrbwalkingChild
    {
        #region Fields

        /// <summary>
        ///     The irelia e
        /// </summary>
        private readonly KayleE kayleE;

        /// <summary>
        ///     The target
        /// </summary>
        private Obj_AI_Hero target;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="E" /> class.
        /// </summary>
        /// <param name="kayleE">The irelia e.</param>
        public E(KayleE kayleE)
        {
            this.kayleE = kayleE;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "E";

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
                new MenuItem(this.Path + "." + "stunwhenpossible", "Stun whenever possible").SetValue(true));

            this.Menu.AddItem(
                new MenuItem(this.Path + "." + "slowdown", "Cast when faster than the player").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Path + "." + "faceaway", "Cast when facing away").SetValue(false));

            // TODO: If enemy has interruptable spells wait for them to cast E option, or if player is far below enemy health and enemy is stunnable
        }

        /// <summary>
        ///     Casts when the target is facing away
        /// </summary>
        private void LogicFaceAway()
        {
            if (!this.Menu.Item(this.Path + "." + "faceaway").GetValue<bool>()) return;

            if (this.target.HasBuffOfType(BuffType.Slow) || this.target.HasBuffOfType(BuffType.Charm)
                || this.target.HasBuffOfType(BuffType.Taunt) || this.target.HasBuffOfType(BuffType.Flee)
                || this.target.IsMovementImpaired())
            {
                return;
            }

            if (!this.target.IsFacing(ObjectManager.Player)
                && ObjectManager.Player.Distance(this.target) > ObjectManager.Player.AttackRange - 50)
            {
                this.kayleE.Spell.Cast(this.target);
            }
        }

        /// <summary>
        ///     Slows the target down
        /// </summary>
        private void LogicSlowDown()
        {
            if (!this.Menu.Item(this.Path + "." + "slowdown").GetValue<bool>()) return;

            if (this.target.MoveSpeed > ObjectManager.Player.MoveSpeed && !this.target.IsMovementImpaired()
                && ObjectManager.Player.Distance(this.target) > ObjectManager.Player.AttackRange - 50)
            {
                this.kayleE.Spell.Cast(this.target);
            }
        }

        /// <summary>
        ///     Stuns whenever possible.
        /// </summary>
        private void LogicStunWhenPossible()
        {
            if (!this.Menu.Item(this.Path + "." + "stunwhenpossible").GetValue<bool>()) return;

            if (this.target.HasBuffOfType(BuffType.Charm) || this.target.HasBuffOfType(BuffType.Taunt)
                || this.target.HasBuffOfType(BuffType.Flee) || this.target.IsMovementImpaired())
            {
                return;
            }

            if (!this.kayleE.CanStun(this.target)) return;

            this.kayleE.Spell.Cast(this.target);
        }

        /// <summary>
        ///     Raises the <see cref="E:GameUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnGameUpdate(EventArgs args)
        {
            if (!this.CheckGuardians()) return;

            this.target = TargetSelector.GetTarget(this.kayleE.Spell.Range, this.kayleE.Spell.DamageType);

            if (this.target == null) return;

            this.LogicStunWhenPossible();

            this.LogicSlowDown();

            this.LogicFaceAway();
        }

        #endregion
    }
}