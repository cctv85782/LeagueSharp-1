namespace Rethought_Yasuo.Yasuo.Modules
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RethoughtLib.CastManager.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Abstract_Classes;

    using Rethought_Yasuo.Yasuo.Modules.Behaviors;
    using Rethought_Yasuo.Yasuo.Modules.Guardians;

    #endregion

    internal abstract class OrbwalkingChild : ChildBase
    {
        #region Fields

        protected readonly List<Action> Behaviors = new List<Action>();

        protected readonly List<Func<bool>> Guardians = new List<Func<bool>>();

        protected OrbwalkingChild(ICastManager castManager = null)
        {
            this.CastManager = castManager;
        }

        internal ICastManager CastManager { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the specified guardian
        /// </summary>
        /// <param name="guardian">The guardian.</param>
        /// <returns></returns>
        internal OrbwalkingChild Behavior(IBehavior guardian)
        {
            return this.Behavior(guardian.Action);
        }

        /// <summary>
        ///     Adds the specified guardian
        /// </summary>
        /// <param name="guardianFunc">The guardian function</param>
        /// <returns></returns>
        internal OrbwalkingChild Behavior(Action behaviorAction)
        {
            this.Behaviors.Add(behaviorAction);

            return this;
        }

        /// <summary>
        ///     Adds the specified guardian
        /// </summary>
        /// <param name="guardian">The guardian.</param>
        /// <returns></returns>
        internal OrbwalkingChild Guardian(IGuardian guardian)
        {
            return this.Guardian(guardian.Func);
        }

        /// <summary>
        ///     Adds the specified guardian
        /// </summary>
        /// <param name="guardianFunc">The guardian function</param>
        /// <returns></returns>
        internal OrbwalkingChild Guardian(Func<bool> guardianFunc)
        {
            this.Guardians.Add(guardianFunc);

            return this;
        }

        /// <summary>
        ///     Removes the guardian.
        /// </summary>
        internal OrbwalkingChild RemoveGuardian(Func<bool> guardianFunc)
        {
            if (this.Guardians.Contains(guardianFunc))
            {
                this.Guardians.Remove(guardianFunc);
            }

            return this;
        }

        /// <summary>
        ///     Removes the guardian.
        /// </summary>
        /// <param name="guardian">The guardian.</param>
        internal OrbwalkingChild RemoveGuardian(IGuardian guardian)
        {
            return this.RemoveGuardian(guardian.Func);
        }

        /// <summary>
        /// Checks the guardians.
        /// </summary>
        /// <returns></returns>
        protected internal bool CheckGuardians()
        {
            return this.Guardians.Select(guardian => guardian.Invoke()).Any(result => result);
        }

        #endregion
    }
}