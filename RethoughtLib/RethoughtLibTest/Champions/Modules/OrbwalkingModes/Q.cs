namespace RethoughtLibTest.Champions.Modules.OrbwalkingModes
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Classes.Feature;
    using RethoughtLib.Menu;
    using RethoughtLib.Menu.Presets;
    using RethoughtLib.TargetValidator;

    using RethoughtLib.TargetValidator.Implementations;

    #endregion

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

        public void OnUpdate(EventArgs args)
        {
            var target = TargetSelector.GetTarget(500, TargetSelector.DamageType.Magical);

            var targetValidator = new TargetValidator();

            targetValidator.AddCheck(new IsValidTargetCheck());

            targetValidator.AddChecks(new []
                                          {
                                              new IsValidTargetCheck(), new IsValidTargetCheck(), new IsValidTargetCheck(), 
                                          });

            // now it checks for IsValidTarget and a SheenBuff

            var valid = targetValidator.Check(target);
        }

        #endregion

        #region Methods

        protected override void OnDisable()
        {
            Game.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            Game.OnUpdate += this.OnUpdate;
            base.OnEnable();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected sealed override void OnLoad()
        {
            Console.WriteLine("Test" + "Nunu");
            this.Menu = new Menu(this.Name, this.Name);
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));



            var selecter = new MenuItem("Selecter", "Selecter").SetValue(new StringList(new []{ "case1", "case2", "case3"}, 1));

            var listListMenuItems = new List<List<MenuItem>>()
                                        {
                                            new List<MenuItem>()
                                                {
                                                    new MenuItem("Number1A","MenuItem 1").SetValue(true),
                                                    new MenuItem("Number2A","MenuItem 2").SetValue(true),
                                                    new MenuItem("Number3A","MenuItem 3").SetValue(true),
                                                },
                                            new List<MenuItem>()
                                                {
                                                    new MenuItem("Number1Bds","MenuItem 1").SetValue(false),
                                                    new MenuItem("Number2Bfd","MenuItem 2").SetValue(true),
                                                    new MenuItem("Number3Bfr","MenuItem 3").SetValue(false),
                                                },
                                            new List<MenuItem>()
                                                {
                                                    new MenuItem("Number1C","MenuItem 1").SetValue(new Circle()),
                                                    new MenuItem("Number2C","MenuItem 2").SetValue(false),
                                                    new MenuItem("Number3C","MenuItem 3").SetValue(new StringList(new string[] {"string1", "string2"})),
                                                }
                                        };

            var menuGenerator = new MenuGenerator(this.Menu, new DynamicMenu("This is my dynamic Menu", selecter, listListMenuItems));

            menuGenerator.Generate();

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        #endregion
    }
}