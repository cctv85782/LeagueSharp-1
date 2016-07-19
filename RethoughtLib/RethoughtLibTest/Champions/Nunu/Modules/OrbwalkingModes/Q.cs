namespace RethoughtLibTest.Champions.Nunu.Modules.OrbwalkingModes
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.Classes.FeatureV2;
    using RethoughtLib.Menu;
    using RethoughtLib.Menu.Presets;
    using RethoughtLib.TargetValidator;
    using RethoughtLib.TargetValidator.Implementations;

    #endregion

    internal class Q : Child
    {
        #region Constructors and Destructors


        #endregion

        #region Public Methods and Operators

        public void OnUpdate(EventArgs args)
        {
            var target = TargetSelector.GetTarget(500, TargetSelector.DamageType.Magical);

            var targetValidator = new TargetValidator();

            var check = new IsValidTargetCheck();

            targetValidator.AddCheck(check);
            targetValidator.RemoveCheck(check);

            var valid = targetValidator.Check(target);
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Q";

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
        }

        #endregion
    }
}