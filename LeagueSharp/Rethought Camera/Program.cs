namespace Rethought_Camera
{
    #region Using Directives

    using RethoughtLib.Bootstraps.Implementations;

    #endregion

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            var bootstrap = new LeagueSharpMultiBootstrap();

            bootstrap.AddModule(new CameraLoader());
            bootstrap.AddString("Utility");

            bootstrap.Run();
        }

        #endregion
    }
}