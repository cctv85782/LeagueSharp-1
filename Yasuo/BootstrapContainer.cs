namespace Yasuo
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using global::Yasuo.Base;
    using CommonEx.Classes;
    using global::Yasuo.Yasuo;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    /// <summary>
    ///     Class that loads every component of the assembly. Should only get called once.
    /// </summary>
    internal class BootstrapContainer
    {
        internal PlaySharpBootstrapBase BootstrapBase = new PlaySharpBootstrap();

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

                            GlobalVariables.Assembly = new Assembly(new BaseChampion());
                        }
                    };
            }

            catch (Exception ex)
            {
                Console.WriteLine(
                    string.Format(
                        "[{0}]: BootstrapContainer.Initialize() Failed loading the assembly. Exception: " + ex,
                        GlobalVariables.Name));
            }
        }

        #endregion
    }

    internal interface IBootstrap
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void Initialize();
    }

    internal abstract class PlaySharpBootstrapBase : IBootstrap
    {
        /// <summary>
        /// The modules
        /// </summary>
        public List<ILoadable> Modules = new List<ILoadable>();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public abstract void Initialize();
    }

    internal interface INamable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }
    }

    internal interface ILoadable : INamable
    {
        /// <summary>
        /// Loads this instance.
        /// </summary>
        void Load();
    }

    internal class PlaySharpBootstrap : PlaySharpBootstrapBase
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            foreach (var module in this.Modules)
            {
                if (module.Name == ObjectManager.Player.ChampionName)
                {
                    module.Load();
                }
            }
        }
    }

    internal class ChampionYasuo : ILoadable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            throw new NotImplementedException();
        }
    }
}