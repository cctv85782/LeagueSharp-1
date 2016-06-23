﻿namespace RethoughtLib.Transitions
{
    #region Using Directives

    using LeagueSharp.Common;

    using SharpDX;

    #endregion

    /// <summary>
    ///     The transition.
    /// </summary>
    public abstract class Transition
    {
        #region Fields

        /// <summary>
        ///     The end position.
        /// </summary>
        private Vector2 endPosition;

        /// <summary>
        ///     The final value.
        /// </summary>
        private float finalValue;

        /// <summary>
        ///     The last value.
        /// </summary>
        private float lastValue;

        /// <summary>
        ///     Gets or sets the last position.
        /// </summary>
        private Vector2 startPosition;

        /// <summary>
        ///     The start value.
        /// </summary>
        private float startValue;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Transition" /> class.
        /// </summary>
        /// <param name="duration">
        ///     The duration.
        /// </param>
        protected Transition(double duration)
        {
            this.Duration = duration;
            this.StartTime = -9999999;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the duration.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        ///     Gets a value indicating whether moving.
        /// </summary>
        public bool Moving => Time < this.StartTime + this.Duration * 1000;

        /// <summary>
        ///     Gets or sets the start time.
        /// </summary>
        public float StartTime { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the time.
        /// </summary>
        private static float Time => Utils.TickCount;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The equation.
        /// </summary>
        /// <param name="t">
        ///     The t.
        /// </param>
        /// <param name="b">
        ///     The b.
        /// </param>
        /// <param name="c">
        ///     The c.
        /// </param>
        /// <param name="d">
        ///     The d.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        public abstract double Equation(double t, double b, double c, double d);

        /// <summary>
        ///     The get position.
        /// </summary>
        /// <returns>
        ///     The <see cref="Vector2" />.
        /// </returns>
        public Vector2 GetPosition()
        {
            if (!this.Moving)
            {
                return this.endPosition;
            }

            return this.startPosition.Extend(
                this.endPosition,
                (float)
                this.Equation(
                    Time - this.StartTime,
                    0,
                    this.endPosition.Distance(this.startPosition),
                    this.Duration * 1000));
        }

        /// <summary>
        ///     The get value.
        /// </summary>
        /// <returns>
        ///     The <see cref="float" />.
        /// </returns>
        public float GetValue()
        {
            if (this.startValue == 0 && this.finalValue == 0)
            {
                this.lastValue =
                    (float)
                    this.Equation(
                        Time - this.StartTime,
                        0,
                        this.endPosition.Distance(this.startPosition),
                        this.Duration);
                return this.lastValue;
            }

            if (!this.Moving && this.StartTime > 0)
            {
                return this.lastValue;
            }

            this.lastValue =
                (float)this.Equation(Time - this.StartTime, this.startValue, this.finalValue, this.Duration * 1000);
            return this.lastValue;
        }

        /// <summary>
        ///     Starts transisting.
        /// </summary>
        /// <param name="from">
        ///     The from.
        /// </param>
        /// <param name="to">
        ///     The to.
        /// </param>
        public void Start(Vector2 from, Vector2 to)
        {
            this.startPosition = from;
            this.endPosition = to;
            this.StartTime = Utils.TickCount;
        }

        /// <summary>
        ///     The start.
        /// </summary>
        /// <param name="from">
        ///     The from.
        /// </param>
        /// <param name="to">
        ///     The to.
        /// </param>
        public void Start(float from, float to)
        {
            this.lastValue = from;
            this.startValue = from;
            this.finalValue = to;
            this.StartTime = Time;
        }

        #endregion
    }
}