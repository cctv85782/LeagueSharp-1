namespace Rethought_Irelia.IreliaV1.Combo
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
        #region Constants

        /// <summary>
        ///     The baitrange
        /// </summary>
        private const float Baitrange = 100f;

        #endregion

        #region Fields

        /// <summary>
        ///     Gets or sets the last rites logic provider.
        /// </summary>
        /// <value>
        ///     The logic provider.
        /// </value>
        private readonly IreliaQ ireliaQ;

        /// <summary>
        ///     The target
        /// </summary>
        private Obj_AI_Hero target;

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

        /// <summary>
        ///     Gets or sets the spell priority.
        /// </summary>
        /// <value>
        ///     The spell priority.
        /// </value>
        public int SpellPriority { get; set; } = 2;

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

            this.Menu.AddItem(new MenuItem(this.Name + "pathfinding", "Gapclosing / Pathfinding").SetValue(true));

            this.Menu.AddItem(new MenuItem(this.Name + "movementprediction", "Movement Prediction").SetValue(true));

            this.Menu.AddItem(
                new MenuItem(this.Name + "baitenemy", "Bait target to use escaping ability").SetValue(true)
                    .SetTooltip(
                        "Tries to gapclose very close to the target first, so the target uses a gapclosing spell too, then you dash onto your enemy."));

            this.Menu.AddItem(
                new MenuItem(this.Name + "minrangetogapclose", "Min distance before gapclosing").SetValue(
                    new Slider((int)ObjectManager.Player.AttackRange, 0, (int)this.ireliaQ.Spell.Range)));
        }

        /// <summary>
        ///     Baits the enemy
        /// </summary>
        private void LogicBaitEnemy()
        {
            if (!this.Menu.Item(this.Name + "baitenemy").GetValue<bool>() || this.target == null
                || ObjectManager.Player.ServerPosition.Distance(this.target.ServerPosition)
                <= this.ireliaQ.Spell.Range - Baitrange) return;

            var possibleUnits = ObjectManager.Get<Obj_AI_Base>().Where(x => this.ireliaQ.WillReset(x));

            this.ireliaQ.Spell.Cast(possibleUnits.MinOrDefault(x => x.Distance(this.target)));
        }

        /// <summary>
        ///     Logic to finish enemies with Q for the reset.
        /// </summary>
        private void LogicFinisher()
        {
            if (this.target == null) return;

            if (this.ireliaQ.WillReset(this.target))
            {
                this.ireliaQ.Spell.Cast(this.target);
            }
        }

        /// <summary>
        ///     Pathfinding
        /// </summary>
        private void LogicPathfinding()
        {
            if (!this.Menu.Item(this.Name + "pathfinding").GetValue<bool>()) return;

            var end = Game.CursorPos;

            if (this.Menu.Item(this.Name + "movementprediction").GetValue<bool>() && this.target != null)
            {
                var gapclosePath = this.ireliaQ.GetPath(ObjectManager.Player.ServerPosition, this.target.ServerPosition);

                var expectedTime =
                    gapclosePath.TakeWhile((t, i) => i != gapclosePath.Count - 1)
                        .Select((t, i) => t.Distance(gapclosePath[i + 1]))
                        .Sum() / this.ireliaQ.Spell.Speed;

                var pred = Prediction.GetPrediction(this.target, expectedTime);

                if (pred != null)
                {
                    end = pred.CastPosition;
                }
            }

            var path = this.ireliaQ.GetPath(ObjectManager.Player.ServerPosition, end);

            this.ireliaQ.Spell.Cast(path.FirstOrDefault());
        }

        /// <summary>
        ///     Raises the <see cref="E:GameUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnGameUpdate(EventArgs args)
        {
            if (!this.CheckGuardians()) return;

            this.target = TargetSelector.GetTarget(1000, TargetSelector.DamageType.Physical, false);

            this.LogicFinisher();

            this.LogicPathfinding();

            if (this.target == null) return;

            if (ObjectManager.Player.Distance(this.target)
                <= this.Menu.Item(this.Name + "minrangetogapclose").GetValue<Slider>().Value) return;

            this.LogicBaitEnemy();
        }

        #endregion
    }
}