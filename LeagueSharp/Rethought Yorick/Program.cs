﻿namespace Rethought_Yorick
{
    #region Using Directives

    using RethoughtLib.Bootstraps.Implementations;

    using Rethought_Yorick.YorickV1;

    #endregion

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            var bootstrap = new LeagueSharpMultiBootstrap();

            bootstrap.AddModule(new Loader());
            bootstrap.AddString("Yorick");

            bootstrap.Run();
        }

        #endregion
    }
}