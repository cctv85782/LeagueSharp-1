namespace Rethought_Fiora.Champions.Fiora.Modules.Core.TargetSelectorModule
{
    #region Using Directives

    using LeagueSharp;

    #endregion

    public interface ITargetRetrieveMethod
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <returns></returns>
        Obj_AI_Hero GetTarget();

        #endregion
    }
}