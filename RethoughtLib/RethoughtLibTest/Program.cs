namespace RethoughtLibTest
{
    #region Using Directives

    using System.Collections.Generic;

    using RethoughtLib;
    using RethoughtLib.Classes.Bootstraps;
    using RethoughtLib.Classes.Intefaces;
    using RethoughtLib.Utility;

    using RethoughtLibTest.Champions;
    using RethoughtLibTest.Champions.Nunu;
    using RethoughtLibTest.Utilities;

    #endregion

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            RethoughtLib.Instance.Load();

            var bootstrap =
                new LeagueSharpAutoBootstrap(
                    new List<ILoadable>()
                        {
                            new NunuLoader(),
                            new YorickLoader(),
                            new ChatLoggerTest()
                        },
                    new List<string>() { "Utility" });

        }

        #endregion
    }
}