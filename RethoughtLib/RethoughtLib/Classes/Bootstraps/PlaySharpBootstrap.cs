namespace RethoughtLib.Classes.Bootstraps
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using global::RethoughtLib.Classes.Bootstraps.Abstract_Classes;
    using global::RethoughtLib.Classes.Bootstraps.Interfaces;
    using global::RethoughtLib.Classes.Intefaces;

    #endregion

    public class PlaySharpBootstrap : PlaySharpBootstrapBase
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Adds the module.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <exception cref="ArgumentException">There can't be multiple similiar modules in the PlaySharpBootstrap.</exception>
        public override void AddModule(ILoadable module)
        {
            base.AddModule(module);
        }

        /// <summary>
        ///     Adds the module.
        /// </summary>
        /// <param name="modules">the modules</param>
        /// <exception cref="ArgumentException">There can't be multiple similiar modules in the PlaySharpBootstrap.</exception>
        public override void AddModules(IEnumerable<ILoadable> modules)
        {
            base.AddModules(modules);
        }

        /// <summary>
        ///     Adds a string with witch the bootstrap is checking for modules.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void AddString(string value)
        {
            base.AddString(value);
        }

        /// <summary>
        ///     Adds strings with witch the bootstrap is checking for modules.
        /// </summary>
        /// <param name="values">the values</param>
        public override void AddStrings(IEnumerable<string> values)
        {
            base.AddStrings(values);
        }

        /// <summary>
        ///     Compares module names with entries in the strings list. If they match it will load the module.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        ///     Removes the module.
        /// </summary>
        /// <param name="module">The module.</param>
        public override void RemoveModule(ILoadable module)
        {
            base.RemoveModule(module);
        }

        /// <summary>
        ///     Removes a string with witch the bootstrap was checking for modules.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void RemoveString(string value)
        {
            base.RemoveString(value);
        }

        /// <summary>
        ///     Removes strings with witch the bootstrap was checking for modules.
        /// </summary>
        /// <param name="values">the values</param>
        public override void RemoveStrings(IEnumerable<string> values)
        {
            base.RemoveStrings(values);
        }

        #endregion
    }
}