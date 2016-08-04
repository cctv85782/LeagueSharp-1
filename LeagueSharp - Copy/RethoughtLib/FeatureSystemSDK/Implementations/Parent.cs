namespace RethoughtLib.FeatureSystemSDK.Implementations
{
    using global::RethoughtLib.FeatureSystemSDK.Abstract_Classes;

    #region Using Directives



    #endregion

    public sealed class Parent : ParentBase
    {
        #region Constructors and Destructors

        public Parent(string name)
        {
            this.Name = name;

            this.OnInitializeInvoker();
        }

        #endregion

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name { get; set; }
    }
}