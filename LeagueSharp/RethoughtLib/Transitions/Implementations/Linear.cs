namespace RethoughtLib.Transitions.Implementations
{
    #region Using Directives

    using System;

    using RethoughtLib.Transitions.Abstract_Base;

    #endregion

    public class Linear : TransitionBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransitionBase" /> class.
        /// </summary>
        /// <param name="duration">
        ///     The duration.
        /// </param>
        public Linear(double duration)
            : base(duration)
        {
        }

        #endregion

        #region Public Methods and Operators

        // TODO TEST
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
            if (time == 0)
            {
                return b;
            }

            if (time == startTime)
            {
                return b + c;
            }

            return (time - startTime) * Math.Min(b, c);
        }

        #endregion
    }
}