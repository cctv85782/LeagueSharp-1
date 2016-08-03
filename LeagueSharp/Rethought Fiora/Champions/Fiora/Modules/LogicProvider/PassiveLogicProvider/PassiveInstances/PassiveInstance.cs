namespace Rethought_Fiora.Champions.Fiora.Modules.LogicProvider.PassiveLogicProvider.PassiveInstances
{
    #region Using Directives

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    // TODO: REWORK
    internal class PassiveInstance : Obj_GeneralParticleEmitter
    {
        #region Fields

        internal GameObject GameObject;

        internal Obj_AI_Hero Owner;

        internal PassiveLogicProviderModule.PassiveType PassiveType;

        internal Geometry.Polygon.Sector Polygon;

        #endregion

        #region Constructors and Destructors

        public PassiveInstance(GameObject objGeneralParticleEmitter, Obj_AI_Hero hero)
            : base((ushort)objGeneralParticleEmitter.Index, (uint)objGeneralParticleEmitter.NetworkId)
        {
            this.Owner = hero;

            this.GameObject = objGeneralParticleEmitter;

            if (objGeneralParticleEmitter.Name.Contains("Base_R"))
            {
                this.PassiveType = PassiveLogicProviderModule.PassiveType.Ult;
            }

            else if (objGeneralParticleEmitter.Name.Contains("Warning"))
            {
                this.PassiveType = PassiveLogicProviderModule.PassiveType.Pre;
            }

            else if (objGeneralParticleEmitter.Name.Contains("Timeout"))
            {
                this.PassiveType = PassiveLogicProviderModule.PassiveType.TimeOut;
            }

            else
            {
                this.PassiveType = PassiveLogicProviderModule.PassiveType.Normal;
            }

            var r = this.PassiveType == PassiveLogicProviderModule.PassiveType.Ult ? 400 : 310;

            this.Polygon = new Geometry.Polygon.Sector(
                this.Owner.ServerPosition,
                this.GameObject.Orientation,
                Geometry.DegreeToRadian(70),
                r);

            this.Polygon.UpdatePolygon();
        }

        #endregion
    }
}