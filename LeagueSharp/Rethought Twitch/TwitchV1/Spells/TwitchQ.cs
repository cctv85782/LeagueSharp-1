namespace Rethought_Twitch.TwitchV1.Spells
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Algorithm.Graphs;
    using RethoughtLib.Algorithm.Pathfinding.AStar;
    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.FeatureSystem.Switches;

    using Rethought_Twitch.TwitchV1.DamageCalculator;
    using Rethought_Twitch.TwitchV1.Drawings;
    using Rethought_Twitch.TwitchV1.GraphGenerator;
    using Rethought_Twitch.TwitchV1.Pathfinder;

    using SharpDX;

    #endregion

    internal class TwitchQ : SpellChild, IDamageCalculatorModule
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the estimated amount in one combo.
        /// </summary>
        /// <value>
        /// The estimated amount in one combo.
        /// </value>
        public int EstimatedAmountInOneCombo { get; set; } = 1;

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Ambush";

        /// <summary>
        ///     Gets or sets the spell.
        /// </summary>
        /// <value>
        ///     The spell.
        /// </value>
        public override Spell Spell { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the damage.
        /// </summary>
        /// <param name="target">The get damage.</param>
        /// <returns></returns>
        public float GetDamage(Obj_AI_Base target)
        {
            return 0;
        }

        /// <summary>
        ///     Whether this instance will reset the spell on the specified target
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public bool WillReset(Obj_AI_Base target)
        {
            // TODO HEALTHPREDICTION

            return false;
        }

        /// <summary>
        /// Gets the remaining time.
        /// </summary>
        /// <returns></returns>
        public float GetRemainingTime()
        {
            // TODO GET BUFF

            return 0;
        }

        public BuffInstance GetBuff()
        {
            // TODO GET BUFF

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Spell = new Spell(SpellSlot.Q);
        }

        /// <summary>
        ///     Sets the switch.
        /// </summary>
        protected override void SetSwitch()
        {
            this.Switch = new UnreversibleSwitch(this.Menu);
        }

        #endregion
    }
}