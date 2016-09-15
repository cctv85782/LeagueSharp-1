namespace Rethought_Irelia.IreliaV1.DamageCalculator
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal class AutoAttacks : IDamageCalculatorModule
    {
        #region Public Properties

        public int EstimatedAmountInOneCombo { get; } = 2;

        public string Name { get; set; } = "Auto-Attacks";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the damage.
        /// </summary>
        /// <param name="target">The get damage.</param>
        /// <returns></returns>
        public float GetDamage(Obj_AI_Base target)
        {
            return (float)ObjectManager.Player.GetAutoAttackDamage(target);
        }

        #endregion
    }
}