//     Copyright (C) 2016 Rethought
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
//     Created: 04.10.2016 1:05 PM
//     Last Edited: 04.10.2016 1:44 PM

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
                || HeroManager.AllHeroes.All(x => x != args.Target)) return;

            this.focus = args.Start;
        }

        #endregion
    }
}