namespace Rethought_Twitch.TwitchV1.DamageCalculator
{
    #region Using Directives

    using LeagueSharp;

    #endregion

    public interface IDamageCalculator
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the damage.
        /// </summary>
        /// <param name="target">The get damage.</param>
        /// <returns></returns>
        float GetDamage(Obj_AI_Base target);

        #endregion
    }
}