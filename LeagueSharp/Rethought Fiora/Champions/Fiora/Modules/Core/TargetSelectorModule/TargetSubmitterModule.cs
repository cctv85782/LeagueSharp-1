namespace Rethought_Fiora.Champions.Fiora.Modules.Core.TargetSelectorModule
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    #endregion

    internal class TargetSubmitterModule : ChildBase
    {
        #region Fields

        /// <summary>
        ///     The target classes
        /// </summary>
        private readonly List<ITargetRequester> requesterList = new List<ITargetRequester>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; } = "TargetSubmitter";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the target requester.
        /// </summary>
        /// <param name="targetRequester">The target requester.</param>
        public void AddTargetRequester(ITargetRequester targetRequester)
        {
            this.requesterList.Add(targetRequester);

            targetRequester.TargetRequested += this.TargetRequesterOnTargetRequested;
        }

        /// <summary>
        /// Removes the target requester.
        /// </summary>
        /// <param name="requester">The requester.</param>
        public void RemoveTargetRequester(ITargetRequester requester)
        {
            this.requesterList.Remove(requester);

            requester.TargetRequested -= this.TargetRequesterOnTargetRequested;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [initialize].
        /// </summary>
        protected override void OnInitialize(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnInitialize(sender, featureBaseEventArgs);
        }

        /// <summary>
        /// Targets the requester on target requested and submits the result.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="targetRequestEventArgs">The <see cref="TargetRequestEventArgs"/> instance containing the event data.</param>
        private void TargetRequesterOnTargetRequested(object sender, TargetRequestEventArgs targetRequestEventArgs)
        {
            foreach (var requester in this.requesterList.Where(x => x.Equals(sender)))
            {
                requester.Target = requester.TargetRetrieveMethod.GetTarget();
            }
        }

        #endregion
    }
}