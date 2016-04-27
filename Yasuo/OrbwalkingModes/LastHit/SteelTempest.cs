namespace Yasuo.OrbwalkingModes.LastHit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Provider;
    using Yasuo.Common.Utility;
    using Yasuo.Common.Extensions;

    using Dash = Yasuo.Common.Objects.Dash;

    internal class SteelTempest : Child<LastHit>
    {
        #region Fields

        public SweepingBladeLogicProvider ProviderE;

        public SteelTempestLogicProvider ProviderQ;

        public TurretLogicProvider ProviderTurret;

        #endregion

        #region Constructors and Destructors

        public SteelTempest(LastHit parent)
            : base(parent)
        {
            this.OnLoad();
        }

        #endregion

        #region Public Properties

        public override string Name => "(Q) Steel Tempest";

        #endregion

        #region Public Methods and Operators

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
            this.ProviderQ = new SteelTempestLogicProvider();
            this.ProviderE = new SweepingBladeLogicProvider();
            this.ProviderTurret = new TurretLogicProvider();

            base.OnInitialize();
        }

        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            #region EQ

            var settingsEq = new Menu("While dashing (EQ)", this.Name + "WhileDashingMenu");

            settingsEq.AddItem(new MenuItem(settingsEq.Name + "Enabled", "Enabled").SetValue(true));

            settingsEq.AddItem(new MenuItem(settingsEq.Name + "MinHit", "Min Hit Count").SetValue(new Slider(1, 1, 15)));

            settingsEq.AddItem(
                new MenuItem(settingsEq.Name + "OnlyKillable", "Only EQ if all minions around you will die").SetValue(true)
                    .SetTooltip(
                        "If this is enabled, the assembly won't kill minions if there are minions around that won't die. Enabling that will help you not pushing your lane while Lasthitting."));

            settingsEq.AddItem(
                new MenuItem(settingsEq.Name + "OnlyNotStacked", "Only Cast if Q not charged").SetValue(true)
                    .SetTooltip("If this is enabled, the assembly won't do EQ if you have stacked/charged Q"));

            this.Menu.AddSubMenu(settingsEq);

            #endregion

            #region advanced

            var settingsAdv = new Menu("Advanced", this.Name + "Advanced");

            settingsAdv.AddItem(new MenuItem(settingsAdv.Name + "MeanVectorCheck", "Check for unit mean vector").SetValue(false)
                .SetTooltip(
                        "if this is enabled, the assembly will try to not use stacked/charged Q inside many minions and will either wait until the buff runs out or until you are further away from the minions to hit more."));

            settingsAdv.AddItem(
                new MenuItem(settingsAdv.Name + "MeanVectorMaxDist", "Max Distance from Mean Vector").SetValue(new Slider(600, 0, 2000)));

            settingsAdv.AddItem(new MenuItem(settingsAdv.Name + "TurretCheck", "Check for enemy turret").SetValue(true)
                .SetTooltip(
                        "if this is enabled, the assembly will try to not use stacked/charged Q inside the enemy turret. But it will use them if the turret is focusing something else!"));

            this.Menu.AddSubMenu(settingsAdv);

            #endregion

            this.Menu.AddItem(new MenuItem(this.Name + "NoQ3Count", "Don't use Q3 if X or more than X enemies around").SetValue(new Slider(2, 0, 5)));

            this.Menu.AddItem(new MenuItem(this.Name + "NoQ3Range", "Check for enemies in range of").SetValue(new Slider(1000, 0, 5000)));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        public void OnUpdate(EventArgs args)
        {
            if (GlobalVariables.Orbwalker.ActiveMode != LeagueSharp.Common.Orbwalking.OrbwalkingMode.LastHit || !GlobalVariables.Spells[SpellSlot.Q].IsReady()
                || GlobalVariables.Player.Spellbook.IsCharging || GlobalVariables.Player.Spellbook.IsChanneling)
            {
                return;
            }

            var units = MinionManager.GetMinions(
                GlobalVariables.Player.ServerPosition,
                GlobalVariables.Spells[SpellSlot.Q].Range,
                MinionTypes.All,
                MinionTeam.NotAlly,
                MinionOrderTypes.None);

            if (ProviderQ.HasQ3() &&
                GlobalVariables.Player.CountEnemiesInRange(this.Menu.Item(this.Name + "NoQ3Range").GetValue<Slider>().Value)
                >= this.Menu.Item(this.Name + "NoQ3Count").GetValue<Slider>().Value)
            {
                return;
            }

            var settingsAdv = this.Menu.SubMenu(this.Name + "Advanced");

            if (units.Any())
            {
                if (settingsAdv.Item(settingsAdv.Name + "TurretCheck").GetValue<bool>())
                {
                    if (!ProviderTurret.IsSafePosition(GlobalVariables.Player.ServerPosition))
                    {
                        return;
                    }
                }

                if (settingsAdv.Item(settingsAdv.Name + "MeanVectorCheck").GetValue<bool>()
                    && !GlobalVariables.Player.IsDashing())
                {
                    var meanVector = Helper.GetMeanVector2(units);

                    if (meanVector.Distance(GlobalVariables.Player.ServerPosition) <
                        settingsAdv.Item(settingsAdv.Name + "MeanVectorMaxDist").GetValue<Slider>().Value)
                    {
                        return;
                    }
                }

                this.Eq(units);
                this.Q3(units);
                this.Q1Q2(units);
            }
        }

        /// <summary>
        ///     LastHits with Q while dashing
        /// </summary>
        /// <param name="units"></param>
        private void Eq(List<Obj_AI_Base> units)
        {
            if (!GlobalVariables.Player.IsDashing())
            {
                return;
            }

            var settingsEq = this.Menu.SubMenu(this.Name + "WhileDashingMenu");

            if (settingsEq.Item(settingsEq.Name + "OnlyNotStacked").GetValue<bool>() && ProviderQ.HasQ3())
            {
                return;
            }

            var dash = new Dash(GlobalVariables.Player.GetDashInfo().EndPos.To3D());



            if (!settingsEq.Item(settingsEq.Name + "Enabled").GetValue<bool>())
            {
                return;
            }

            var unitsAroundEndPos = units.Where(
                    x =>
                    x.Distance(dash.EndPosition) <= GlobalVariables.Spells[SpellSlot.Q].Range).ToList();

            var unitsKillable = units.Where(
                     x =>
                     x.Distance(dash.EndPosition) <= GlobalVariables.Spells[SpellSlot.Q].Range
                     && x.Health <= this.ProviderQ.GetDamage(x)).ToList();

            if (settingsEq.Item(settingsEq.Name + "OnlyKillable").GetValue<bool>())
            {
                if (unitsAroundEndPos.Count - unitsKillable.Count != 0)
                {
                    return;
                }
            }

            if (unitsKillable.Count > settingsEq.Item(settingsEq.Name + "MinHit").GetValue<Slider>().Value)
            {
                this.Execute(unitsKillable.MinOrDefault(x => x.Health));
            }
        }

        /// <summary>
        ///     LastHits with ranged Q
        /// </summary>
        /// <param name="units"></param>
        private void Q3(List<Obj_AI_Base> units)
        { 
            var validunits = units.Where(
                    x =>
                    x.IsValidTarget(GlobalVariables.Spells[SpellSlot.Q].Range - x.BoundingRadius / 2)
                    && x.Health <= ProviderQ.GetDamage(x)).ToList();

            var farmlocation = MinionManager.GetBestLineFarmLocation(
                validunits.ToVector3S().To2D(),
                GlobalVariables.Spells[SpellSlot.Q].Width,
                GlobalVariables.Spells[SpellSlot.Q].Range);

            var unit = validunits.MaxOrDefault(x => x.FlatGoldRewardMod);

            if (farmlocation.Position.IsValid())
            {
                Execute(farmlocation.Position);
            }
            else if (unit != null)
            {
                Execute(unit);
            }
        }

        /// <summary>
        ///     Lasth hits with Q
        /// </summary>
        /// <param name="units"></param>
        private void Q1Q2(List<Obj_AI_Base> units)
        {
            var validunits = units.Where(
                    x =>
                    x.IsValidTarget(GlobalVariables.Spells[SpellSlot.Q].Range - x.BoundingRadius / 2)
                    && x.Health <= ProviderQ.GetDamage(x));

            var unit = validunits.MaxOrDefault(x => x.FlatGoldRewardMod);

            if (unit != null)
            {
                Execute(unit);
            }
        }

        public void Execute(Obj_AI_Base unit)
        {
            if (unit.IsValid)
            {
                GlobalVariables.Spells[SpellSlot.Q].Cast(unit);
            }
        }

        public void Execute(Vector2 position)
        {
            if (position.IsValid())
            {
                GlobalVariables.Spells[SpellSlot.Q].Cast(position);
            }
        }

        #endregion
    }
}