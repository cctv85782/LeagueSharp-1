namespace RethoughtLib.Classes.FeatureV2.Abstract_Classes
{
    #region Using Directives

    using System;

    using global::RethoughtLib.Classes.Intefaces;

    using LeagueSharp.Common;

    #endregion

    /// <summary>
    ///     EventHandler with no arguments
    /// </summary>
    public delegate void EventHandler();

    /// <summary>
    ///     Class that represents the base for parents and children.
    /// </summary>
    /// <seealso cref="RethoughtLib.Classes.Intefaces.INamable" />
    public abstract class Base : INamable
    {
        #region Public Events

        /// <summary>
        ///     Occurs when [on disable event].
        /// </summary>
        public event EventHandler OnDisableEvent;

        /// <summary>
        ///     Occurs when [on enable event].
        /// </summary>
        public event EventHandler OnEnableEvent;

        /// <summary>
        ///     Occurs when [on initialize event].
        /// </summary>
        public event EventHandler OnInitializeEvent;

        /// <summary>
        ///     Occurs when [on load event].
        /// </summary>
        public event EventHandler OnLoadEvent;

        /// <summary>
        ///     Occurs when [on refresh event].
        /// </summary>
        public event EventHandler OnRefreshEvent;

        /// <summary>
        ///     Occurs when [on initialize event].
        /// </summary>
        public event EventHandler OnTerminateEvent;

        /// <summary>
        ///     Occurs when [on unload event].
        /// </summary>
        public event EventHandler OnUnLoadEvent;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="Base" /> is enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public virtual bool Enabled { get; set; } = true;

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="Base" /> is initialized.
        /// </summary>
        /// <value>
        ///     <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        public bool Initialized { get; protected internal set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="Base" /> is loaded.
        /// </summary>
        /// <value>
        ///     <c>true</c> if loaded; otherwise, <c>false</c>.
        /// </value>
        public bool Loaded { get; protected internal set; } = false;

        /// <summary>
        ///     Gets or sets the menu.
        /// </summary>
        /// <value>
        ///     The menu.
        /// </value>
        public Menu Menu { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public virtual string Name { get; set; } = "Unknown";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Delegates the events.
        /// </summary>
        /// <param name="unsubscribe">if set to <c>true</c> [unsubscribe].</param>
        public virtual void DelegateEvents(bool unsubscribe = false)
        {
            if (!unsubscribe)
            {
                this.OnDisableEvent += this.OnDisable;
                this.OnEnableEvent += this.OnEnable;
                this.OnInitializeEvent += this.OnInitialize;
                this.OnLoadEvent += this.OnLoad;
                this.OnUnLoadEvent += this.OnUnload;
                this.OnRefreshEvent += this.OnRefresh;
                this.OnTerminateEvent += this.OnTerminate;
            }
            else if (this.Initialized)
            {
                this.OnDisableEvent -= this.OnDisable;
                this.OnEnableEvent -= this.OnEnable;
                this.OnInitializeEvent -= this.OnInitialize;
                this.OnLoadEvent -= this.OnLoad;
                this.OnUnLoadEvent -= this.OnUnload;
                this.OnRefreshEvent -= this.OnRefresh;
                this.OnTerminateEvent -= this.OnTerminate;
            }
        }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public virtual void Initialize()
        {
            this.DelegateEvents();
        }

        /// <summary>
        ///     Called when [uninitialize].
        /// </summary>
        public virtual void OnTerminate()
        {
            this.Terminate();
        }

        /// <summary>
        ///     Creates the menu.
        /// </summary>
        public virtual void SetupMenu()
        {
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Menu.Item(this.Name + "Enabled").ValueChanged += delegate(object sender, OnValueChangeEventArgs args)
                {
                    if (args.GetNewValue<bool>())
                    {
                        this.OnEnableInvoker();
                    }
                    else
                    {
                        this.OnDisableInvoker();
                    }
                };
        }

        /// <summary>
        ///     Terminates this instance.
        /// </summary>
        public virtual void Terminate()
        {
            this.OnDisableInvoker();
            this.OnUnLoadInvoker();
            this.DelegateEvents(true);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [on disable event].
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// </exception>
        protected internal void OnDisableInvoker()
        {
            if (!this.Initialized)
            {
                throw new InvalidOperationException(
                    $"{this}, can't invoke OnDisableEvent if {this} was not initialized yet.");
            }

            if (!this.Enabled)
            {
                throw new InvalidOperationException(
                    $"{this}, can't invoke OnDisableEvent if {this} was already disabled.");
            }

            if (!this.Loaded)
            {
                throw new InvalidOperationException(
                    $"{this}, can't invoke OnDisableEvent if {this} was not loaded yet.");
            }

            if (this.Menu.Item(this.Name + "Enabled") != null)
            {
                this.Menu.Item(this.Name + "Enabled").SetValue(false);
            }

            Console.WriteLine($"{this.Name} OnDisableEvent invoked");
            this.Enabled = false;
            this.OnDisableEvent?.Invoke();
        }

        /// <summary>
        ///     Called when [on enable event].
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// </exception>
        protected internal void OnEnableInvoker()
        {
            if (!this.Initialized)
            {
                throw new InvalidOperationException(
                    $"{this}, can't invoke OnEnableEvent if {this} was not initialized yet.");
            }

            if (this.Enabled)
            {
                throw new InvalidOperationException(
                    $"{this}, can't invoke OnEnableEvent if {this} was already enabled.");
            }

            if (!this.Loaded)
            {
                throw new InvalidOperationException($"{this}, can't invoke OnEnableEvent if {this} was not loaded yet.");
            }

            Console.WriteLine($"{this.Name} OnEnableEvent invoked");
            this.Enabled = true;
            this.OnEnableEvent?.Invoke();
        }

        /// <summary>
        ///     Called when [on initialize event].
        /// </summary>
        protected internal void OnInitializeInvoker()
        {
            if (this.Initialized)
            {
                throw new InvalidOperationException(
                    $"{this}, can't invoke OnInitializeEvent if {this} was already initialized.");
            }

            if (this.Loaded)
            {
                throw new InvalidOperationException(
                    $"{this}, can't invoke OnInitializeEvent if {this} was already loaded.");
            }

            Console.WriteLine($"{this.Name} OnInitializeEvent invoked");
            this.Initialized = true;
            this.OnInitializeEvent?.Invoke();
        }

        /// <summary>
        ///     Called when [on load event].
        /// </summary>
        protected internal void OnLoadInvoker()
        {
            if (!this.Initialized)
            {
                throw new InvalidOperationException(
                    $"{this}, can't invoke OnLoadEvent if {this} it has not been initialized.");
            }

            if (this.Loaded)
            {
                throw new InvalidOperationException(
                    $"{this}, can't invoke OnLoadEvent if {this} it has already been loaded.");
            }

            Console.WriteLine($"{this.Name} OnLoadEvent invoked");
            this.Loaded = true;
            this.OnLoadEvent?.Invoke();
        }

        /// <summary>
        ///     Called when [on refresh event].
        /// </summary>
        protected internal virtual void OnRefreshInvoker()
        {
            if (!this.Initialized)
            {
                throw new InvalidOperationException(
                    $"{this}, can't invoke OnRefreshEvent if {this} it has not been initialized.");
            }

            Console.WriteLine($"{this.Name} OnRefreshEvent invoked");
            this.OnRefreshEvent?.Invoke();
        }

        /// <summary>
        /// Called when [terminate event].
        /// </summary>
        /// <exception cref="System.InvalidOperationException"></exception>
        protected internal virtual void OnTerminateInvoker()
        {
            if (!this.Initialized)
            {
                throw new InvalidOperationException(
                    $"{this}, can't invoke OnTerminateEvent if {this} it has not been initialized.");
            }

            this.OnTerminateEvent?.Invoke();
        }

        /// <summary>
        ///     Called when [on unload event].
        /// </summary>
        protected internal void OnUnLoadInvoker()
        {
            Console.WriteLine($"{this.Name} OnUnloadEvent invoked");
            this.OnUnLoadEvent?.Invoke();
        }

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected virtual void OnDisable()
        {
            Console.WriteLine($"{this.Name} OnDisable triggered");
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected virtual void OnEnable()
        {
            Console.WriteLine($"{this.Name} OnEnable triggered");
        }

        /// <summary>
        ///     Called when [initialize].
        /// </summary>
        protected virtual void OnInitialize()
        {
            Console.WriteLine($"{this.Name} OnInitialize triggered");
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected virtual void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.SetupMenu();

            Console.WriteLine($"{this.Name} OnLoad triggered");
        }

        /// <summary>
        ///     Called when [refresh].
        /// </summary>
        protected virtual void OnRefresh()
        {
            this.OnTerminateInvoker();
            this.OnInitializeInvoker();

            Console.WriteLine($"{this.Name} OnRefresh triggered");
        }

        /// <summary>
        ///     Called when [unload].
        /// </summary>
        protected virtual void OnUnload()
        {
            this.OnDisableInvoker();
            Console.WriteLine($"{this.Name} OnUnload triggered");
        }

        #endregion
    }
}