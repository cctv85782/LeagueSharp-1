using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rethought_Kayle
{
    using RethoughtLib.Bootstraps.Implementations;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var bootstrap = new LeagueSharpMultiBootstrap();

            bootstrap.AddString("Kayle");
            bootstrap.AddModule();


            bootstrap.Run();
        }
    }
}
