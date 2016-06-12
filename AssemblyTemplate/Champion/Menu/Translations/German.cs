namespace AssemblyName.Champion.Menu.Translations
{
    using System.Collections.Generic;

    internal class German : ITranslation
    {
        public string Name { get; set; } = "German";

        public Dictionary<string, string> Strings()
        {
            var strings = new Dictionary<string, string> { };

            return strings;
        }
    }
}
