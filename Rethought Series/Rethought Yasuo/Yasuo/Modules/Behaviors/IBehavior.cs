namespace Rethought_Yasuo.Yasuo.Modules.Behaviors
{
    #region Using Directives

    using System;

    #endregion

    internal interface IBehavior
    {
        #region Public Properties

        Action Action { get; set; }

        #endregion
    }
}