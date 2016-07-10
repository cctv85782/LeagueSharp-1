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
        ///     Adds the check.
        /// </summary>
        /// <param name="check">The check.</param>
        public void AddCheck(ICheckable check)
        {
            this.invalidStates.Add(check);
        }

        /// <summary>
        ///     Adds the checks.
        /// </summary>
        /// <param name="checks">The checks.</param>
        public void AddChecks(IEnumerable<ICheckable> checks)
        {
            foreach (var check in checks)
            {
                this.AddCheck(check);
            }
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
        ///     Removes the check.
        /// </summary>
        /// <param name="check">The check.</param>
        public void RemoveCheck(ICheckable check)
        {
            this.invalidStates.Remove(check);
        }

        /// <summary>
        ///     Removes the checks.
        /// </summary>
        /// <param name="checks">The checks.</param>
        public void RemoveChecks(IEnumerable<ICheckable> checks)
        {
            foreach (var check in checks)
            {
                this.RemoveCheck(check);
            }
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

#if DEBUG
            Console.WriteLine($"[TargetValidator] Target {this.target.Name} is an valid target: {this.valid}");
#endif
        }

        #endregion
    }
}