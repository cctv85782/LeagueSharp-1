namespace xCsTracking
{
    #region Using Directives

    using System;
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal class Program
    {
        #region Static Fields

        private static float maxminions = 0;

        private static Menu Menu;

        private static int temp = 0;

        #endregion

        #region Methods

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Menu.Item("Enabled").GetValue<bool>())
            {
                var killedminions = ObjectManager.Player.MinionsKilled;
                Drawing.DrawText(
                    Menu.Item("X").GetValue<Slider>().Value,
                    Menu.Item("Y").GetValue<Slider>().Value,
                    Color.White,
                    killedminions.ToString() + "/" + maxminions.ToString());
            }
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Menu = new Menu("xCsTracking", "xCsTracking", true);
            Menu.AddToMainMenu();

            Menu.AddItem(new MenuItem("Enabled", "Enabled").SetValue(true));
            Menu.AddItem(
                new MenuItem("X", "X").SetValue(
                    new Slider(Drawing.Direct3DDevice.Viewport.Width / 2, 0, Drawing.Direct3DDevice.Viewport.Width)));
            Menu.AddItem(new MenuItem("Y", "Y").SetValue(new Slider(0, 0, Drawing.Direct3DDevice.Viewport.Height)));
            Drawing.OnDraw += Drawing_OnDraw;
            GameObject.OnCreate += Obj_AI_Minion_OnCreate;
        }

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Obj_AI_Minion_OnCreate(GameObject sender, EventArgs args)
        {
            if (HeroManager.Player.Team == GameObjectTeam.Order && sender.Name.Contains("Minion_T200"))
            {
                temp++;
                if (temp == 3)
                {
                    maxminions++;
                    temp = 0;
                }
            }
            else if (HeroManager.Player.Team == GameObjectTeam.Chaos && sender.Name.Contains("Minion_T100"))
            {
                temp++;
                if (temp == 3)
                {
                    maxminions++;
                    temp = 0;
                }
            }
        }

        #endregion
    }
}