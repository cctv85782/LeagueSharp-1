namespace RethoughtLibTestPrj
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using RethoughtLib.Classes.Bootstraps;
    using RethoughtLib.Classes.Intefaces;

    using RethoughtLibTestPrj.Champions;
    using RethoughtLibTestPrj.Utilities;

    #endregion

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            var bootstrap =
                new LeagueSharpAutoChampionBootstrap(new List<ILoadable>() { new NunuLoader(), new YorickLoader(), new ChatLoggerTest()});
        }

        #endregion
    }
}