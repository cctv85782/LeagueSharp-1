namespace Yasuo.Common.Classes
{
    public interface IChild
    {
        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether this <see cref="IChild"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        bool Enabled { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IChild"/> is handled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if handled; otherwise, <c>false</c>.
        /// </value>
        bool Handled { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IChild"/> is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        bool Initialized { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IChild"/> is unloaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if unloaded; otherwise, <c>false</c>.
        /// </value>
        bool Unloaded { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Handles the events.
        /// </summary>
        void HandleEvents();

        #endregion
    }
}