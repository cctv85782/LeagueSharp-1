namespace Rethought_Irelia.IreliaV1.Drawings
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Design;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;

    #endregion

    public sealed class DamageDrawingParent : ParentBase
    {
        #region Constants

        /// <summary>
        ///     The hero bar height
        /// </summary>
        private const int HeroBarHeight = 9;

        /// <summary>
        ///     The hero bar width
        /// </summary>
        private const int HeroBarWidth = 103;

        /// <summary>
        ///     The hero x offset
        /// </summary>
        private const int HeroXOffset = 10;

        /// <summary>
        ///     The hero y offset
        /// </summary>
        private const int HeroYOffset = 20;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageDrawingParent" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public DamageDrawingParent(string name)
        {
            this.Name = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     The damage calculator
        /// </summary>
        public List<IDamageDrawing> DamageCalculators { get; set; } = new List<IDamageDrawing>();

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the specified damage calculator.
        /// </summary>
        /// <param name="damageCalculator">The damage calculator.</param>
        public void Add(IDamageDrawing damageCalculator)
        {
            this.DamageCalculators.Add(damageCalculator);

            var @base = damageCalculator as Base;

            if (@base == null) return;

            this.Add(@base);
        }

        /// <summary>
        ///     Adds the specified damage calculators.
        /// </summary>
        /// <param name="damageCalculators">The damage calculators.</param>
        public void Add(IEnumerable<IDamageDrawing> damageCalculators)
        {
            foreach (var damageCalculator in damageCalculators)
            {
                this.Add(damageCalculator);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="Base.FeatureBaseEventArgs" /> instance containing the event data.</param>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Drawing.OnEndScene -= this.DrawingOnEndScene;
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="Base.FeatureBaseEventArgs" /> instance containing the event data.</param>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Drawing.OnEndScene += this.DrawingOnEndScene;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="featureBaseEventArgs">The <see cref="Base.FeatureBaseEventArgs" /> instance containing the event data.</param>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.Menu.AddItem(
                new MenuItem(this.Path + ".ordermode", "Order Mode: ").SetValue(
                    new StringList(new[] { "Static Order", "Dynamic Ordering (Most Damage first)" }, 1)));
        }

        /// <summary>
        ///     Drawings the on end scene.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void DrawingOnEndScene(EventArgs args)
        {
            //    if (Drawing.Direct3DDevice == null || Drawing.Direct3DDevice.IsDisposed)
            //    {
            //        return;
            //    }

            //    var offset = new Offset<int>();

            //    // IDrawTarget - Must have offset and some value (health?)
            //    foreach (var hero in HeroManager.Enemies)
            //    {
            //        var list = this.DamageCalculators.ToDictionary(drawer => drawer, drawer => drawer.GetDamage(hero) / hero.Health * HeroBarWidth);

            //        switch (this.Menu.Item(this.Path + ".ordermode").GetValue<StringList>().SelectedIndex)
            //        {
            //            case 0:
            //                foreach (var item in list)
            //                {
            //                    if (item.Value)
            //                }
            //                break;
            //            case 1:
            //                break;
            //            default:
            //                throw new ArgumentOutOfRangeException();
            //        }

            //        offset.Left = 11;
            //        offset.Bottom = 24;

            //        var enemyHealthMultiplicative = hero.Health / hero.MaxHealth;

            //        var end = new Vector2(
            //            (int)hero.HPBarPosition.X + offset.Left + enemyHealthMultiplicative * HeroBarWidth,
            //            (int)hero.HPBarPosition.Y + offset.Bottom);

            //        foreach (var drawer in this.DamageCalculators)
            //        {
            //            var healthPercDamageApplied = drawer.GetDamage(hero) / hero.MaxHealth;

            //            var newSectionLength = (int)(HeroBarWidth * healthPercDamageApplied);

            //            var start = new Vector2(
            //                (int)hero.HPBarPosition.X + offset.Left + newSectionLength,
            //                (int)hero.HPBarPosition.Y + offset.Bottom);

            //            if ((int)start.Distance(end) != newSectionLength)
            //            {
            //                start = end.Extend(start, newSectionLength);
            //            }

            //            drawer.Draw(start, end, 20);

            //            end = start;
            //        }
            //    }


            #endregion
        }
    }
}