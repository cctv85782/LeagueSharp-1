namespace Rethought_Fiora.Champions.Fiora.Modules.Evade
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;
    using LeagueSharp.Data;
    using LeagueSharp.Data.DataTypes;
    using LeagueSharp.SDK;

    using RethoughtLib.AssemblyInteractor;
    using RethoughtLib.AssemblyInteractor.Implementations;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Fiora.Champions.Fiora.Modules.Core.SpellsModule;

    #endregion

    internal class RiposteModule : ChildBase
    {
        #region Fields

        private readonly SpellsModule spellsModule;

        #endregion

        #region Constructors and Destructors

        public RiposteModule(SpellsModule spellsModule)
        {
            this.spellsModule = spellsModule;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnDisable(sender, featureBaseEventArgs);

            Game.OnUpdate -= this.GameOnOnUpdate;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnEnable(sender, featureBaseEventArgs);

            Game.OnUpdate += this.GameOnOnUpdate;
            Obj_AI_Base.OnProcessSpellCast += this.ObjAiBaseOnOnProcessSpellCast;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            var championData = Data.Get<ChampionDatabase>();

            foreach (var hero in HeroManager.Enemies)
            {
                var menu = new Menu(hero.ChampionName, hero.ChampionName);

                var champion = championData[hero.ChampionName];

                foreach (var spell in champion.Spells)
                {
                    this.Menu.AddItem(new MenuItem(spell.Name, spell.Name).SetTooltip(spell.Description));
                }

                this.Menu.AddSubMenu(menu);
            }

            this.Menu.AddItem(new MenuItem("AutoDisableEvade", "Automatically disable Evade Assemblies to block"))
                .SetValue(true);
        }

        private void GameOnOnUpdate(EventArgs args)
        {
            foreach (var detectedSkillshot in Tracker.DetectedSkillshots)
            {
            }
        }

        private void ObjAiBaseOnOnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe || sender.IsAlly || !this.spellsModule.Spells[SpellSlot.W].IsReady()
                || this.Menu.SubMenu(sender.CharData.BaseSkinName) == null
                || (bool)!this.Menu.SubMenu(sender.CharData.BaseSkinName).Item(args.SData.Name)?.GetValue<bool>())
            {
                return;
            }

#if DEBUG
            Console.WriteLine($"[{this}] Detected {args.SData.Name} from {sender.NetworkId}");
#endif

            if (this.Menu.Item("AutoDisableEvade").GetValue<bool>())
            {
                AssemblyInteractor.DisableByMenu(new EvadeSharp());
            }

            if (args.SData.TargettingType == SpellDataTargetType.Unit)
            {
            }

            if (this.Menu.Item("AutoDisableEvade").GetValue<bool>())
            {
                AssemblyInteractor.EnableByMenu(new EvadeSharp());
            }
        }

        #endregion
    }
}