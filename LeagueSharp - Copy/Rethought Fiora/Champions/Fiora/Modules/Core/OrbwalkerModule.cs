using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rethought_Fiora.Champions.Fiora.Modules.Core
{
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal class OrbwalkerModule : ChildBase
    {
        public OrbwalkerModule(Menu mainMenu)
        {
            Orbwalker = new Orbwalking.Orbwalker(mainMenu);
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Unknown";

        /// <summary>
        /// The Orbwalker
        /// </summary>
        public static Orbwalking.Orbwalker Orbwalker;

        /// <summary>
        ///     Initializes the menu, overwrite this method to change the menu.
        /// </summary>
        protected override void CreateMenu()
        {
            return;
        }
    }
}
