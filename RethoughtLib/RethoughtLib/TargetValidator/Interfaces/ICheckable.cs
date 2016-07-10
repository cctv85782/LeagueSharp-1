namespace RethoughtLib.TargetValidator.Interfaces
{
    using LeagueSharp;

    public interface ICheckable
    {
        bool Check(Obj_AI_Base target);
    }
}