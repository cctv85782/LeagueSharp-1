namespace Yasuo.OrbwalkingModes.LaneClear
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Extensions;
    using Yasuo.Common.LogicProvider;
    using Yasuo.Common.Utility;

    internal class SteelTempest : Child<LaneClear>
    {
        #region Fields

        /// <summary>
        ///     The minions
        /// </summary>
        protected List<Obj_AI_Base> Minions = new List<Obj_AI_Base>();

        /// <summary>
        ///     The E logicprovider
        /// </summary>
        private SweepingBladeLogicProvider providerE;

        /// <summary>
        ///     The Q logicprovider
        /// </summary>
        private SteelTempestLogicProvider providerQ;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SteelTempest" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public SteelTempest(LaneClear parent)
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
        public override string Name => "(Q) Steel Tempest";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnUpdate(EventArgs args)
        {
            this.SoftReset();

            if (GlobalVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear
                || !GlobalVariables.Spells[SpellSlot.Q].IsReady())
            {
                return;
            }

            this.GetMinions();

            this.LogicMassClear();

            this.LogicLastHit();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable()
        {
            Game.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            Game.OnUpdate += this.OnUpdate;
            base.OnEnable();
        }

        /// <summary>
        ///     Called when [initialize].
        /// </summary>
        protected override void OnInitialize()
        {
            this.providerQ = new SteelTempestLogicProvider();
            this.providerE = new SweepingBladeLogicProvider();

            base.OnInitialize();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);


            this.SetupGeneralMenu();


            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        /// <summary>
        ///     Executes on the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        private static void Execute(Vector3 position)
        {
            if (position.IsValid())
            {
                GlobalVariables.Spells[SpellSlot.Q].Cast(position);
            }
        }

        /// <summary>
        ///     Executes the LastHit logic.
        /// </summary>
        private void LogicLastHit()
        {
            if (GlobalVariables.Player.IsWindingUp || GlobalVariables.Player.Spellbook.IsCharging
                || GlobalVariables.Player.Spellbook.IsChanneling)
            {
                return;
            }
        }

        /// <summary>
        ///     Executes the mass clear.
        /// </summary>
        private void LogicMassClear()
        {
            if (GlobalVariables.Player.IsWindingUp || GlobalVariables.Player.Spellbook.IsCharging
                || GlobalVariables.Player.Spellbook.IsChanneling)
            {
                return;
            }

            var farmlocation = MinionManager.GetBestLineFarmLocation(
                Minions.ToVector3S().To2D(),
                GlobalVariables.Spells[SpellSlot.Q].Width,
                GlobalVariables.Spells[SpellSlot.Q].Range);

            if (farmlocation.MinionsHit == 0)
            {
                return;
            }

            if (this.Menu.Item(this.Name + "AOE").GetValue<bool>()
                && this.Menu.Item(this.Name + "MinHitAOE").GetValue<Slider>().Value <= farmlocation.MinionsHit)
            {
                if (this.Menu.Item(this.Name + "CenterCheck").GetValue<bool>()
                    && GlobalVariables.Player.Distance(Helper.GetMeanVector2(Minions)) > 450
                    || Minions.Where(x => !x.InAutoAttackRange()).ToList().Count <= farmlocation.MinionsHit
                    || this.providerQ.BuffTime() <= 100)
                {
                    Minions = Minions.Where(x => !x.InAutoAttackRange()).ToList();
                    Execute(farmlocation.Position.To3D());
                }
            }
        }

        /// <summary>
        ///     Method to set the general settings
        /// </summary>
        private void SetupGeneralMenu()
        {
            this.Menu.AddItem(
                new MenuItem(this.Name + "AOE", "Try to hit multiple").SetValue(true)
                    .SetTooltip(
                        "If predicted hit count > slider, it will try to hit multiple, else it will aim for a single minion"));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MinHitAOE", "Min HitCount for AOE").SetValue(new Slider(1, 1, 15)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "CenterCheck", "Check for the minions mean vector").SetValue(true)
                    .SetTooltip(
                        "if this is enabled, the assembly will try to not use stacked/charged Q inside many minions and will either wait until the buff runs out or until you are further away from the minions to hit more."));

            this.Menu.AddItem(
                new MenuItem(this.Name + "EQ", "Do EQ").SetValue(true)
                    .SetTooltip("If this is enabled, the assembly will try to hit minions while dashing"));

            this.Menu.AddItem(
                new MenuItem(this.Name + "EQ.OnlyNotStacked", "Only EQ if Q not charged").SetValue(true)
                    .SetTooltip("If this is enabled, the assembly won't do EQ if you have stacked/charged Q"));
        }

        /// <summary>
        ///     Gets the minions.
        /// </summary>
        private void GetMinions()
        {
            this.Minions = MinionManager.GetMinions(
                GlobalVariables.Player.ServerPosition,
                GlobalVariables.Spells[SpellSlot.Q].Range,
                MinionTypes.All,
                MinionTeam.NotAlly,
                MinionOrderTypes.None);
        }

        /// <summary>
        /// Resets some fields
        /// </summary>
        private void SoftReset()
        {
            this.Minions = new List<Obj_AI_Base>();
        }

        #endregion
    }
}