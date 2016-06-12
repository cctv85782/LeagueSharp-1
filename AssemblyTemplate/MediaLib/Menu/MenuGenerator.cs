namespace AssemblyName.MediaLib.Menu
{
    using AssemblyName.MediaLib.Menu.Interfaces;

    #region Using Directives

    

    #endregion

    internal class MenuGenerator
    {
        #region Fields

        /// <summary>
        /// The menu set
        /// </summary>
        private readonly IMenuSet menuSet = null;

        private bool initialized;

        #endregion

        #region Constructors and Destructors

        public MenuGenerator(IMenuSet menuSet)
        {
            this.menuSet = menuSet;
        }

        #endregion

        #region Public Methods and Operators

        public void Generate()
        {
            if (this.initialized)
            {
                return;
            }

            this.initialized = true;

            this.menuSet.Generate();
        }

        #endregion
    }
}