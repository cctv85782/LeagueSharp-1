//     Copyright (C) 2016 Rethought
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
//     Created: 04.10.2016 1:05 PM
//     Last Edited: 04.10.2016 1:44 PM

namespace Rethought_Twitch.TwitchV1.Spells
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.DamageCalculator;
    using RethoughtLib.FeatureSystem.Implementations;
    using RethoughtLib.FeatureSystem.Switches;

    #endregion

    internal class TwitchE : SpellChild, IDamageCalculatorModule
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the estimated amount in one combo.
        /// </summary>
        /// <value>
        ///     The estimated amount in one combo.
        /// </value>
        public int EstimatedAmountInOneCombo { get; set; } = 1;

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Contaminate";

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
        ///     Determines whether this instance can stun the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public bool CanKill(Obj_AI_Base target)
        {
            if (this.Menu.Item(this.Path + "." + "usehealthprediction").GetValue<bool>())
            {
                var predictedEnemyHealth = HealthPrediction.GetHealthPrediction(
                    target,
                    0,
                    (int)(this.Spell.Delay + this.Spell.Speed / ObjectManager.Player.Distance(target)));

                return predictedEnemyHealth <= this.GetDamage(target);
            }

            return target.Health <= this.GetDamage(target);
        }

        /// <summary>
        ///     Gets the damage.
        /// </summary>
        /// <param name="target">The get damage.</param>
        /// <returns></returns>
        public float GetDamage(Obj_AI_Base target)
        {
            return !this.Spell.IsReady() ? 0 : this.Spell.GetDamage(target);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        /// <param name="sender">todo: describe sender parameter on OnLoad</param>
        /// <param name="eventArgs">todo: describe eventArgs parameter on OnLoad</param>
        protected override void OnLoad(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnLoad(sender, eventArgs);

            this.Spell = new Spell(SpellSlot.E, 1200, TargetSelector.DamageType.Physical);
            this.Spell.SetTargetted(0.1f, 2500);

            this.Menu.AddItem(
                new MenuItem(this.Path + "." + "usehealthprediction", "Use HealthPrediction").SetValue(true));
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