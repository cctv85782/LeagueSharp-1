namespace RethoughtLib.Classes.Bootstraps.Interfaces
{
    public interface ILoadable : INamable
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        void Load();

        #endregion
    }
}