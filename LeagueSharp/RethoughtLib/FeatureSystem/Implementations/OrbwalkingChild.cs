namespace RethoughtLib.FeatureSystem.Implementations
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RethoughtLib.CastManager.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Behaviors;
    using RethoughtLib.FeatureSystem.Guardians;

    #endregion

    public abstract class OrbwalkingChild : ChildBase
    {
        #region Fields

        /// <summary>
        ///     The behaviors
        /// </summary>
        protected readonly List<Action> Behaviors = new List<Action>();

        /// <summary>
        ///     The guardians
        /// </summary>
        protected readonly List<GuardianBase> Guardians = new List<GuardianBase>();

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the specified behavior
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns></returns>
        public OrbwalkingChild Behavior(IBehavior behavior)
        {
            return this.Behavior(behavior.Action);
        }

        /// <summary>
        ///     Adds the specified behavior
        /// </summary>
        /// <param name="behaviorAction">The behavior action.</param>
        /// <returns></returns>
        public OrbwalkingChild Behavior(Action behaviorAction)
        {
            this.Behaviors.Add(behaviorAction);

            return this;
        }

        /// <summary>
        ///     Adds the specified guardian
        /// </summary>
        /// <param name="guardianBase">The guardian.</param>
        /// <returns></returns>
        public OrbwalkingChild Guardian(GuardianBase guardianBase)
        {
            this.Guardians.Add(guardianBase);

            return this;
        }

        /// <summary>
        ///     Removes the guardian.
        /// </summary>
        /// <param name="guardianBase">The guardian.</param>
        public OrbwalkingChild RemoveGuardian(GuardianBase guardianBase)
        {
            this.Guardians.Remove(guardianBase);

            return this;
        }

        /// <summary>
        ///     Checks the guardians.
        /// </summary>
        /// <returns></returns>
        protected bool CheckGuardians()
        {
            return this.Guardians.All(x => x.Check());
        }

        #endregion
    }
}