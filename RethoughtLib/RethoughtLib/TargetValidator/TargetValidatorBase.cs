namespace RethoughtLib.TargetValidator
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using global::RethoughtLib.TargetValidator.Interfaces;

    using LeagueSharp;

    #endregion

    public abstract class TargetValidatorBase
    {
        #region Fields

        /// <summary>
        ///     The invalid states
        /// </summary>
        protected readonly List<ICheckable> ChecksList = new List<ICheckable>();

        /// <summary>
        ///     The object
        /// </summary>
        protected Obj_AI_Base Target;

        /// <summary>
        ///     Whether target is valid
        /// </summary>
        protected bool Valid = true;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the check.
        /// </summary>
        /// <param name="check">The check.</param>
        public virtual void AddCheck(ICheckable check)
        {
            this.ChecksList.Add(check);
        }

        /// <summary>
        ///     Adds the checks.
        /// </summary>
        /// <param name="checks">The checks.</param>
        public virtual void AddChecks(IEnumerable<ICheckable> checks)
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
        public virtual bool Check(Obj_AI_Base @object)
        {
            this.Reset();

            this.Target = @object;

            this.Start();

            return this.Valid;
        }

        /// <summary>
        ///     Removes the check.
        /// </summary>
        /// <param name="check">The check.</param>
        public virtual void RemoveCheck(ICheckable check)
        {
            this.ChecksList.Remove(check);
        }

        /// <summary>
        ///     Removes the checks.
        /// </summary>
        /// <param name="checks">The checks.</param>
        public virtual void RemoveChecks(IEnumerable<ICheckable> checks)
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
        protected virtual void Reset()
        {
            this.Target = null;
            this.Valid = true;
        }

        /// <summary>
        ///     Starts this instance.
        /// </summary>
        protected virtual void Start()
        {
            if (this.Target == null)
            {
                return;
            }

            foreach (var state in this.ChecksList)
            {
                if (this.Valid == false)
                {
                    break;
                }

                this.Valid = state.Check(this.Target);
            }

#if DEBUG
            Console.WriteLine($"[TargetValidator] Target {this.Target.Name} is an valid target: {this.Valid}");
#endif
        }

        #endregion
    }
}