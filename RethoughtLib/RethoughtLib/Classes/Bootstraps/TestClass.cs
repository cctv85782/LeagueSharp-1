namespace RethoughtLib.Classes.Bootstraps
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp.Common;

    using global::RethoughtLib.Classes.Bootstraps.Interfaces;
    using global::RethoughtLib.Classes.Intefaces;

    #endregion

#if DEBUG
    internal class TestClass
    {
        #region Public Methods and Operators

        public void TestVoid()
        {
            CustomEvents.Game.OnGameLoad += OnLoad;

            var automaticBootstrap = new LeagueSharpAutoBootstrap(new List<ILoadable>() { new NunuModule() });
        }

        #endregion

        #region Methods

        private static void OnLoad(EventArgs args)
        {
            var bootstrap = new PlaySharpBootstrap();

            bootstrap.AddModule(new NunuModule());
            bootstrap.AddString("Nunu");

            bootstrap.Initialize();
        }

        #endregion

        private class NunuModule : ILoadable
        {
            #region Public Properties

            /// <summary>
            ///     Gets or sets the name.
            /// </summary>
            /// <value>
            ///     The name.
            /// </value>
            public string Name { get; set; } = "Nunu";

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Loads this instance.
            /// </summary>
            public void Load()
            {
                Console.WriteLine($"Hey, I loaded {this.Name}!");
            }

            #endregion
        }
    }
#endif
}