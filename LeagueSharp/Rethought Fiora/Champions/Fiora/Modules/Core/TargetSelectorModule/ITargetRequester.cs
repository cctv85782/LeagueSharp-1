namespace Rethought_Fiora.Champions.Fiora.Modules.Core.TargetSelectorModule
{
    #region Using Directives

    using System;

    using LeagueSharp;

    #endregion

    public interface ITargetRequester
    {
        #region Public Events

        /// <summary>
        ///     Occurs when [target requested].
        /// </summary>
        event EventHandler<TargetRequestEventArgs> TargetRequested;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the target.
        /// </summary>
        /// <value>
        ///     The target.
        /// </value>
        Obj_AI_Hero Target { get; set; }

        /// <summary>
        ///     Gets or sets the target retrieve method.
        /// </summary>
        /// <value>
        ///     The target retrieve method.
        /// </value>
        ITargetRetrieveMethod TargetRetrieveMethod { get; set; }

        #endregion
    }

    public class TargetRequestEventArgs : EventArgs
    {
        #region Fields

        public ITargetRetrieveMethod TargetRetrieveMethod;

        #endregion
    }
}