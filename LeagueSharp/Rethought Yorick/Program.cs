namespace Rethought_Yorick
{
    #region Using Directives

    using RethoughtLib.Bootstraps.Implementations;

    using Rethought_Yorick.YorickV1;

    #endregion

    internal class Program
    {
        #region Methods

        /* TODO GLOBAL:
         *
         * Rethought's Ideas and plans:
         * - Building a base (DONE)
         * - Add Version-Changing on the fly (Rethought's job)
         * - IManaManager for every spell (Rethought's job)
         * - Spell Interrupting with the edge of the W (cage thingy, only if possible)
         * - Logic for LaneClear, LastHit, Mixed
         * NOTE: if you have done LastHit, it will automatically LastHit in LaneClear/Mixed too. You don't need to write this multiple times. You wonder why? Take a look at Loader.cs and then at the OrbwalkingChildren
         * - Damage Calculations for R, and test all other damage calculations
         * - Automatic "converting" from a Spell object into multiple MenuItems which can adjust the converted Spell object. They are linked. (Rethought's job)
         * */

        private static void Main(string[] args)
        {
            var bootstrap = new LeagueSharpMultiBootstrap();

            bootstrap.AddModule(new Loader());
            bootstrap.AddString("Yorick_" + bootstrap.Modules.Count);

            bootstrap.Run();
        }

        #endregion
    }
}