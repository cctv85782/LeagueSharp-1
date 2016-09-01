﻿namespace RethoughtLib.Transitions.Implementations
{
    using global::RethoughtLib.Transitions.Abstract_Base;

    /// <summary>
    ///     The cubic ease in out.
    /// </summary>
    public class CubicEaseInOut : TransitionBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CubicEaseInOut" /> class.
        /// </summary>
        /// <param name="duration">
        ///     The duration.
        /// </param>
        public CubicEaseInOut(double duration)
            : base(duration)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The equation.
        /// </summary>
        /// <param name="time">
        ///     The t.
        /// </param>
        /// <param name="b">
        ///     The b.
        /// </param>
        /// <param name="c">
        ///     The c.
        /// </param>
        /// <param name="startTime">
        ///     The d.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public override double Equation(double time, double b, double c, double startTime)
        {
            if ((time /= startTime / 2) < 1)
            {
                return c / 2 * time * time * time + b;
            }

            return c / 2 * ((time -= 2) * time * time + 2) + b;
        }

        #endregion
    }
}