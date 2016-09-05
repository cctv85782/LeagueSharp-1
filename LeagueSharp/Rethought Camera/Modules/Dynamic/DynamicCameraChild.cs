namespace Rethought_Camera.Modules.Dynamic
{
    #region Using Directives

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;

    #endregion

    internal abstract class DynamicCameraChild : Base
    {
        #region Public Methods and Operators

        public abstract Vector3 GetPosition();

        protected Vector3 Lerp(Vector3 start, Vector3 end, float percent)
        {
            return start + percent * (end - start);
        }

        #endregion
    }
}