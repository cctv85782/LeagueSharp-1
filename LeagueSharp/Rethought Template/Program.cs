using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rethought_Template
{
    using RethoughtLib.Bootstraps.Implementations;

    using Rethought_Kayle.KayleV1;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var bootstrap = new LeagueSharpMultiBootstrap();

            bootstrap.AddString();
            bootstrap.AddModule(new Loader());

            bootstrap.Run();
        }
    }
}
