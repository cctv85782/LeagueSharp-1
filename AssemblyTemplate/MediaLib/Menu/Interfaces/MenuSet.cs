namespace AssemblyName.MediaLib.Menu.Interfaces
{
    using LeagueSharp.Common;

    public interface IMenuSet
    {
        Menu Menu { get; set; }

        void Generate();
    }
}
