namespace RethoughtLibTest.Champions.Modules.OrbwalkingModes
{
    using LeagueSharp.Common;

    using RethoughtLib.Classes.Feature;

    internal class Combo : FeatureParent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureParent"/> class.
        /// </summary>
        /// <param name="rootMenu">The root menu. (the generated menu will get attached to it)</param>
        public Combo(Menu rootMenu)
            : base(rootMenu)
        {
        }

        public override string Name => "Combo";
    }
}
