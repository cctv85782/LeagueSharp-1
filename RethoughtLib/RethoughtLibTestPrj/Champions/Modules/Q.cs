namespace RethoughtLibTestPrj.Champions.Modules
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Classes.Feature;

    internal class Q : FeatureChild<Combo>
    {
        #region Constructors and Destructors

        public Q(Combo parent)
            : base(parent)
        {
            this.OnLoad();
        }

        #endregion

        #region Public Properties

        public override string Name => "Q";

        #endregion

        #region Public Methods and Operators

        public void OnDraw(EventArgs args)
        {
        }

        #endregion

        #region Methods

        protected override void OnDisable()
        {
            Drawing.OnDraw -= this.OnDraw;
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            Drawing.OnDraw += this.OnDraw;
            base.OnEnable();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        #endregion
    }
}