﻿namespace AssemblyName.Champion.Modules.Auto
{
    using System;

    using AssemblyName.Base.Modules.Assembly;
    using AssemblyName.MediaLib.Classes.Feature;
    using AssemblyName.MediaLib.Utility;

    using LeagueSharp;
    using LeagueSharp.Common;

    internal class Smite : FeatureChild<Modules>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssemblyName.Champion.Modules.Auto.KillSteal" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public Smite(Modules parent)
            : base(parent)
        {
            this.OnLoad();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name => "Smite";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Executes on the specified target with the specified spellslot.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="spellslot">The spellslot.</param>
        public void Execute(Obj_AI_Base target, SpellSlot spellslot)
        {
            CastManager.Manager.Queque.Enqueue(5, () =>
            GlobalVariables.Spells[spellslot].Cast(target));
        }

        // TODO
        /// <summary>
        ///     Raises the <see cref="E:Update" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public void OnUpdate(EventArgs args)
        {
            throw new NotImplementedException("Smite is not implemented");
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [disable].
        /// </summary>
        protected override void OnDisable()
        {
            Events.OnUpdate -= this.OnUpdate;
            base.OnDisable();
        }

        /// <summary>
        ///     Called when [enable].
        /// </summary>
        protected override void OnEnable()
        {
            Events.OnUpdate += this.OnUpdate;
            base.OnEnable();
        }

        /// <summary>
        ///     Called when [initialize].
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        // TODO
        /// <summary>
        ///     Called when [load].
        /// </summary>
        protected sealed override void OnLoad()
        {
            this.Menu = new Menu(this.Name, this.Name);

            this.Menu.AddItem(new MenuItem(this.Name + "Enabled", "Enabled").SetValue(true));

            this.Parent.Menu.AddSubMenu(this.Menu);
        }

        #endregion
    }
}