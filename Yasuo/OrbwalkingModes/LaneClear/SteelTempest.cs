namespace Yasuo.OrbwalkingModes.LaneClear
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;
    using LeagueSharp.SDK.Utils;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Provider;
    using Yasuo.Common.Utility;

    internal class SteelTempest : Child<LaneClear>
    {
        #region Fields

        /// <summary>
        ///     The E logicprovider
        /// </summary>
        public SweepingBladeLogicProvider ProviderE;

        /// <summary>
        ///     The Q logicprovider
        /// </summary>
        public SteelTempestLogicProvider ProviderQ;

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
            if (GlobalVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear
                || !GlobalVariables.Spells[SpellSlot.Q].IsReady())
            {
                return;
            }

            var minions = MinionManager.GetMinions(
                GlobalVariables.Player.ServerPosition,
                GlobalVariables.Spells[SpellSlot.Q].Range,
                MinionTypes.All,
                MinionTeam.Enemy,
                MinionOrderTypes.None);

            if (minions.Count == 0)
            {
                return;
            }

            #region EQ

            // EQ > Synergyses with the E function in SweepingBlade/LogicProvider.cs
            if (this.Menu.Item(this.Name + "EQ").GetValue<bool>()
                && (GlobalVariables.Player.IsDashing()
                    && minions.Where(x => x.Health <= this.ProviderQ.GetDamage(x))
                           .Count(x => x.Distance(GlobalVariables.Player) <= 375) > 2))
            {
                // Won't waste Q3
                // TODO: Add a Logic to do it if an enemy can get hit
                if (this.Menu.Item(this.Name + "EQNoQ3").GetValue<bool>() && this.ProviderQ.HasQ3())
                {
                    return;
                }
                this.Execute(minions);
            }

                #endregion

                #region Unstacked Q and Stacked Q

            else
            {
                if (GlobalVariables.Player.Spellbook.IsAutoAttacking || GlobalVariables.Player.Spellbook.IsCharging
                    || GlobalVariables.Player.Spellbook.IsChanneling)
                {
                    return;
                }

                // Mass lane clear logic
                if (this.ProviderQ.HasQ3())
                {
                    // if AOE is enabled and more than X units are around us.
                    if (this.Menu.Item(this.Name + "AOE").GetValue<bool>()
                        && this.Menu.Item(this.Name + "MinHitAOE").GetValue<Slider>().Value
                        <= minions.Where(x => !x.InAutoAttackRange()).ToList().Count)
                    {
                        // Check for the minions centered position and wait until we are a bit away
                        // TODO: Add values like Spread of the minions
                        if (this.Menu.Item(this.Name + "CenterCheck").GetValue<bool>()
                            && GlobalVariables.Player.Distance(Helper.GetMeanVector2(minions)) > 450
                            || minions.Where(x => !x.InAutoAttackRange()).ToList().Count > 15
                            || this.ProviderQ.BuffTime() <= 10)
                        {
                            minions = minions.Where(x => !x.InAutoAttackRange()).ToList();
                            this.Execute(minions, true);
                        }

                        // Alternative Logic if the Menu Item is disabled
                        else if (!this.Menu.Item(this.Name + "CenterCheck").GetValue<bool>())
                        {
                            minions = minions.Where(x => !x.InAutoAttackRange()).ToList();
                            this.Execute(minions, true);
                        }
                    }
                }

                // Stack Logic
                // TODO: Add Health Prediction
                else
                {
                    this.Execute(minions, tryStacking: true);
                }
            }

            #endregion
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
            this.ProviderQ = new SteelTempestLogicProvider();
            this.ProviderE = new SweepingBladeLogicProvider();

            base.OnInitialize();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.GeneralSettings();

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        /// <summary>
        ///     Executes the specified units.
        /// </summary>
        /// <param name="units">The units.</param>
        /// <param name="aoe">if set to <c>true</c> [aoe].</param>
        /// <param name="circular">if set to <c>true</c> [circular].</param>
        /// <param name="tryStacking">if set to <c>true</c> [try stacking].</param>
        private void Execute(List<Obj_AI_Base> units, bool aoe = false, bool circular = false, bool tryStacking = false)
        {
            if (aoe)
            {
                var pred = MinionManager.GetBestLineFarmLocation(
                    units.Select(m => m.ServerPosition.To2D()).ToList(),
                    GlobalVariables.Spells[SpellSlot.Q].Width,
                    GlobalVariables.Spells[SpellSlot.Q].Range);

                GlobalVariables.Spells[SpellSlot.Q].Cast(pred.Position);
            }
            if (circular)
            {
                GlobalVariables.Spells[SpellSlot.Q].Cast(
                    units.Where(x => x.Distance(GlobalVariables.Player) <= 375).MinOrDefault(x => x.Health));
            }
            if (tryStacking)
            {
                // Get the minion that is furthest away and killable
                var minion =
                    MinionManager.GetMinions(
                        GlobalVariables.Player.ServerPosition,
                        GlobalVariables.Spells[SpellSlot.Q].Range)
                        .Where(
                            x =>
                            x.Health <= this.ProviderQ.GetDamage(x)
                            && x.Distance(GlobalVariables.Player.ServerPosition)
                            <= GlobalVariables.Spells[SpellSlot.Q].Range)
                        .MaxOrDefault(x => x.Distance(GlobalVariables.Player.ServerPosition));

                if (minion != null)
                {
                    GlobalVariables.Spells[SpellSlot.Q].Cast(minion.ServerPosition);
                }
            }
        }

        /// <summary>
        ///     Method to set the general settings
        /// </summary>
        private void GeneralSettings()
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
                new MenuItem(this.Name + "EQNoQ3", "Only EQ if Q not charged").SetValue(true)
                    .SetTooltip("If this is enabled, the assembly won't do EQ if you have stacked/charged Q"));
        }

        #endregion
    }
}