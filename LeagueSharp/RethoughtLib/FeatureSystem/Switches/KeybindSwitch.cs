﻿namespace RethoughtLib.FeatureSystem.Switches
{
    #region Using Directives

    using System;

    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    /// <summary>
    ///     Switch that displays a bool that can get named
    /// </summary>
    /// <seealso cref="RethoughtLib.FeatureSystem.Switches.SwitchBase" />
    public class KeybindSwitch : SwitchBase
    {
        #region Fields

        /// <summary>
        ///     The owner
        /// </summary>
        private readonly Base owner;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BoolSwitch" /> class.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="boolName">Name of the bool.</param>
        /// <param name="owner">The owner.</param>
        public KeybindSwitch(Menu menu, string boolName, char key, Base owner)
            : base(menu)
        {
            this.BoolName = boolName;
            this.Key = key;
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
        public char Key { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Setups this instance.
        /// </summary>
        public override void Setup()
        {
            this.Menu.AddItem(new MenuItem(this.owner.Name + this.BoolName, this.BoolName).SetValue(new KeyBind(this.Key, KeyBindType.Toggle)))
                .ValueChanged += delegate(object sender, OnValueChangeEventArgs args)
                    {
                        if (args.GetNewValue<KeyBind>().Active)
                        {
                            this.Enable(new Base.FeatureBaseEventArgs(this.owner));
                        }
                        else
                        {
                            this.Disable(new Base.FeatureBaseEventArgs(this.owner));
                        }
                    };

            if (this.Menu.Item(this.owner.Name + this.BoolName).GetValue<KeyBind>().Active)
            {
                this.Enable(new Base.FeatureBaseEventArgs(this.owner));
            }
            else
            {
                this.Disable(new Base.FeatureBaseEventArgs(this.owner));
            }
        }

        #endregion
    }
}