namespace Rethought_Fiora
{
    #region Using Directives

    using RethoughtLib;
    using RethoughtLib.Bootstraps.Implementations;

    using Rethought_Fiora.Champions.Fiora;

    #endregion

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            RethoughtLib.Instance.Load();

            var bootstrap = new LeagueSharpMultiBootstrap();

            bootstrap.AddModule(new FioraLoader());
            bootstrap.AddString("Fiora");
        }

        #endregion
    }
}