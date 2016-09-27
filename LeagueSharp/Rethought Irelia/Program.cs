namespace Rethought_Irelia
{
    #region Using Directives

    using RethoughtLib.Bootstraps.Implementations;

    using IreliaV1;

    #endregion

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            var bootstrap = new LeagueSharpMultiBootstrap();

            bootstrap.AddModule(new Loader());

            bootstrap.Run();
        }

        #endregion
    }
}