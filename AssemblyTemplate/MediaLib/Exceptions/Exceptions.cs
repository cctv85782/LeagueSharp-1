namespace AssemblyName.MediaLib.Exceptions
{
    #region Using Directives

    using System;
    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    ///     Exceptions that gets thrown when the Bootstrap fails loading because of any unspecified reason
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    internal class BootstrapFailedLoadingException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BootstrapFailedLoadingException" /> class.
        /// </summary>
        public BootstrapFailedLoadingException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BootstrapFailedLoadingException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BootstrapFailedLoadingException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BootstrapFailedLoadingException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception, or a null reference (Nothing in
        ///     Visual Basic) if no inner exception is specified.
        /// </param>
        public BootstrapFailedLoadingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BootstrapFailedLoadingException" /> class.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object
        ///     data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual
        ///     information about the source or destination.
        /// </param>
        protected BootstrapFailedLoadingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }

    /// <summary>
    ///     Exceptions that gets thrown when there is a missing interface for a specified champions name
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    internal class MissingInterfaceChampionException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MissingInterfaceChampionException" /> class.
        /// </summary>
        public MissingInterfaceChampionException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MissingInterfaceChampionException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MissingInterfaceChampionException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BootstrapFailedLoadingException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception, or a null reference (Nothing in
        ///     Visual Basic) if no inner exception is specified.
        /// </param>
        public MissingInterfaceChampionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MissingInterfaceChampionException" /> class.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object
        ///     data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual
        ///     information about the source or destination.
        /// </param>
        protected MissingInterfaceChampionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }

    /// <summary>
    ///     Exceptions that gets thrown when a SpellSlot is not implemented
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    internal class MissingSpellSlotImplementationException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MissingSpellSlotImplementationException" /> class.
        /// </summary>
        public MissingSpellSlotImplementationException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MissingSpellSlotImplementationException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MissingSpellSlotImplementationException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MissingSpellSlotImplementationException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception, or a null reference (Nothing in
        ///     Visual Basic) if no inner exception is specified.
        /// </param>
        public MissingSpellSlotImplementationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MissingSpellSlotImplementationException" /> class.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object
        ///     data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual
        ///     information about the source or destination.
        /// </param>
        protected MissingSpellSlotImplementationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}