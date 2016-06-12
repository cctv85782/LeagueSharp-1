namespace AssemblyName.MediaLib.Menu
{
    #region Using Directives

    using System;

    using AssemblyName.Champion.Menu.Translations;

    using LeagueSharp.Common;

    #endregion

    internal class MenuTranslator
    {
        #region Fields

        private readonly ITranslation menuTranslation;

        private bool translated;

        internal Menu Menu;

        #endregion

        #region Constructors and Destructors

        public MenuTranslator(Menu menu, ITranslation translation)
        {
            this.Menu = menu;   
            this.menuTranslation = translation;
        }

        #endregion

        #region Public Methods and Operators

        public void Translate()
        {
            if (this.translated || this.menuTranslation == null)
            {
                return;
            }

            this.translated = true;

            foreach (var entry in this.menuTranslation.Strings())
            {
                this.SearchAndTranslate(entry.Key, entry.Value);
            }
        }

        #endregion

        #region Methods

        private void SearchAndTranslate(string internalName, string newDisplayName)
        {
            try
            {
                var item = this.Menu.Item(internalName);

                if (item != null)
                {
                    item.DisplayName = newDisplayName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[{GlobalVariables.Name}] Failed translating > {internalName} into {newDisplayName}. Exception: {ex}");
            }
        }

        #endregion
    }
}