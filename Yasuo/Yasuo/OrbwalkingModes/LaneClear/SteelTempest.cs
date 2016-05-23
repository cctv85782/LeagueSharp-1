namespace Yasuo.Yasuo.OrbwalkingModes.LaneClear
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Yasuo.CommonEx;
    using global::Yasuo.CommonEx.Classes;
    using global::Yasuo.CommonEx.Extensions;
    using global::Yasuo.CommonEx.Menu;
    using global::Yasuo.CommonEx.Utility;
    using global::Yasuo.Yasuo.LogicProvider;
    using global::Yasuo.Yasuo.Menu.MenuSets.OrbwalkingModes.LaneClear;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    internal class SteelTempest : Child<LaneClear>
    {
        #region Fields

        /// <summary>
        ///     The minions
        /// </summary>
        protected List<Obj_AI_Base> Units = new List<Obj_AI_Base>();

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
                || !GlobalVariables.Spells[SpellSlot.Q].IsReady() || GlobalVariables.Player.IsWindingUp)
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
            Events.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            Events.OnUpdate += this.OnUpdate;
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
        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);


            var menuGenerator = new MenuGenerator(new SteelTempestMenu(this.Menu));

            menuGenerator.Generate();


            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        /// <summary>
        ///     Executes on the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        private static void Execute(Vector3 position)
        {
            if (!position.IsValid()) return;

            GlobalVariables.CastManager.Queque.Enqueue(4, () => GlobalVariables.Spells[SpellSlot.Q].Cast(position));
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

        // TODO: Player path based mode
        /// <summary>
        ///     Executes the mass clear.
        /// </summary>
        private void LogicMassClear()
        {
            if (GlobalVariables.Player.IsWindingUp || GlobalVariables.Player.Spellbook.IsCharging
                || GlobalVariables.Player.Spellbook.IsChanneling || !this.Units.Any())
            {
                return;
            }

            var farmlocation = MinionManager.GetBestLineFarmLocation(
                this.Units.ToVector3S().To2D(),
                GlobalVariables.Spells[SpellSlot.Q].Width,
                GlobalVariables.Spells[SpellSlot.Q].Range);

            if (this.Menu.Item(this.Name + "MinHitAOE").GetValue<Slider>().Value > farmlocation.MinionsHit)
            {
                return;
            }

            if (this.Menu.Item(this.Name + "CenterCheck").GetValue<bool>())
            {
                if (GlobalVariables.Player.Distance(Helper.GetMeanVector2(this.Units)) >= 450
                || this.Units.Where(x => !x.InAutoAttackRange()).ToList().Count <= farmlocation.MinionsHit
                || this.providerQ.BuffTime() <= 50)
                {
                    Execute(farmlocation.Position.To3D());
                }

            }
            else
            {
                Execute(farmlocation.Position.To3D());
            }
        }

        /// <summary>
        ///     Gets the minions.
        /// </summary>
        private void GetMinions()
        {
            this.Units = SebbyLib.Cache.GetMinions(
                GlobalVariables.Player.ServerPosition,
                GlobalVariables.Spells[SpellSlot.Q].Range,
                MinionTeam.NotAlly);
        }

        /// <summary>
        /// Resets some fields
        /// </summary>
        private void SoftReset()
        {
            this.Units = new List<Obj_AI_Base>();
        }

        #endregion
    }
}