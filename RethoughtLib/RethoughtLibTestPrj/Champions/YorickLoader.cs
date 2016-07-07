namespace RethoughtLibTestPrj.Champions
{
    using System;

    using RethoughtLib.Classes.Intefaces;

    internal class YorickLoader : ILoadable
    {
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; } = "Yorick";

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public void Load()
        {
            Console.WriteLine("Yorick loaded!");
        }
    }
}
