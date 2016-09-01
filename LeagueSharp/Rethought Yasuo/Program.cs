namespace Rethought_Yasuo
{
    #region Using Directives

    using RethoughtLib.Bootstraps.Implementations;
    using RethoughtLib.Core;

    using Rethought_Yasuo.Yasuo;

    #endregion

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            RethoughtLib.Instance.Load();

            var bootstrap = new LeagueSharpMultiBootstrap(new[] { new YasuoLoader() });

            bootstrap.Run();
        }

        #endregion
    }
}