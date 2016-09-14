namespace Rethought_Camera.Modules.Dynamic
{
    #region Using Directives

    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using SharpDX;

    #endregion

    internal abstract class DynamicCameraChild : Base
    {
        #region Public Properties

        public bool InternalEnabled { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the position.
        /// </summary>
        /// <returns></returns>
        public virtual Vector3 GetPosition()
        {
            if (!this.InternalEnabled) return Vector3.Zero;
            return Vector3.Zero;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Lerps the specified start.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="percent">The percent.</param>
        /// <returns></returns>
        protected Vector3 Lerp(Vector3 start, Vector3 end, float percent)
        {
            return start + percent * (end - start);
        }

        protected void ProcessKeybind(OnValueChangeEventArgs args)
        {
            if (args.GetNewValue<KeyBind>().Active)
            {
                this.InternalEnabled = true;
            }
            else
            {
                this.InternalEnabled = false;
            }
        }

        #endregion
    }
}