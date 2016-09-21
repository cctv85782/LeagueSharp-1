using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rethought_Clicks
{
    using RethoughtLib.Bootstraps.Implementations;

    using Rethought_Clicks.Modules;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var bootstrap = new LeagueSharpMultiBootstrap();

            bootstrap.AddModule(new Loader());
            bootstrap.AddString("Version_" + bootstrap.Modules.Count);

            bootstrap.Run();
        }
    }
}
