namespace Yasuo.OrbwalkingModes.Combo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Extensions;
    using Yasuo.Common.Extensions.MenuExtensions;
    using Yasuo.Common.Provider;

    internal class LastBreath : Child<Combo>
    {
        #region Fields

        /// <summary>
        ///     The blacklist
        /// </summary>
        public Blacklist Blacklist;

        /// <summary>
        ///     The R logicprovider
        /// </summary>
        public LastBreathLogicProvider Provider;

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

        #region Public Methods and Operators

        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnUpdate(EventArgs args)
        {
            try
            {
                if (GlobalVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                    || !GlobalVariables.Spells[SpellSlot.R].IsReady())
                {
                    return;
                }

                var advanced = Menu.SubMenu(this.Name + "Advanced");

                var executions = new List<Common.Objects.LastBreath>();

                foreach (var hero in HeroManager.Enemies.Where(x => x.IsAirbone()))
                {
                    var execution = new Common.Objects.LastBreath(hero);

                    if (!executions.Contains(execution))
                    {
                        executions.Add(execution);
                    }
                }

                #region processing/validating

                if (executions.Any())
                {
                    foreach (var execution in executions.ToList())
                    {
                        // Count
                        if (execution.AffectedEnemies.Count
                            < Menu.Item(this.Name + "MinHitAOE").GetValue<Slider>().Value)
                        {
                            executions.Remove(execution);
                        }

                        // Mean Health
                        if ((execution.AffectedEnemies.Sum(x => x.HealthPercent) / execution.AffectedEnemies.Count)
                            > Menu.Item(this.Name + "MaxTargetsMeanHealth").GetValue<Slider>().Value)
                        {
                            executions.Remove(execution);
                        }

                        // Max Health Percentage Difference
                        if ((execution.AffectedEnemies.Sum(x => x.HealthPercent) / execution.AffectedEnemies.Count)
                            - GlobalVariables.Player.HealthPercent
                            > advanced.Item(advanced.Name + "MaxHealthPercDifference").GetValue<Slider>().Value)
                        {
                            executions.Remove(execution);
                        }

                        if (advanced.Item(advanced.Name + "OverkillCheck").GetValue<bool>() && execution.IsOverkill)
                        {
                            executions.Remove(execution);
                        }
                    }
                }

                #endregion

                Common.Objects.LastBreath possibleExecution = null;

                // TODO: ADD safetyvalue/dangervalue
                if (executions.Any())
                {
                    switch (advanced.Item(advanced.Name + "EvaluationLogic").GetValue<StringList>().SelectedIndex)
                    {
                        // Damage
                        case 0:
                            possibleExecution = EnumerableExtensions.MaxOrDefault(executions, x => x.DamageDealt);
                            break;
                        // Count
                        case 1:
                            possibleExecution = EnumerableExtensions.MaxOrDefault(
                                executions,
                                x => x.AffectedEnemies.Count);
                            break;
                        // Priority
                        case 2:
                            possibleExecution = EnumerableExtensions.MaxOrDefault(executions, x => x.Priority);
                            break;
                        // Auto
                        case 3:
                            break;
                    }
                }

                if (possibleExecution != null)
                {
                    Execute(possibleExecution);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
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
            this.Provider = new LastBreathLogicProvider();

            base.OnInitialize();
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            Blacklist = new Blacklist(this.Menu, "Blacklist");

            this.AdvancedMenu();
            this.GeneralSettings();

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        /// <summary>
        ///     Method to set advanced settings.
        /// </summary>
        private void AdvancedMenu()
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
        ///     Executes the specified execution.
        /// </summary>
        /// <param name="execution">The execution.</param>
        private void Execute(Common.Objects.LastBreath execution)
        {
            if (Provider.ShouldCastNow(execution, SweepingBlade.PathCopy))
            {
                GlobalVariables.Spells[SpellSlot.R].CastOnUnit(execution.Target);
            }
        }

        /// <summary>
        ///     Method to set general settings.
        /// </summary>
        private void GeneralSettings()
        {
            this.Menu.AddItem(
                new MenuItem(this.Name + "MinHitAOE", "Min HitCount for AOE").SetValue(new Slider(2, 1, 5)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MinPlayerHealth", "Min Player Health (%)").SetValue(new Slider(10)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "MaxTargetsMeanHealth", "Max Target(s) Health (%)").SetValue(new Slider(80)));
        }

        #endregion
    }
}