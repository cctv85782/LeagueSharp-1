namespace RethoughtLibTest.Champions.Unknown
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using RethoughtLib.Bootstraps.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    using RethoughtLibTest.Champions.Unknown.Childs;

    #endregion

    internal class UnknownLoader : LoadableBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name of the displaying.
        /// </summary>
        /// <value>
        ///     The name of the displaying.
        /// </value>
        public override string DisplayName { get; set; } = "My Unknown Assembly";

        /// <summary>
        ///     Gets or sets the internal name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string InternalName { get; set; } = "Unknown";

        /// <summary>
        ///     Gets or sets the tags.
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public override IEnumerable<string> Tags { get; set; } = new List<string>() { "Always" };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public override void Load()
        {
            Console.WriteLine("Loading Yorick");
            /* do something here to load your assembly
             * for example you could do this > */

            // basically the root of our structure (menu-wise)
            var superParent = new SuperParent("SuperParent");

            // something like a combo menu
            var comboParent = new Parent("ComboParent");

            // our actual feature containing the logic
            var child = new ExampleChild("Q");
            var child2 = new ExampleChild("W");
            var child3 = new ExampleChild("E");
            var child4 = new ExampleChild("R");

            // assigns "Combo" to "My Yorick Assembly"
            superParent.AddChild(comboParent);

            // assigns the feature to "Combo"
            comboParent.AddChild(child);
            comboParent.AddChild(child2);
            comboParent.AddChild(child3);
            comboParent.AddChild(child4);

            // loads everything
            superParent.OnLoadInvoker();
        }

        #endregion
    }
}