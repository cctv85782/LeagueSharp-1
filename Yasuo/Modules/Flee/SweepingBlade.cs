// TODO: Add Multi Pathing System. The Idea is to get some paths that are equally good and compare them then. This way you could do things like if Path A is safer than Path B in Situation X choose Path A

namespace Yasuo.Modules.Flee
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Yasuo.Common.Algorithm.Djikstra;
    using Yasuo.Common.Classes;
    using Yasuo.Common.Objects;
    using Yasuo.Common.Provider;
    using Yasuo.Common.Utility;

    internal class SweepingBlade : Child<Modules>
    {
        public SweepingBlade(Modules parent)
            : base(parent)
        {
            this.OnLoad();
        }

        public Path Path;

        public override string Name => "Flee";

        public SweepingBladeLogicProvider ProviderE;

        public TurretLogicProvider ProviderTurret;

        protected override void OnEnable()
        {
            Game.OnUpdate += this.OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += this.OnProcessSpellCast;
            Drawing.OnDraw += this.OnDraw;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            Game.OnUpdate -= this.OnUpdate;
            Obj_AI_Base.OnProcessSpellCast -= this.OnProcessSpellCast;
            Drawing.OnDraw -= this.OnDraw;
            base.OnDisable();
        }

        protected override sealed void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);
            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            // Spell Settings
            this.Menu.AddItem(
                new MenuItem(this.Name + "Keybind", "Keybind").SetValue(new KeyBind('A', KeyBindType.Press)));

            this.Menu.AddItem(
                new MenuItem(this.Name + "PathAroundSkillShots", "[Experimental] Try to Path around Skillshots").SetValue(
                    true).SetTooltip("if this is enabled, the assembly will path around a skillshot if a path is given"));


            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        protected override void OnInitialize()
        {
            this.ProviderE = new SweepingBladeLogicProvider();
            this.ProviderTurret = new TurretLogicProvider();

            base.OnInitialize();
        }

        public void OnUpdate(EventArgs args)
        {
            try
            {
                if (!this.Menu.Item(this.Name + "Keybind").GetValue<KeyBind>().Active
                    || !Variables.Spells[SpellSlot.E].IsReady())
                {
                    return;
                }

                Variables.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

                var targetedVector = Game.CursorPos;

                if (targetedVector != Vector3.Zero)
                {
                    this.Path = this.ProviderE.GetPath(targetedVector);
                }

                // if a path is given, and the first unit of the path is in dash range
                if (this.Path != null
                    && Variables.Player.Distance(this.Path.Connections.First().Over.ServerPosition) <= Variables.Spells[SpellSlot.E].Range)
                {
                    this.Execute(Path.Connections.First().Over);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        public void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender != Variables.Player || args.SData.Name != "YasuoDashWrapper")
            {
                return;
            }

            var connectionToRemove = this.Path?.Connections.First(x => x.Over == args.Target);

            if (connectionToRemove != null)
            {
                this.Path.RemoveConnection(connectionToRemove);
            }
        }

        public void OnDraw(EventArgs args)
        {
            //if (this.Path = null)
            //{
            //    Console.WriteLine("Cant draw Path == null");
            //}
            //this.GapClosePath?.RealPath.Draw();
            this.Path?.Draw();
            //this.Path?.DashObject?.Draw();
        }

        private void Execute(Obj_AI_Base unit)
        {
            try
            {
                if (unit == null || !unit.IsValidTarget() || unit.HasBuff("YasuoDashWrapper"))
                {
                    return;
                }

                Variables.Spells[SpellSlot.E].CastOnUnit(unit);
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Modules/Flee/SweepingBlade/Execute(): " + ex);
            }
        }
    }
}