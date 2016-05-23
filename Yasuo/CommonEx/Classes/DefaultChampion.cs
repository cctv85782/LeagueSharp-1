using System;

namespace Yasuo.CommonEx.Classes
{
    class DefaultChampion : IChampion
    {
        public void Load()
        {
            Console.WriteLine(@"This champion is not supported");
        }

        public string Name { get; set; } = "default";
    }
}
