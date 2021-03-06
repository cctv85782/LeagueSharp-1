﻿namespace Rethought_Kayle.KayleV1.Drawings
{
    #region Using Directives

    using Rethought_Kayle.KayleV1.DamageCalculator;

    using SharpDX;

    #endregion

    public interface IDamageDrawing : IDamageCalculatorModule
    {
        #region Public Properties

        Color Color { get; set; }

        #endregion

        #region Public Methods and Operators

        float Draw(Vector2 startPosition, Vector2 endPosition, float width);

        #endregion
    }
}