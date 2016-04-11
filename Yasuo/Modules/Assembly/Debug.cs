namespace Yasuo.Modules.Assembly
{
    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Utility;

    internal class Debug : Child<Assembly>
    {
        #region Constructors and Destructors

        public Debug(Assembly parent)
            : base(parent)
        {
            this.OnLoad();
        }

        #endregion

        #region Public Properties

        public override string Name => "Debug";

        #endregion

        #region Methods

        protected override void OnDisable()
        {
            Variables.Debug = false;
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            Variables.Debug = true;
            base.OnEnable();
        }

        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(false));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        #endregion
    }
}