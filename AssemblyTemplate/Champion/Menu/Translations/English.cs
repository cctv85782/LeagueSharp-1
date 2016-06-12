namespace AssemblyName.Champion.Menu.Translations
{
    using System.Collections.Generic;

    internal class English : ITranslation
    {
        public string Name { get; set; } = "English (Default)";

        public Dictionary<string, string> Strings()
        {
            var strings = new Dictionary<string, string> { };

            return strings;
        }
    }
}
