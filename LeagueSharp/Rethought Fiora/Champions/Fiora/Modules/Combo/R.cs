namespace Rethought_Fiora.Champions.Fiora.Modules.Combo
{
    #region Using Directives

    using System;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Fiora.Champions.Fiora.Modules.Core.SpellsModule;

    #endregion

    internal class R : ChildBase
    {
        private readonly SpellsModule spellsModule;

        public R(SpellsModule spellsModule)
        {
            this.spellsModule = spellsModule;
        }

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "R";

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Orbwalking.OnAttack -= this.ObjAiBase_OnOnProcessSpellCast;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Orbwalking.OnAttack += this.ObjAiBase_OnOnProcessSpellCast;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            throw new NotImplementedException();
        }

        // TODO: HealthPrediction to catch things like ignite or incoming attacks
        private void ObjAiBase_OnOnProcessSpellCast(AttackableUnit unit, AttackableUnit target)
        {
            if (!unit.IsMe)
            {
                return;
            }

            var targetObj = (Obj_AI_Base)target;

            // we low life, aa is going to kill target, enemies around us => Cast R on target
            if (targetObj.Health - ObjectManager.Player.GetAutoAttackDamage(targetObj) <= 0
                && ObjectManager.Player.HealthPercent < 5 && ObjectManager.Player.CountEnemiesInRange(2500) > 1)
            {
                this.spellsModule.Spells[SpellSlot.R].Cast(targetObj);
            }
        }

        #endregion
    }
}