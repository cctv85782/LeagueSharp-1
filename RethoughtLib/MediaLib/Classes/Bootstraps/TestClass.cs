namespace RethoughtLib.Classes.Bootstraps
{
    #region Using Directives

    using System;

    using LeagueSharp.Common;

    using Interfaces;

    #endregion

#if DEBUG
    internal class TestClass
    {
        #region Public Methods and Operators

        public void TestVoid()
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        #endregion

        #region Methods

        private static void OnLoad(EventArgs args)
        {
            var bootstrap = new Bootstrap();

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