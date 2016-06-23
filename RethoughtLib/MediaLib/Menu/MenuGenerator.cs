namespace RethoughtLib.Menu
{
    #region Using Directives

    using System;

    using LeagueSharp.Common;

    using RethoughtLib.Exceptions;
    using RethoughtLib.Menu.Interfaces;

    #endregion

    #region Using Directives

    #endregion

    internal class MenuGenerator
    {
        #region Fields

        /// <summary>
        ///     The menu
        /// </summary>
        private readonly Menu menu = null;

        /// <summary>
        ///     The menu set
        /// </summary>
        private readonly IMenuPreset menuPreset = null;

        /// <summary>
        ///     bool that determines whether the generator generated something successfully or not
        /// </summary>
        private bool generated;

        #endregion

        #region Constructors and Destructors

        public MenuGenerator(Menu menu, IMenuPreset menuPreset)
        {
            this.menuPreset = menuPreset;
            this.menu = menu;

            menuPreset.Menu = menu;
        }

        #endregion

        #region Public Methods and Operators

        public void Generate()
        {
            try
            {
                if (this.generated)
                {
                    throw new MenuGenerationException(
                        $"The MenuSet {this.menuPreset} already got generated in this instance.");
                }

                if (this.menuPreset == null || this.menu == null)
                {
                    throw new NullReferenceException(
                        "Get sure that you declared a valid menuPreset and a valid menu in the constructor before generating.");
                }

                this.generated = true;

                this.menuPreset.Generate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #endregion
    }
}