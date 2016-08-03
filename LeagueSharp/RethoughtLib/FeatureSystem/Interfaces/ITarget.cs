namespace RethoughtLib.FeatureSystem.Interfaces
{
    #region Using Directives

    using LeagueSharp;

    #endregion

    public interface ITarget
    {
        #region Public Properties

        Obj_AI_Hero Target { get; set; }

        #endregion
    }
}