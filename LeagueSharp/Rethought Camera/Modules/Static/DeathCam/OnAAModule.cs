namespace Rethought_Camera.Modules.Static.DeathCam
{
    #region Using Directives

    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    #endregion

    internal class OnAaModule : DeathCamModule
    {
        #region Fields

        private Vector3 focus;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "Auto-Attacks";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the position.
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetPosition()
        {
            return this.focus;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnDisable(sender, eventArgs);
            Obj_AI_Base.OnDoCast -= this.ObjAiHeroOnOnDoCast;
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs eventArgs)
        {
            base.OnEnable(sender, eventArgs);
            Obj_AI_Base.OnDoCast += this.ObjAiHeroOnOnDoCast;
        }

        private void ObjAiHeroOnOnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMinion || !args.SData.ConsideredAsAutoAttack
                || HeroManager.AllHeroes.All(x => x != args.Target))
            {
                return;
            }

            this.focus = args.Start;
        }

        #endregion
    }
}