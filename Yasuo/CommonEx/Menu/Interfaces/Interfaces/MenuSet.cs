namespace Yasuo.CommonEx.Menu.Interfaces.Interfaces
{
    using LeagueSharp.Common;

    public interface IMenuSet
    {
        Menu Menu { get; set; }

        void Generate();
    }
}
