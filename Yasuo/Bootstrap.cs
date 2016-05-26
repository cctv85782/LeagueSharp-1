namespace Yasuo
{
    #region Using Directives

    using System;

    using global::Yasuo.CommonEx.Classes;
    using global::Yasuo.Yasuo;

    using LeagueSharp.Common;

    #endregion

    /// <summary>
    ///     Class that loads every component of the assembly. Should only get called once.
    /// </summary>
    internal class Bootstrap
    {
        #region Methods

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        internal void Initialize()
        {
            try
            {
                CustomEvents.Game.OnGameLoad += delegate
                    {


                        if (GlobalVariables.Assembly != null)
                        {
                            return;
                        }

                        if (GlobalVariables.ChampionDependent)
                        {
                            if (GlobalVariables.SupportedChampions.Contains(
                                    GlobalVariables.Player.ChampionName.ToLower()))
                            {
                                switch (GlobalVariables.Player.ChampionName)
                                {
                                    case "Yasuo":
                                        GlobalVariables.Assembly = new Assembly(new ChampionYasuo());
                                        break;
                                }
                            }

                            GlobalVariables.Assembly = new Assembly(new DefaultChampion());
                        }
                    };
            }

            catch (Exception ex)
            {
                Console.WriteLine(
                    string.Format(
                        "[{0}]: Bootstrap.Initialize() Failed loading the assembly. Exception: " + ex,
                        GlobalVariables.Name));
            }
        }

        #endregion
    }
}