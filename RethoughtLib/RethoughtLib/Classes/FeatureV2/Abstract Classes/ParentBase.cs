namespace RethoughtLib.Classes.FeatureV2.Abstract_Classes
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp.Common;

    #endregion

    public abstract class ParentBase : Base
    {
        #region Fields

        private readonly List<Base> children = new List<Base>();

        #endregion

        #region Public Methods and Operators

        public void AddChildren(Base child)
        {
            this.children.Add(child);

            child.DelegateEvents();
        }

        public override void Initialize()
        {
            this.OnInitializeInvoker();

            this.OnLoadInvoker();

            this.HandleChildren();

            this.Menu.Item(this.Name + "Enabled").ValueChanged += delegate(object sender, OnValueChangeEventArgs args)
                {
                    if (args.GetNewValue<bool>())
                    {
                        this.Enabled = true;

                        foreach (var child in this.children.Where(child => child.Enabled))
                        {
                            child.OnEnableInvoker();
                        }
                    }
                    else
                    {
                        this.Enabled = false;

                        foreach (var child in this.children.Where(child => child.Enabled))
                        {
                            child.OnDisableInvoker();
                        }
                    }
                };
        }

        /// <summary>
        ///     Called when [uninitialize].
        /// </summary>
        public override void OnTerminate()
        {
            base.OnTerminate();

            foreach (var child in this.children)
            {
                child.OnTerminateInvoker();
            }
        }

        /// <summary>
        ///     Removes the children.
        /// </summary>
        /// <param name="child">The child.</param>
        public void RemoveChildren(Base child)
        {
            if (!this.children.Contains(child))
            {
                throw new InvalidOperationException(
                    $"{this}, can't remove a child from the list children that has not been added to this list.");
            }

            this.children.Remove(child);
            child.OnUnLoadInvoker();
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return "ParentBase " + this.Name;
        }

        #endregion

        #region Methods

        protected virtual void HandleChildren()
        {
            if (!this.children.Any())
            {
                return;
            }

            foreach (var child in this.children)
            {
                if (child == null)
                {
                    continue;
                }

                child.Initialize();

                child.OnLoadInvoker();

                this.Menu.AddSubMenu(child.Menu);
            }
        }

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();

            foreach (var child in this.children)
            {
                child.OnDisableInvoker();
            }
        }

        #endregion
    }
}