using System;

using LeagueSharp;

namespace Yasuo.CommonEx
{
    public delegate void EventHandler(EventArgs args);

    internal class Events
    {
        public static event EventHandler OnPreUpdate;

        public static event EventHandler OnUpdate;

        public static event EventHandler OnPostUpdate;

        public static void Initialize()
        {
            Game.OnUpdate += OnGameUpdate;
        }

        private static void OnGameUpdate(EventArgs args)
        {
            try
            {
                OnPreUpdate?.Invoke(args);

                OnUpdate?.Invoke(args);

                OnPostUpdate?.Invoke(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
