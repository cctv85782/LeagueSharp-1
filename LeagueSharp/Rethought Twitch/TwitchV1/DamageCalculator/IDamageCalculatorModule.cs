﻿namespace Rethought_Twitch.TwitchV1.DamageCalculator
{
    public interface IDamageCalculatorModule : IDamageCalculator
    {
        #region Public Properties

        int EstimatedAmountInOneCombo { get; set; }

        string Name { get; set; }

        #endregion
    }
}