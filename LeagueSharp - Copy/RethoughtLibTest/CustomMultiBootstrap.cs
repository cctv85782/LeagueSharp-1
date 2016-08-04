using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RethoughtLibTest
{
    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.Bootstraps.Implementations;
    using RethoughtLib.Classes.General_Intefaces;

    internal class CustomMultiBootstrap : LeagueSharpMultiBootstrap
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LeagueSharpMultiBootstrap" /> class.
        /// </summary>
        public CustomMultiBootstrap(List<LoadableBase> modules, List<string> additionalStrings = null)
            : base(modules, additionalStrings)
        {
            // do something here
        }

        // override some members here or add something
    }
}
