﻿namespace Rethought_Kayle.KayleV1.DamageCalculator
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class DamageCalculatorParent : ParentBase, IDamageCalculator
    {
        #region Fields

        private List<IDamageCalculatorModule> damageCalculatorsModules = new List<IDamageCalculatorModule>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Damage Calculator";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the specified damage calculator module.
        /// </summary>
        /// <param name="damageCalculatorModule">The damage calculator module.</param>
        public void Add(IDamageCalculatorModule damageCalculatorModule)
        {
            this.damageCalculatorsModules.Add(damageCalculatorModule);
        }

        /// <summary>
        ///     Gets the damage.
        /// </summary>
        /// <param name="target">The get damage.</param>
        /// <returns></returns>
        public float GetDamage(Obj_AI_Base target)
        {
            var result =
                this.damageCalculatorsModules.Sum(
                    calculatorModule => calculatorModule.GetDamage(target) * calculatorModule.EstimatedAmountInOneCombo);

            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(new MenuItem("equalsonecombo", "One combo equals: "));

            foreach (var damageCalculator in this.damageCalculatorsModules)
            {
                var slider =
                    this.Menu.AddItem(
                        new MenuItem(this.Path + "." + damageCalculator.Name, damageCalculator.Name).SetValue(
                            new Slider(damageCalculator.EstimatedAmountInOneCombo, 0, 5)));

                slider.ValueChanged +=
                    (o, args) => { damageCalculator.EstimatedAmountInOneCombo = args.GetNewValue<Slider>().Value; };

                damageCalculator.EstimatedAmountInOneCombo = slider.GetValue<Slider>().Value;
            }
        }

        #endregion
    }
}