namespace RethoughtLibTest.Utilities
{
    #region Using Directives

    using System;

    using RethoughtLib.ChatLogger;
    using RethoughtLib.ChatLogger.Implementations;
    using RethoughtLib.Classes.General_Intefaces;

    #endregion

    public class ChatLoggerTest : ILoadable
    {
        #region Fields

        /// <summary>
        ///     The chat logger
        /// </summary>
        public ChatLogger ChatLogger;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChatLoggerTest" /> class.
        /// </summary>
        public ChatLoggerTest()
        {
            this.ChatLogger = new ChatLogger();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Tests this instance.
        /// </summary>
        public void Test()
        {
            var logger = new DefaultLogger();
            var format = new DefaultFormat();

            logger.Format = format;

            this.ChatLogger.Add(logger);

            foreach (var message in this.ChatLogger.LoggedMessages)
            {
                Console.WriteLine(message.FormatedMessage);
            }

            /* do something with the logged stuff */
        }

        #endregion

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; } = "Utility";

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public void Load()
        {
            this.Test();
            Console.WriteLine("Loaded ChatLogger and tested.");
        }
    }
}