namespace Rethought_Camera.Modules.Transitions
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp.Common;

    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.Transitions.Abstract_Base;
    using RethoughtLib.Transitions.Implementations;

    #endregion

    internal sealed class TransitionsModule : ChildBase
    {
        #region Fields

        private Dictionary<string, TransitionBase> availableTransitions = new Dictionary<string, TransitionBase>();

        /// <summary>
        ///     The previous transition
        /// </summary>
        private TransitionBase previousTransition;

        private TransitionBase activeTransitionBase;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransitionsModule" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public TransitionsModule(string name)
        {
            this.Name = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the active transition base.
        /// </summary>
        /// <value>
        ///     The active transition base.
        /// </value>
        public TransitionBase ActiveTransitionBase
        {
            get
            {
                return this.activeTransitionBase;
            }
            set
            {
                this.activeTransitionBase = value;

                this.activeTransitionBase.Duration = this.Duration;
            }
        }

        /// <summary>
        ///     Gets or sets the duration.
        /// </summary>
        /// <value>
        ///     The duration.
        /// </value>
        public double Duration { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnDisable(sender, featureBaseEventArgs);

            this.previousTransition = this.ActiveTransitionBase;

            this.ActiveTransitionBase = new QuadEaseInOut(this.Duration);
        }

        /// <summary>
        ///     Called when [enable]
        /// </summary>
        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnEnable(sender, featureBaseEventArgs);

            if (this.previousTransition == null) return;

            this.ActiveTransitionBase = this.previousTransition;
        }

        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            this.availableTransitions = new Dictionary<string, TransitionBase>();

            var circEaseOutIn = new CircEaseOutIn(this.Duration);
            var cubicEaseInOut = new CubicEaseInOut(this.Duration);
            var elasticEaseInOut = new ElasticEaseInOut(this.Duration);
            var expoEaseInOut = new ExpoEaseInOut(this.Duration);
            var quadEaseInOut = new QuadEaseInOut(this.Duration);
            var quadEaseOut = new QuadEaseOut(this.Duration);
            var quartEaseInOut = new QuartEaseInOut(this.Duration);
            var easeOutIn = new CircEaseOutIn(this.Duration);

            this.availableTransitions.Add("Circular Ease Out and In", circEaseOutIn);
            this.availableTransitions.Add("Cubic Ease Out and In", cubicEaseInOut);
            this.availableTransitions.Add("Elastical Ease Out and In", elasticEaseInOut);
            this.availableTransitions.Add("Expotential Ease Out and In", expoEaseInOut);
            this.availableTransitions.Add("Quadratic Ease Out and In", quadEaseInOut);
            this.availableTransitions.Add("Quadratic Ease Out", quadEaseOut);
            this.availableTransitions.Add("Quart Ease Out and In", quartEaseInOut);
            this.availableTransitions.Add("Ease Out and In", easeOutIn);

            var stringArray = this.availableTransitions.Keys.ToArray();

            this.Menu.AddItem(new MenuItem("transition", "Transition"))
                .SetValue(new StringList(stringArray))
                .ValueChanged +=
                (o, args) =>
                    {
                        this.ActiveTransitionBase =
                            this.availableTransitions[args.GetNewValue<StringList>().SelectedValue];
                    };

            this.Menu.AddItem(new MenuItem("duration", "Duration").SetValue(new Slider(225, 50, 2500))).ValueChanged +=
                (o, args) =>
                    {
                        this.Duration = args.GetNewValue<Slider>().Value;
                        this.ActiveTransitionBase.Duration = this.Duration;
                    };

            this.Duration = this.Menu.Item("duration").GetValue<Slider>().Value;

            this.ActiveTransitionBase =
                this.availableTransitions[this.Menu.Item("transition").GetValue<StringList>().SelectedValue];
        }

        #endregion
    }
}