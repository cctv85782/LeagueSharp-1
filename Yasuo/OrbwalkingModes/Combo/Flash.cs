namespace Yasuo.OrbwalkingModes.Combo
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Yasuo.Common.Classes;
    using Yasuo.Common.Extensions;
    using Yasuo.Common.Provider;

    using SkillshotType = SebbyLib.Prediction.SkillshotType;

    internal class Flash : Child<Combo>
    {
        public Flash(Combo parent)
            : base(parent)
        {
            this.OnLoad();
        }

        public override string Name => "Flash";

        public FlashLogicProvider Provider;

        public SteelTempestLogicProvider ProviderQ;

        private SpellSlot FlashSlot;

        protected override void OnEnable()
        {
            Game.OnUpdate += this.OnUpdate;
            Obj_AI_Base.OnPlayAnimation += this.OnAnimation;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            Game.OnUpdate -= this.OnUpdate;
            Obj_AI_Base.OnPlayAnimation -= this.OnAnimation;
            base.OnDisable();
        }

        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "[Disabled] Enabled").SetValue(true));

            // Blacklist
            var blacklist = new Menu("Blacklist", this.Name + "Blacklist");

            if (HeroManager.Enemies.Count == 0)
            {
                blacklist.AddItem(new MenuItem(blacklist.Name + "null", "No enemies found"));
            }
            else
            {
                foreach (var x in HeroManager.Enemies)
                {
                    blacklist.AddItem(new MenuItem(blacklist.Name + x.ChampionName, x.ChampionName).SetValue(false));
                }
                MenuExtensions.AddToolTip(
                    blacklist,
                    "Setting a champion to 'on', will make the script not using Flash for him anymore");
            }
            this.Menu.AddSubMenu(blacklist);

            this.Menu.AddItem(new MenuItem(this.Name + "EQFlash", "Do EQ Flash").SetValue(true)
                .SetTooltip("If enabled the assembly will perfom an EQ Flash Combo when it can hit X enemies"));
            this.Menu.AddItem(new MenuItem(this.Name + "MinHitCount", "Min HitCount EQ Flash").SetValue(new Slider(3, 1, 5)));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.Provider = new FlashLogicProvider();
            this.ProviderQ = new SteelTempestLogicProvider();

            foreach (var spell in Variables.Player.Spellbook.Spells)
            {
                if (spell.Name == "SummonerFlash")
                {
                    this.FlashSlot = spell.Slot;
                }
            }

            base.OnInitialize();
        }

        public void OnUpdate(EventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
            {
                return;
            }

            foreach (var minion in 
                MinionManager.GetMinions(
                    Variables.Player.ServerPosition, Variables.Spells[SpellSlot.E].Range, MinionTypes.All, MinionTeam.NotAlly, MinionOrderTypes.None)
                        .Where(x => !x.HasBuff("YasuoDashWrapper")).ToList())
            {
                //TODO: Add logic.
            }

        }

        public void OnAnimation(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (args.Animation == "Spell1_Dash" && sender.IsMe
                && Variables.Player.HasQ3() && Variables.Player.Spellbook.CanUseSpell(FlashSlot) == SpellState.Ready)
            {
                var targets = HeroManager.Enemies.Where(x => x.IsValid && x.Distance(Variables.Player) <= Variables.Spells[SpellSlot.Q].Range).ToList();

                var preds = targets.Select(unit => this.ProviderQ.GetPrediction(unit, true, SkillshotType.SkillshotCircle)).ToList();


                Execute(Game.CursorPos);
            }
        }

        private void Execute(Vector3 position)
        {
            if (!position.IsWall() && position.IsValid())
            {
                Variables.Player.Spellbook.CastSpell(this.FlashSlot, position);
            }
        }
    }
}

