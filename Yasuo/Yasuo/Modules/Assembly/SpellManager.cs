namespace Yasuo.Yasuo.Modules.Assembly
{
    #region Using Directives

    using System;

    using global::Yasuo.CommonEx;
    using global::Yasuo.CommonEx.Classes;
    using global::Yasuo.CommonEx.Extensions;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal class SpellManager : Child<Assembly>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CastManager" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public SpellManager(Assembly parent)
            : base(parent)
        {
            this.OnLoad();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name => "Spell Manager";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable()
        {
            Events.OnPreUpdate -= OnPreUpdate;
            GlobalVariables.Debug = false;
            base.OnDisable();
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            Events.OnPreUpdate += OnPreUpdate;
            base.OnEnable();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            // TODO: Add slider to adjust spells in lenght, speed etc.

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(false));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }


        /// <summary>
        /// Raises the <see cref="E:PreUpdate" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void OnPreUpdate(EventArgs args)
        {
            SetSpells();
        }

        /// <summary>
        ///     Method that adjusts the spells to the current circumstances
        /// </summary>
        public static void SetSpells()
        {
            if (GlobalVariables.Spells == null)
            {
                return;
            }

            var q = GlobalVariables.Spells[SpellSlot.Q];
            var w = GlobalVariables.Spells[SpellSlot.W];
            var e = GlobalVariables.Spells[SpellSlot.E];
            var r = GlobalVariables.Spells[SpellSlot.R];

            if (GlobalVariables.Player.IsDashing())
            {
                q.SetSkillshot(GlobalVariables.providerQ.GetQDelay, 350, float.MaxValue, false, SkillshotType.SkillshotCircle);
                q.Range = 0;
                q.MinHitChance = HitChance.High;
            }
            else
            {
                if (GlobalVariables.Player.HasQ3())
                {
                    q.SetSkillshot(GlobalVariables.providerQ.GetQDelay, 90, 1200, false, SkillshotType.SkillshotLine);
                    q.Range = 950;
                    q.MinHitChance = HitChance.VeryHigh;
                }
                else
                {
                    q.SetSkillshot(GlobalVariables.providerQ.GetQDelay, 20, float.MaxValue, false, SkillshotType.SkillshotLine);
                    q.Range = 475;
                    q.MinHitChance = HitChance.VeryHigh;
                }
            }

            w.SetSkillshot(0, 250 + (w.Level * 50), 400, false, SkillshotType.SkillshotCone);
            w.Range = 400;

            e.SetTargetted(0, 1025);
            e.Speed = 1000;
            e.Range = 475;

            r.SetTargetted(0, float.MaxValue);
            r.Range = 900;

            GlobalVariables.Spells[SpellSlot.Q] = q;
            GlobalVariables.Spells[SpellSlot.W] = w;
            GlobalVariables.Spells[SpellSlot.E] = e;
            GlobalVariables.Spells[SpellSlot.R] = r;
        }


        #endregion
    }
}