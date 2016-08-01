namespace RethoughtLib.Menu
{
    #region Using Directives

    using System;

    using global::RethoughtLib.Exceptions;
    using global::RethoughtLib.Menu.Interfaces;

    using LeagueSharp.Common;

    #endregion

    #region Using Directives

    #endregion

    /// <summary>
    ///     Generates a Preset Menu to the given Menu
    /// </summary>
    public class MenuGenerator
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="MenuGenerator" /> class.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="menuPreset">The menu preset.</param>
        public MenuGenerator(Menu menu, IMenuPreset menuPreset)
        {
            this.menuPreset = menuPreset;
            this.menu = menu;

            menuPreset.Menu = menu;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Generates the menu.
        /// </summary>
        /// <exception cref="MenuGenerationException"></exception>
        /// <exception cref="System.NullReferenceException">
        ///     Get sure that you declared a valid menuPreset and a valid menu in the
        ///     constructor before generating.
        /// </exception>
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