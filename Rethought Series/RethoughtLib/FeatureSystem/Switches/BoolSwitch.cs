﻿//     Copyright (C) 2016 Rethought
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
//     Created: 04.10.2016 1:05 PM
//     Last Edited: 04.10.2016 1:44 PM

namespace RethoughtLib.FeatureSystem.Switches
{
    #region Using Directives

    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    /// <summary>
    ///     Switch that displays a bool that can get named
    /// </summary>
    /// <seealso cref="RethoughtLib.FeatureSystem.Switches.SwitchBase" />
    public class BoolSwitch : SwitchBase
    {
        #region Fields

        /// <summary>
        ///     The owner
        /// </summary>
        private readonly Base owner;

        private Base.FeatureBaseEventArgs Cache;

        #endregion

        #region Constructors and Destructors

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BoolSwitch" /> class.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="boolName">Name of the bool.</param>
        /// <param name="boolValue">if set to <c>true</c> [bool value].</param>
        /// <param name="owner">The owner.</param>
        public BoolSwitch(Menu menu, string boolName, bool boolValue, Base owner)
            : base(menu)
        {
            this.BoolName = boolName;
            this.BoolValue = boolValue;
            this.owner = owner;
        }

        #endregion

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
        public override void Disable(Base.FeatureBaseEventArgs e)
        {
            if (e.Sender == this.owner)
            {
                if (this.Cache != null)
                {
                    e = this.Cache;

                    this.Cache = null;
                }

                base.Disable(e);
                return;
            }

            this.Cache = e;

            this.Menu.Item(this.owner.Path + "." + this.BoolName).SetValue(false);
        }

        /// <summary>
        ///     Raises the <see cref="E:OnEnableEvent" /> event.
        /// </summary>
        /// <param name="e">The <see cref="Base.FeatureBaseEventArgs" /> instance containing the event data.</param>
        public override void Enable(Base.FeatureBaseEventArgs e)
        {
            if (e.Sender == this.owner)
            {
                if (this.Cache != null)
                {
                    e = this.Cache;

                    this.Cache = null;
                }

                base.Enable(e);
                return;
            }

            this.Cache = e;

            this.Menu.Item(this.owner.Path + "." + this.BoolName).SetValue(true);
        }

        /// <summary>
        ///     Setups this instance.
        /// </summary>
        public override void Setup()
        {
            var menuItem =
                this.Menu.AddItem(
                    new MenuItem(this.owner.Path + "." + this.BoolName, this.BoolName).SetValue(this.BoolValue));

            menuItem.ValueChanged += delegate(object sender, OnValueChangeEventArgs args)
                {
                    if (args.GetNewValue<bool>()) this.Enable(new Base.FeatureBaseEventArgs(this.owner));
                    else if (!args.GetNewValue<bool>()) this.Disable(new Base.FeatureBaseEventArgs(this.owner));
                };

            if (this.Menu.Item(this.owner.Path + "." + this.BoolName).GetValue<bool>()) this.Enable(new Base.FeatureBaseEventArgs(this.owner));
            else this.Disable(new Base.FeatureBaseEventArgs(this.owner));
        }

        #endregion
    }
}