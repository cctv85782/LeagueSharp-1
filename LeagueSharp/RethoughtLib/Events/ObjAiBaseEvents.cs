namespace RethoughtLib.Events
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    public static class ObjAiBaseEvents
    {
        #region Static Fields

        private static readonly List<Obj_AI_Base> DeadObjectCache = new List<Obj_AI_Base>();

        #endregion

        #region Constructors and Destructors

        static ObjAiBaseEvents()
        {
            Game.OnUpdate += OnUpdate;
        }

        #endregion

        #region Events

        public static event EventHandler<OnDeathEventArgs> OnDeath;

        #endregion

        #region Methods

        private static void OnOnDeath(OnDeathEventArgs eventArgs)
        {
            OnDeath?.Invoke(null, eventArgs);
        }

        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private static void OnUpdate(EventArgs args)
        {
            foreach (var obj in ObjectManager.Get<Obj_AI_Base>())
            {
                if (!obj.IsDead || DeadObjectCache.Contains(obj)) continue;

                DeadObjectCache.Add(obj);

                Utility.DelayAction.Add((int)obj.DeathDuration, () => { DeadObjectCache.Remove(obj); });

                OnOnDeath(new OnDeathEventArgs(obj));
            }
        }

        #endregion
    }

    public class OnDeathEventArgs : EventArgs
    {
        #region Constructors and Destructors

        public OnDeathEventArgs(Obj_AI_Base sender)
        {
            this.Sender = sender;
        }

        #endregion

        #region Public Properties

        public Obj_AI_Base Sender { get; set; }

        #endregion
    }
}