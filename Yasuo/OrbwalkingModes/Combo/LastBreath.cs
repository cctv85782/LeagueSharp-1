namespace Yasuo.OrbwalkingModes.Combo
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;
    using Yasuo.Common.Classes;
    using Yasuo.Common.Extensions;
    using Yasuo.Common.Extensions.MenuExtensions;
    using Yasuo.Common.LogicProvider;

    #endregion

    internal class LastBreath : Child<Combo>
    {
        #region Fields

        /// <summary>
        ///     The blacklist
        /// </summary>
        public BlacklistMenu BlacklistMenu;

        /// <summary>
        ///     The possible executions
        /// </summary>
        private List<Common.Objects.LastBreath> executions = new List<Common.Objects.LastBreath>();

        /// <summary>
        ///     The R logicprovider
        /// </summary>
        private LastBreathLogicProvider provider;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LastBreath" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public LastBreath(Combo parent)
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
        public override string Name => "(R) Last Breath";

        #endregion

        #region Properties

        /// <summary>
        ///     The valid execution
        /// </summary>
        private Common.Objects.LastBreath ValidExecution { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnUpdate(EventArgs args)
        {
            this.SoftReset();

            if (GlobalVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || !GlobalVariables.Spells[SpellSlot.R].IsReady())
            {
                return;
            }

            this.BuildExecutions();

            this.ValidateExecutions();

            this.DecideAndExecute();
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
            this.provider = new LastBreathLogicProvider();

            base.OnInitialize();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.BlacklistMenu = new BlacklistMenu(this.Menu, "Blacklist");

            this.SetupAdvancedMenu();

            this.SetupGeneralMenu();

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        private void BuildExecutions()
        {
            foreach (var hero in HeroManager.Enemies.Where(x => x.IsAirbone()))
            {
                var execution = new Common.Objects.LastBreath(hero);

                if (!this.executions.Contains(execution))
                {
                    this.executions.Add(execution);
                }
            }
        }

        /// <summary>
        ///     Decides for an execution and executes.
        /// </summary>
        private void DecideAndExecute()
        {
            var advanced = Menu.SubMenu(this.Name + "Advanced");

            if (!this.executions.Any())
            {
                return;
            }

            // TODO: ADD safetyvalue/dangervalue auto mode
            switch (advanced.Item(advanced.Name + "EvaluationLogic").GetValue<StringList>().SelectedIndex)
            {
                // Damage
                case 0:
                    ValidExecution = this.executions.MaxOrDefault(x => x.DamageDealt);
                    break;
                // Count
                case 1:
                    ValidExecution = this.executions.MaxOrDefault(x => x.AffectedEnemies.Count);
                    break;
                // Priority
                case 2:
                    ValidExecution = this.executions.MaxOrDefault(x => x.Priority);
                    break;
                // Auto
                case 3:
                    break;
            }

            if (ValidExecution != null)
            {
                Execute(ValidExecution);
            }
        }

        /// <summary>
        ///     Executes the specified execution.
        /// </summary>
        /// <param name="execution">The execution.</param>
        private void Execute(Common.Objects.LastBreath execution)
        {
            if (this.provider.ShouldCastNow(execution, SweepingBlade.PathCopy))
            {
                GlobalVariables.Spells[SpellSlot.R].CastOnUnit(execution.Target);
            }
        }

        // TODO: Dynamic Menu
        /// <summary>
        ///     Method to set advanced settings.
        /// </summary>
        private void SetupAdvancedMenu()
        {
            var advanced = new Menu("Advanced", this.Name + "Advanced");

            advanced.AddItem(
                new MenuItem(advanced.Name + "EvaluationLogic", "Evaluation Logic").SetValue(
                    new StringList(new[] { "Damage", "Count", "Priority", "Auto" })));

            advanced.AddItem(
                new MenuItem(advanced.Name + "MaxHealthPercDifference", "Max Health (%) Difference").SetValue(
                    new Slider(40)));

            advanced.AddItem(
                new MenuItem(advanced.Name + "OverkillCheck", "Overkill Check").SetValue(true)
                    .SetTooltip(
                        "If Combo is enough to finish the target it won't execute. Only works on single targets."));

            advanced.AddItem(
                new MenuItem(advanced.Name + "Disclaimer", "[i] Disclaimer").SetTooltip(
                    "Changing Values here might destroy the assembly logic, only change values if you know what you are doing!"));

            this.Menu.AddSubMenu(advanced);
        }

        /// <summary>
        ///     Method to set general settings.
        /// </summary>
        private void SetupGeneralMenu()
        {
            this.Menu.AddItem(
                new MenuItem(this.Name + "MinHitAOE", "Min HitCount for AOE").SetValue(new Slider(2, 1, 5)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MinPlayerHealth", "Min Player Health (%)").SetValue(new Slider(10)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MaxTargetsMeanHealth", "Max Target(s) Health (%)").SetValue(new Slider(80)));
        }

        /// <summary>
        ///     Resets the properties
        /// </summary>
        private void SoftReset()
        {
            if (!this.executions.Any() || this.ValidExecution == null
                || !this.ValidExecution.IsValid())
            {
                return;
            }

            this.executions = new List<Common.Objects.LastBreath>();
            this.ValidExecution = new Common.Objects.LastBreath();
        }

        /// <summary>
        ///     Validates the executions.
        /// </summary>
        private void ValidateExecutions()
        {
            var advanced = Menu.SubMenu(this.Name + "Advanced");

            if (!this.executions.Any())
            {
                return;
            }

            foreach (var execution in this.executions.ToList())
            {
                // Invalid
                if (!execution.IsValid())
                {
                    this.executions.Remove(execution);
                }

                // Count
                if (execution.AffectedEnemies.Count < this.Menu.Item(this.Name + "MinHitAOE").GetValue<Slider>().Value)
                {
                    this.executions.Remove(execution);
                }

                // Mean Health
                if ((execution.AffectedEnemies.Sum(x => x.HealthPercent) / execution.AffectedEnemies.Count)
                    > this.Menu.Item(this.Name + "MaxTargetsMeanHealth").GetValue<Slider>().Value)
                {
                    this.executions.Remove(execution);
                }

                // Max Health Percentage Difference
                if ((execution.AffectedEnemies.Sum(x => x.HealthPercent) / execution.AffectedEnemies.Count)
                    - GlobalVariables.Player.HealthPercent
                    > advanced.Item(advanced.Name + "MaxHealthPercDifference").GetValue<Slider>().Value)
                {
                    this.executions.Remove(execution);
                }

                // Overkill
                if (advanced.Item(advanced.Name + "OverkillCheck").GetValue<bool>() && execution.IsOverkill)
                {
                    this.executions.Remove(execution);
                }
            }
        }

        #endregion
    }
}