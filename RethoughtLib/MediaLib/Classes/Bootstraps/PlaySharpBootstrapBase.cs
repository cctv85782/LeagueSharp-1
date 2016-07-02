﻿#region Using Directives



#endregion

namespace RethoughtLib.Classes.Bootstraps
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RethoughtLib.Classes.Bootstraps.Interfaces;

    #endregion

    public abstract class PlaySharpBootstrapBase : IBootstrap
    {
        #region Fields

        /// <summary>
        ///     Gets or sets the string that gets checked for check for.
        /// </summary>
        /// <value>
        ///     The check for.
        /// </value>
        protected List<string> Strings = new List<string>();

        /// <summary>
        ///     The modules
        /// </summary>
        protected List<ILoadable> Modules = new List<ILoadable>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the module.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <exception cref="ArgumentException">There can't be multiple similiar modules in the Bootstrap.</exception>
        public virtual void AddModule(ILoadable module)
        {
            if (this.Modules.Contains(module))
            {
                throw new ArgumentException("There can't be multiple similiar modules in the Bootstrap.");
            }

            this.Modules.Add(module);
        }

        /// <summary>
        ///     Adds the module.
        /// </summary>
        /// <param name="modules">the modules</param>
        /// <exception cref="ArgumentException">There can't be multiple similiar modules in the Bootstrap.</exception>
        public virtual void AddModules(IEnumerable<ILoadable> modules)
        {
            var loadables = modules as IList<ILoadable> ?? modules.ToList();

            if ((from moduleToAdd in loadables
                 from existingModule in this.Modules
                 where moduleToAdd.Equals(existingModule)
                 select moduleToAdd).Any())
            {
                throw new ArgumentException("There can't be multiple similiar modules in the Bootstrap.");
            }

            this.Modules.AddRange(loadables);
        }

        /// <summary>
        ///     Adds a string with witch the bootstrap is checking for modules.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void AddString(string value)
        {
            if (this.Strings.Contains(value))
            {
                return;
            }

            this.Strings.Add(value);
        }

        /// <summary>
        ///     Adds strings with witch the bootstrap is checking for modules.
        /// </summary>
        /// <param name="values">the values</param>
        public virtual void AddStrings(IEnumerable<string> values)
        {
            var validValues =
                this.Strings.SelectMany(@string => values, (@string, value) => new { @string, value })
                    .Where(@t => !@t.@string.Equals(@t.value))
                    .Select(@t => @t.value)
                    .ToList();

            this.Strings.AddRange(validValues);
        }

        /// <summary>
        ///     Compares module names with entries in the strings list. If they match it will load the module.
        /// </summary>
        public virtual void Initialize()
        {
            if (!this.Modules.Any())
            {
                throw new InvalidOperationException(
                    "There are no modules in the Bootstrap to load.");
            }

            if (!this.Strings.Any())
            {
                throw new InvalidOperationException(
                    "There are no strings in the Bootstrap to make a check with modules.");
            }

            foreach (var module in this.Modules)
            {
                foreach (var @string in this.Strings)
                {
                    if (module.Name.Equals(@string))
                    {
                        module.Load();
                    }
                }
            }
        }

        /// <summary>
        ///     Removes the module.
        /// </summary>
        /// <param name="module">The module.</param>
        public virtual void RemoveModule(ILoadable module)
        {
            if (!this.Modules.Contains(module))
            {
                return;
            }

            this.Modules.Remove(module);
        }

        /// <summary>
        ///     Removes a string with witch the bootstrap was checking for modules.
        /// </summary>
        /// <param name="value">The value.</param>
        public virtual void RemoveString(string value)
        {
            if (!this.Strings.Contains(value))
            {
                return;
            }

            this.Strings.Remove(value);
        }

        /// <summary>
        ///     Removes strings with witch the bootstrap was checking for modules.
        /// </summary>
        /// <param name="values">the values</param>
        public virtual void RemoveStrings(IEnumerable<string> values)
        {
            var strings = values as IList<string> ?? values.ToList();

            foreach (var @string in this.Strings)
            {
                foreach (var value in strings)
                {
                    if (@string.Equals(value))
                    {
                        this.Strings.Remove(value);
                    }
                }
            }
        }

        #endregion
    }
}