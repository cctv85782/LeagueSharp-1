namespace RethoughtLib.TargetValidator.Implementations
{
    using global::RethoughtLib.TargetValidator.Interfaces;

    using LeagueSharp;
    using LeagueSharp.Common;

    public class IsValidTarget : ICheckable
    {
        public bool Check(Obj_AI_Base target)
        {
            return target.IsValidTarget();
        }
    }
}