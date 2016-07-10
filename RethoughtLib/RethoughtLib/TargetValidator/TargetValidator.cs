namespace RethoughtLib.TargetValidator
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using global::RethoughtLib.TargetValidator.Interfaces;

    using LeagueSharp;

    #endregion

    public class TargetValidator
    {
        #region Fields

        /// <summary>
        ///     The invalid states
        /// </summary>
        private readonly List<ICheckable> invalidStates = new List<ICheckable>();

        /// <summary>
        ///     The object
        /// </summary>
        private Obj_AI_Base target;

        /// <summary>
        ///     Whether target is valid
        /// </summary>
        private bool valid = true;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the state.
        /// </summary>
        /// <param name="state">The state.</param>
        public void AddCheck(ICheckable state)
        {
            this.invalidStates.Add(state);
        }

        /// <summary>
        ///     Checks the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        public bool Check(Obj_AI_Base @object)
        {
            this.Reset();

            this.target = @object;

            this.Start();

            return this.valid;
        }

        /// <summary>
        ///     Removes the state.
        /// </summary>
        /// <param name="state">The state.</param>
        public void RemoveCheck(ICheckable state)
        {
            this.invalidStates.Remove(state);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Resets this instance.
        /// </summary>
        private void Reset()
        {
            this.target = null;
            this.valid = true;
        }

        /// <summary>
        ///     Starts this instance.
        /// </summary>
        private void Start()
        {
            if (this.target == null)
            {
                return;
            }

            foreach (var state in this.invalidStates)
            {
                if (this.valid == false)
                {
                    break;
                }

                this.valid = state.Check(this.target);
            }

            Console.WriteLine($"Target {this.target.Name} is an valid target: {this.valid}");
        }

        #endregion
    }
}