namespace RethoughtLibTest
{
    #region Using Directives

    using System.Collections.Generic;

    using RethoughtLib;
    using RethoughtLib.Bootstraps.Abstract_Classes;

    using RethoughtLibTest.Champions.Yorick;

    #endregion

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            RethoughtLib.Instance.Load();

            var bootstrap = new CustomAioBootstrap(
                // Adds all loadable modules
                new List<LoadableBase>() { new YorickLoader() },

                // Adds keywords that will get loaded, for example everything that has the name Utility
                // this bootstrap will automatically load the module with the champion name of the player!
                new List<string>() { "Yorick" });

            bootstrap.Run();
        }

        #endregion
    }
}