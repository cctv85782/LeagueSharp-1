using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yasuo.CommonEx.Objects.Pathfinding
{
    using global::Yasuo.CommonEx.Menu;
    using global::Yasuo.CommonEx.Menu.Presets;
    using global::Yasuo.Yasuo.LogicProvider;
    using global::Yasuo.Yasuo.Menu.MenuSets.Modules;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    class SimplePathfinder : IPathfinder
    {
        #region Fields

        /// <summary>
        ///     The path
        /// </summary>
        public Path Path;

        /// <summary>
        ///     The targeted vector
        /// </summary>
        public Vector3 TargetedVector;

        /// <summary>
        ///     The E logicprovider
        /// </summary>
        private SweepingBladeLogicProvider providerE;

        /// <summary>
        ///     The R logicprovider
        /// </summary>
        private TurretLogicProvider providerTurret;

        private readonly Menu menu;

        #endregion

        public SimplePathfinder(Menu menu)
        {
            this.menu = menu;

            var menuGenerator = new MenuGenerator(new SimplePathfinderMenu(this.menu, "Simplified Pathfinder"));

            menuGenerator.Generate();
        }

        /// <summary>
        ///     Executes the path.
        /// </summary>
        public void ExecutePath()
        {
            if (this.Path?.Connections == null || !this.Path.Connections.Any()) return;

            #region Dashing

            var connection = this.Path.Connections.FirstOrDefault();

            if (connection == null)
            {
                return;
            }

            if (connection.IsDash)
            {
                if (this.menu.Item(this.menu.Name + "AutoDashing").GetValue<bool>()
                    && GlobalVariables.Player.Distance(connection.Unit.ServerPosition)
                    <= GlobalVariables.Spells[SpellSlot.E].Range
                    && connection.To.Position.Distance(
                        GlobalVariables.Player.ServerPosition.Extend(
                            connection.Unit.ServerPosition,
                            GlobalVariables.Spells[SpellSlot.E].Range)) <= 50)
                {
                    GlobalVariables.CastManager.Queque.Enqueue(3, () =>
                    GlobalVariables.Spells[SpellSlot.E].CastOnUnit(connection.Unit));
                }
            }

            #endregion

            #region Walking

            // Auto-Walking
            if (this.menu.Item(this.menu.Name + "AutoWalking").GetValue<bool>())
            {
                if (GlobalVariables.Player.ServerPosition.Distance(connection.To.Position) <= 50)
                {
                    this.Path.RemoveConnection(connection);
                }

                if (connection.Lenght > 50)
                {
                    //GlobalVariables.CastManager.Queque.Enqueue(3, () =>
                    //GlobalVariables.Orbwalker.SetOrbwalkingPoint(connection.To.Position));
                }
            }

            #endregion
        }

        public void Initialize()
        {
            this.providerE = new SweepingBladeLogicProvider();
            this.providerTurret = new TurretLogicProvider();
        }

        public Path GeneratePath()
        {
            this.Reset();

            this.FindTargetedVector();

            this.Path = this.CalculatePath(this.TargetedVector);

            return this.Path;
        }

        private Path CalculatePath(Vector3 position)
        {
            if (position == Vector3.Zero || !position.IsValid())
            {
                return null;
            }

            this.providerE.GenerateGrid(
                GlobalVariables.Player.ServerPosition,
                position,
                SweepingBladeLogicProvider.Units.All);

            if (this.providerE.GridGenerator.Grid == null || !this.providerE.GridGenerator.Grid.Connections.Any())
            {
                return null;
            }

            // TODO: PRIORITY MEDIUM > Make some more settings for this, such as Danger Value of skillshot etc.
            //if (menu.Item(menu.Name + "PathAroundSkillShots").GetValue<bool>())
            //{
            //    var skillshotList = Tracker.DetectedSkillshots.Where(x => x.SData.DangerValue > 1).ToList();

            //    this.providerE.GridGenerator.RemovePathesThroughSkillshots(skillshotList);
            //}

            // TODO: PRIORITY MEDIUM > Make some more settings for this, such as minions under turret etc. Ref; TurretLP
            if (this.menu.Item(this.menu.Name + "DontDashUnderTurret").GetValue<bool>())
            {
                foreach (var connection in this.providerE.GridGenerator.Grid.Connections.Where(x => x.IsDash).ToList())
                {
                    if (this.providerTurret.IsSafePosition(connection.To.Position)) continue;

                    this.providerE.GridGenerator.Grid.Connections.Remove(connection);
                    this.providerE.GridGenerator.RemoveDisconnectedConnections();
                }
            }

            this.providerE.FinalizeGrid();

            return this.providerE.GetPath(position);
        }

        /// <summary>
        ///     Finds the targeted vector.
        /// </summary>
        private void FindTargetedVector()
        {
            this.TargetedVector = Game.CursorPos;
        }

        private void Reset()
        {
            this.Path = null;
        }
    }
}
