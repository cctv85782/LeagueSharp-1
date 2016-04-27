namespace Yasuo.Common.Classes
{
    using LeagueSharp.Common;

    public abstract class Child<T> : Base, IChild
        where T : Parent
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Child{T}"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        protected Child(T parent)
        {
            this.Parent = parent;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether this <see cref="Child{T}"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public override bool Enabled
        {
            get
            {
                return !this.Unloaded && this.Parent != null && this.Parent.Enabled && this.Menu != null
                       && this.Menu.Item(this.Menu.Name + "Enabled").GetValue<bool>();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Child{T}"/> is handled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if handled; otherwise, <c>false</c>.
        /// </value>
        public bool Handled { get; protected set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public T Parent { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Handles the events.
        /// </summary>
        public void HandleEvents()
        {
            if (this.Parent?.Menu == null || this.Menu == null || this.Handled)
            {
                return;
            }

            this.Parent.Menu.Item(this.Parent.Name + "Enabled").ValueChanged +=
                delegate(object sender, OnValueChangeEventArgs args)
                    {
                        if (!Unloaded && args.GetNewValue<bool>())
                        {
                            if (Menu != null && Menu.Item(Menu.Name + "Enabled").GetValue<bool>())
                            {
                                OnEnable();
                            }
                        }
                        else
                        {
                            OnDisable();
                        }
                    };

            this.Menu.Item(this.Menu.Name + "Enabled").ValueChanged +=
                delegate(object sender, OnValueChangeEventArgs args)
                    {
                        if (!Unloaded && args.GetNewValue<bool>())
                        {
                            if (Parent.Menu != null && Parent.Menu.Item(Parent.Name + "Enabled").GetValue<bool>())
                            {
                                OnEnable();
                            }
                        }
                        else
                        {
                            OnDisable();
                        }
                    };

            if (this.Enabled)
            {
                this.OnEnable();
            }

            this.Handled = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when [load].
        /// </summary>
        protected abstract void OnLoad();

        #endregion
    }
}