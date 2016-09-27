namespace Rethought_Irelia.IreliaV1.Drawings
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Extensions;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;
    using SharpDX.Direct3D9;

    #endregion

    public sealed class DamageDrawingChild : ChildBase, IDamageDrawing
    {
        #region Fields

        private readonly GetDamageDelegate getDamageDelegate;

        /// <summary>
        ///     The spell
        /// </summary>
        private readonly Spell spell;

        /// <summary>
        ///     The line
        /// </summary>
        private Line line;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DamageDrawingChild" /> class.
        /// </summary>
        /// <param name="spell">The spell.</param>
        /// <param name="name">The name.</param>
        /// <param name="getDamageDelegate">Custom get damage delegate</param>
        public DamageDrawingChild(Spell spell, string name, GetDamageDelegate getDamageDelegate = null)
        {
            this.spell = spell;

            this.getDamageDelegate = getDamageDelegate;

            this.Name = name;
        }

        #endregion

        #region Delegates

        /// <summary>
        ///     Gets the damage.
        /// </summary>
        /// <param name="target">The get damage.</param>
        /// <returns></returns>
        public delegate float GetDamageDelegate(Obj_AI_Base target);

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the color.
        /// </summary>
        /// <value>
        ///     The color.
        /// </value>
        public Color Color { get; set; }

        /// <summary>
        ///     Gets or sets the estimated amount in one combo.
        /// </summary>
        /// <value>
        ///     The estimated amount in one combo.
        /// </value>
        public int EstimatedAmountInOneCombo { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        public float Draw(Vector2 start, Vector2 end, float width)
        {
            this.line = new Line(Drawing.Direct3DDevice) { Width = width };

            this.line.Begin();
            this.line.Draw(new[] { start, end }, this.Color);
            this.line.End();

            return 0f;
        }

        /// <summary>
        ///     Gets the damage.
        /// </summary>
        /// <param name="target">The get damage.</param>
        /// <returns></returns>
        public float GetDamage(Obj_AI_Base target)
        {
            if (!this.Switch.Enabled) return 0f;

            return this.getDamageDelegate?.Invoke(target) ?? this.spell.GetDamage(target);
        }

        #endregion

        #region Methods

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            var colorPicker =
                this.Menu.AddItem(
                    new MenuItem(this.Path + "." + "color", "Color").SetValue(
                        new Circle(true, System.Drawing.Color.White)));

            colorPicker.ValueChanged += (o, args) => { this.Color = args.GetNewValue<Circle>().Color.ToSharpDxColor(); };

            this.Color = colorPicker.GetValue<Circle>().Color.ToSharpDxColor();
        }

        #endregion
    }
}