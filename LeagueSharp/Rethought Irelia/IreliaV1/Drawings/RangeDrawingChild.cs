﻿namespace Rethought_Irelia.IreliaV1.Drawings
{
    #region Using Directives

    using System;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal sealed class RangeDrawingChild : ChildBase
    {
        #region Fields

        /// <summary>
        ///     The spell
        /// </summary>
        private readonly Spell spell;

        /// <summary>
        ///     Whether the spell must be ready
        /// </summary>
        private readonly bool spellMustBeReady;

        /// <summary>
        ///     The color
        /// </summary>
        private Color color = Color.White;

        /// <summary>
        ///     Whether the z-axis is considered
        /// </summary>
        private bool zAxis;

        #endregion

        #region Constructors and Destructors

        public RangeDrawingChild(Spell spell, string name, bool spellMustBeReady = false)
        {
            this.spell = spell;

            this.Name = name;

            this.spellMustBeReady = spellMustBeReady;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);

            Drawing.OnDraw -= this.DrawingOnOnDraw;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);

            Drawing.OnDraw += this.DrawingOnOnDraw;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            var colorPicker =
                this.Menu.AddItem(
                    new MenuItem(this.Path + "." + "color", "Color").SetValue(new Circle(true, Color.White)));

            colorPicker.ValueChanged += (o, args) => { this.color = args.GetNewValue<Circle>().Color; };

            this.color = colorPicker.GetValue<Circle>().Color;

            this.Menu.AddItem(
                new MenuItem(this.Path + "." + "spellready", "Spell must be ready").SetValue(this.spellMustBeReady));

            var zaxisItem = this.Menu.AddItem(new MenuItem(this.Path + "." + "zaxis", "z-axis").SetValue(true));

            zaxisItem.ValueChanged += (o, args) => { this.zAxis = args.GetNewValue<bool>(); };

            this.zAxis = zaxisItem.GetValue<bool>();

            this.Menu.AddItem(
                new MenuItem(this.Path + "." + "displaymethod", "Display Method").SetValue(
                    new StringList(new[] { "Render Drawings", "League Drawings" })));
        }

        /// <summary>
        ///     Triggers OnDraw
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void DrawingOnOnDraw(EventArgs args)
        {
            if (this.Menu.Item(this.Path + "." + "spellready").GetValue<bool>() && !this.spell.IsReady()) return;

            if (this.Menu.Item(this.Path + "." + "displaymethod").GetValue<StringList>().SelectedIndex == 0)
            {
                Render.Circle.DrawCircle(ObjectManager.Player.Position, this.spell.Range, this.color, 3, this.zAxis);
            }
            else
            {
                Drawing.DrawCircle(ObjectManager.Player.Position, this.spell.Range, this.color);
            }
        }

        #endregion
    }
}