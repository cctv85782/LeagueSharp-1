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

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

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

            if (GlobalVariables.Player.IsDashing())
            {
                GlobalVariables.Spells[SpellSlot.Q].SetSkillshot(GlobalVariables.providerQ.GetQDelay, 350, float.MaxValue, false, SkillshotType.SkillshotCircle);
                GlobalVariables.Spells[SpellSlot.Q].Range = 0;
                GlobalVariables.Spells[SpellSlot.Q].MinHitChance = HitChance.High;
            }
            else
            {
                if (GlobalVariables.Player.HasQ3())
                {
                    GlobalVariables.Spells[SpellSlot.Q].SetSkillshot(GlobalVariables.providerQ.GetQDelay, 90, 1200, false, SkillshotType.SkillshotLine);
                    GlobalVariables.Spells[SpellSlot.Q].Range = 950;
                    GlobalVariables.Spells[SpellSlot.Q].MinHitChance = HitChance.VeryHigh;
                }
                else
                {
                    GlobalVariables.Spells[SpellSlot.Q].SetSkillshot(GlobalVariables.providerQ.GetQDelay, 20, float.MaxValue, false, SkillshotType.SkillshotLine);
                    GlobalVariables.Spells[SpellSlot.Q].Range = 475;
                    GlobalVariables.Spells[SpellSlot.Q].MinHitChance = HitChance.VeryHigh;
                }
            }

            GlobalVariables.Spells[SpellSlot.W].SetSkillshot(0, 250 + (GlobalVariables.Spells[SpellSlot.W].Level * 50), 400, false, SkillshotType.SkillshotCone);
            GlobalVariables.Spells[SpellSlot.W].Range = 400;

            GlobalVariables.Spells[SpellSlot.E].SetTargetted(0, 1025);
            GlobalVariables.Spells[SpellSlot.E].Speed = 1000;
            GlobalVariables.Spells[SpellSlot.E].Range = 475;

            GlobalVariables.Spells[SpellSlot.R].SetTargetted(0, float.MaxValue);
            GlobalVariables.Spells[SpellSlot.R].Range = 900;
        }


        #endregion
    }
}