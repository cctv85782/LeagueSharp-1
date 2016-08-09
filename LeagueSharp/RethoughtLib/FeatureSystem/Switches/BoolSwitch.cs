﻿namespace RethoughtLib.FeatureSystem.Switches
{
    #region Using Directives

    using global::RethoughtLib.FeatureSystem.Abstract_Classes;

    using LeagueSharp.Common;

    #endregion

    internal class BoolSwitch : SwitchBase
    {
        #region Fields

        private readonly Base owner;

        private bool checkedDisabled;

        private bool checkedEnabled;

        private Base.FeatureBaseEventArgs onOnDisableEventCache;

        private Base.FeatureBaseEventArgs onOnEnableEventCache;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BoolSwitch" /> class.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="boolName">Name of the bool.</param>
        /// <param name="boolValue">if set to <c>true</c> [bool value].</param>
        public BoolSwitch(Menu menu, string boolName, bool boolValue, Base owner)
            : base(menu)
        {
            this.BoolName = boolName;
            this.BoolValue = boolValue;
            this.owner = owner;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name of the bool.
        /// </summary>
        /// <value>
        ///     The name of the bool.
        /// </value>
        public string BoolName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [bool value].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [bool value]; otherwise, <c>false</c>.
        /// </value>
        public bool BoolValue { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Raises the <see cref="E:OnDisableEvent" /> event.
        /// </summary>
        /// <param name="e">The <see cref="Base.FeatureBaseEventArgs" /> instance containing the event data.</param>
        public override void OnOnDisableEvent(Base.FeatureBaseEventArgs e)
        {
            if (!this.checkedDisabled)
            {
                this.checkedDisabled = true;
                this.onOnDisableEventCache = e;
                this.Menu.Item(this.BoolName).SetValue(false);
            }
            else
            {
                this.checkedDisabled = false;

                e = this.onOnDisableEventCache;

                base.OnOnDisableEvent(e);
            }
        }

        /// <summary>
        ///     Raises the <see cref="E:OnEnableEvent" /> event.
        /// </summary>
        /// <param name="e">The <see cref="Base.FeatureBaseEventArgs" /> instance containing the event data.</param>
        public override void OnOnEnableEvent(Base.FeatureBaseEventArgs e)
        {
            if (!this.checkedEnabled)
            {
                this.checkedEnabled = true;

                this.onOnEnableEventCache = e;
                this.Menu.Item(this.BoolName).SetValue(true);
            }
            else
            {
                this.checkedEnabled = false;

                e = this.onOnEnableEventCache;

                base.OnOnEnableEvent(e);
            }
        }

        /// <summary>
        ///     Setups this instance.
        /// </summary>
        public override void Setup()
        {
            this.Menu.AddItem(new MenuItem(this.BoolName, this.BoolName).SetValue(this.BoolValue)).ValueChanged +=
                delegate(object sender, OnValueChangeEventArgs args)
                    {
                        if (args.GetNewValue<bool>())
                        {
                            this.OnOnEnableEvent(new Base.FeatureBaseEventArgs(this.owner));
                        }
                        else if (!args.GetNewValue<bool>())
                        {
                            this.OnOnDisableEvent(new Base.FeatureBaseEventArgs(this.owner));
                        }
                    };

            this.Enabled = this.Menu.Item(this.BoolName).GetValue<bool>();

            if (this.Enabled)
            {
                this.OnOnEnableEvent(new Base.FeatureBaseEventArgs(this.owner));
            }
            else
            {
                this.OnOnDisableEvent(new Base.FeatureBaseEventArgs(this.owner));
            }
        }

        #endregion
    }
}