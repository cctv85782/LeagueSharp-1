namespace RethoughtLib.FeatureSystem.Abstract_Classes
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public abstract class ParentBase : Base
    {
        #region Fields

        public readonly Dictionary<Base, bool> Children = new Dictionary<Base, bool>();

        #endregion

        #region Public Events

        /// <summary>
        ///     Occurs when [on disable event].
        /// </summary>
        public event EventHandler<ParentBaseEventArgs> OnChildAddEvent;

        /// <summary>
        ///     Occurs when [on disable event].
        /// </summary>
        public event EventHandler<ParentBaseEventArgs> OnChildRemoveEvent;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the child.
        /// </summary>
        /// <param name="child">The child.</param>
        public void Add(Base child)
        {
            this.OnChildAddInvoker(new ParentBaseEventArgs() { Child = child });
        }

        /// <summary>
        ///     Adds the children.
        /// </summary>
        /// <param name="children">The children.</param>
        public void Add(IEnumerable<Base> children)
        {
            foreach (var child in children)
            {
                this.Add(child);
            }
        }

        /// <summary>
        ///     Removes the children.
        /// </summary>
        /// <param name="child">The child.</param>
        public void Remove(Base child)
        {
            if (!this.Children.ContainsKey(child))
            {
                throw new InvalidOperationException(
                    $"{this}, can't remove a child from the list children that has not been added to the list.");
            }

            this.Children.Remove(child);
            child.OnUnLoadInvoker();
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [uninitialize].
        /// </summary>
        protected internal override void OnTerminate(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnTerminate(sender, featureBaseEventArgs);

            this.OnChildAddEvent -= this.OnChildAdd;

            foreach (var child in this.Children.Keys.ToList())
            {
                child.OnTerminateInvoker();
            }
        }

        /// <summary>
        ///     Merges the child with another children with the same Name
        /// </summary>
        /// <param name="child">The child.</param>
        protected virtual void MergeChild(Base child)
        {
            foreach (var menuEntry in child.Menu.Items)
            {
                if (this.Menu.SubMenu(child.Menu.Name).Items.Contains(menuEntry))
                {
                    Console.WriteLine("Merging");
                    continue;
                }

                this.Menu.SubMenu(child.Menu.Name).AddItem(menuEntry);
            }

            child.Menu = this.Menu.SubMenu(child.Menu.Name);
        }

        /// <summary>
        ///     Called when [child add].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="parentBaseEventArgs">The <see cref="ParentBaseEventArgs" /> instance containing the event data.</param>
        protected virtual void OnChildAdd(object sender, ParentBaseEventArgs parentBaseEventArgs)
        {
            var child = parentBaseEventArgs.Child;

            child.OnInitializeInvoker();

            this.Children.Add(child, child.Switch.Enabled);
        }

        /// <summary>
        ///     Raises the <see cref="E:ChildAddInvoker" /> event.
        /// </summary>
        /// <param name="eventArgs">The <see cref="ParentBaseEventArgs" /> instance containing the event data.</param>
        protected virtual void OnChildAddInvoker(ParentBaseEventArgs eventArgs)
        {
            if (!this.Initialized)
            {
                this.OnInitializeInvoker();
            }

            this.OnChildAddEvent?.Invoke(this, eventArgs);
        }

        /// <summary>
        ///     Called when [child disabled].
        ///     Default Behavior:
        ///     > if the sender is the parent do nothing
        ///     > else if the sender is a child and all children are disabled then the parent will get disabled if it was enabled
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="featureBaseEventArgs"></param>
        protected virtual void OnChildDisabled(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Console.WriteLine($"Parent {this}: Child Disabled, Sender: " + featureBaseEventArgs.Sender);

            var child = featureBaseEventArgs.Sender;

            if (child == null || child.Equals(this))
            {
                return;
            }

            this.Children[child] = child.Switch.Enabled;

            if (this.Children.Any(x => x.Value)) return;

            this.Switch.OnOnDisableEvent(new FeatureBaseEventArgs(this));
        }

        /// <summary>
        ///     Called when [child enabled].
        ///     Default Behavior:
        ///     > if the sender is the parent do nothing
        ///     > else if the sender is a child enable the parent if the parent was disabled
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="featureBaseEventArgs"></param>
        protected virtual void OnChildEnabled(object o, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Console.WriteLine($"Parent {this}: Child Enabled, Sender: " + featureBaseEventArgs.Sender);

            var child = featureBaseEventArgs.Sender;

            if (child == null || child == this)
            {
                return;
            }

            this.Children[child] = child.Switch.Enabled;

            if (this.Switch.Enabled) return;

            this.Switch.OnOnEnableEvent(new FeatureBaseEventArgs(this) { Receiver = this});
        }

        /// <summary>
        ///     Called when [child remove].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="parentBaseEventArgs">The <see cref="ParentBaseEventArgs" /> instance containing the event data.</param>
        protected virtual void OnChildRemove(object sender, ParentBaseEventArgs parentBaseEventArgs)
        {
            this.Children.Remove(parentBaseEventArgs.Child);

            parentBaseEventArgs.Child.OnTerminateInvoker();

            this.Menu.RemoveMenu(parentBaseEventArgs.Child.Menu);
        }

        /// <summary>
        ///     Raises the <see cref="E:ChildRemoveInvoker" /> event.
        /// </summary>
        /// <param name="eventArgs">The <see cref="ParentBaseEventArgs" /> instance containing the event data.</param>
        protected virtual void OnChildRemoveInvoker(ParentBaseEventArgs eventArgs)
        {
            this.OnChildRemoveEvent?.Invoke(this, eventArgs);
        }

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Console.WriteLine($"Parent {this} > OnDisable");

            foreach (var child in this.Children.ToList())
            {
                if (!child.Key.Switch.Enabled)
                {
                    continue;
                }

                Console.WriteLine($"Parent {this} > OnEnable: Disabling " + child);

                this.Children[child.Key] = true;

                child.Key.Switch.OnOnDisableEvent(new FeatureBaseEventArgs(this));
            }
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Console.WriteLine($"Parent {this} > OnEnable");

            foreach (var child in this.Children.ToList())
            {
                if (!child.Value)
                {
                    continue;
                }

                Console.WriteLine($"Parent {this} > OnEnable: Enabling " + child);

                child.Key.Switch.OnOnEnableEvent(new FeatureBaseEventArgs(this));
            }
        }

        /// <summary>
        ///     Called when [initialize].
        /// </summary>
        protected override void OnInitialize(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnInitialize(sender, featureBaseEventArgs);

            this.OnChildAddEvent += this.OnChildAdd;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            foreach (var keyValuePair in this.Children.ToList())
            {
                Console.WriteLine("Loading " + keyValuePair.Key.Name);
                var child = keyValuePair.Key;

                child.OnLoadInvoker();

                child.Switch.OnEnableEvent += this.OnChildEnabled;
                child.Switch.OnDisableEvent += this.OnChildDisabled;

                // This Parents Menu already contains a Menu with the same name
                if (this.Menu.Children.Contains(child.Menu))
                {
                    Console.WriteLine($"Merged {child.Name} to {this.Name}'s menu");

                    this.MergeChild(child);
                }
                else
                {
                    Console.WriteLine($"Added {child.Name} to {this.Name}'s menu");
                    this.Menu.AddSubMenu(child.Menu);
                }
            }
        }

        /// <summary>
        ///     Called when [refresh].
        /// </summary>
        protected override void OnRefresh(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnRefresh(sender, featureBaseEventArgs);

            foreach (var child in this.Children.ToList())
            {
                child.Key.OnRefreshInvoker();
            }
        }

        /// <summary>
        ///     Called when [unload].
        /// </summary>
        protected override void OnUnload(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnUnload(sender, featureBaseEventArgs);

            foreach (var child in this.Children.ToList())
            {
                child.Key.OnUnLoadInvoker();
            }
        }

        #endregion

        /// <summary>
        ///     Custom Event Args.
        /// </summary>
        /// <seealso cref="System.EventArgs" />
        public class ParentBaseEventArgs : EventArgs
        {
            #region Public Properties

            public Base Child { get; set; }

            #endregion
        }
    }
}