namespace AssemblyName
{
    #region Using Directives

    using System;

    using AssemblyName.Base;
    using AssemblyName.Champion;
    using AssemblyName.MediaLib.Exceptions;

    using LeagueSharp.Common;

    #endregion

    /// <summary>
    ///     Class that loads every component of the assembly. Should only get called once.
    /// </summary>
    internal static class Bootstrap
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            try
            {
                CustomEvents.Game.OnGameLoad += delegate
                    {
                        // Checks that only one instance of assembly is active
                        if (GlobalVariables.Assembly != null)
                        {
                            return;
                        }

                        // Assembly for specific champions
                        if (GlobalVariables.ChampionDependent
                            && GlobalVariables.SupportedChampions.Contains(
                                GlobalVariables.Player.ChampionName.ToLower()))
                        {
                            switch (GlobalVariables.Player.ChampionName)
                            {
                                case "ExampleChampionName":
                                    GlobalVariables.Assembly = new Assembly(new ChampionName());
                                    break;
                                default:
                                    throw new MissingInterfaceChampionException();
                            }
                        }
                        // Assembly for unspecific champions
                        else if (!GlobalVariables.ChampionDependent)
                        {
                            GlobalVariables.Assembly = new Assembly(new DefaultImplementation());
                        }
                    };
            }

            catch (Exception ex)
            {
                throw new BootstrapFailedLoadingException($"{GlobalVariables.Name} failed loading: ", ex);
            }
        }

        #endregion
    }
}