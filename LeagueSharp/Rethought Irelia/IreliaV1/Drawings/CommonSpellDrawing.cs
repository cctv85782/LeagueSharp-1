﻿namespace Rethought_Irelia.IreliaV1.Drawings
{
    #region Using Directives

    using System;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal sealed class CommonSpellDrawing : ChildBase
    {
        #region Fields

        private readonly Spell spell;

        private readonly bool spellMustBeReady;

        private Color color = Color.White;

        #endregion

        #region Constructors and Destructors

        public CommonSpellDrawing(Spell spell, string name, bool spellMustBeReady = false)
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

            var colorPicker = this.Menu.AddItem(new MenuItem(this.Name + "color", "Color").SetValue(new Circle(true, Color.White)));

            colorPicker.ValueChanged += (o, args) => { this.color = args.GetNewValue<Circle>().Color; };

            this.color = colorPicker.GetValue<Circle>().Color;

            this.Menu.AddItem(new MenuItem(this.Name + "spellready", "Spell must be ready").SetValue(this.spellMustBeReady));

            this.Menu.AddItem(
                new MenuItem(this.Name + "displaymethod", "Display Method").SetValue(
                    new StringList(new string[] { "Render Drawings", "League Drawings" })));
        }

        /// <summary>
        ///     Triggers OnDraw
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void DrawingOnOnDraw(EventArgs args)
        {
            if (this.Menu.Item(this.Name + "spellready").GetValue<bool>() && !this.spell.IsReady()) return;

            if (this.Menu.Item(this.Name + "displaymethod").GetValue<StringList>().SelectedIndex == 0)
            {
                Drawing.DrawCircle(ObjectManager.Player.Position, this.spell.Range, this.color);
            }
            else
            {
                Render.Circle.DrawCircle(ObjectManager.Player.ServerPosition, this.spell.Range, this.color, 3, true);
            }


        }

        #endregion
    }
}