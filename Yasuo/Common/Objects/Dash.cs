namespace Yasuo.Common.Objects
{
    using System.Collections.Generic;
    using System.Linq;

    using ClipperLib;

    using LeagueSharp;
    using LeagueSharp.Common;
    using LeagueSharp.SDK;

    using SharpDX;

    using Yasuo.Common.Algorithm.Djikstra;
    using Yasuo.Common.Provider;

    using Color = System.Drawing.Color;
    using Geometry = LeagueSharp.Common.Geometry;
    using MinionTypes = LeagueSharp.Common.MinionTypes;
    using Variables = Yasuo.Variables;

    public class Dash
    {
        #region Fields

        public float Distance;

        public Vector3 EndPosition;

        public bool InSkillshot;

        public List<Obj_AI_Hero> KnockUpHeroes = new List<Obj_AI_Hero>();

        public List<Obj_AI_Base> KnockUpMinions = new List<Obj_AI_Base>();

        public Obj_AI_Base Over;

        public Vector3 StartPosition;

        #endregion

        #region Constructors and Destructors

        public Dash(Vector3 from, Obj_AI_Base over)
        {
            this.Over = over;

            this.StartPosition = from;
            this.EndPosition = Geometry.Extend(this.StartPosition, over.ServerPosition, Variables.Spells[SpellSlot.E].Range);

            this.SetDashLength();
            this.SetDangerValue();

            this.CheckWallDash();

            this.Distance = Geometry.Distance(this.StartPosition, EndPosition);

            this.CheckKnockups();
            this.CheckSkillshot();
        }

        public Dash(Obj_AI_Base over)
        {
            this.Over = over;

            this.StartPosition = Variables.Player.ServerPosition;
            this.EndPosition = Geometry.Extend(this.StartPosition, over.ServerPosition, Variables.Spells[SpellSlot.E].Range);

            this.SetDashLength();
            this.SetDangerValue();

            this.CheckWallDash();

            this.Distance = Geometry.Distance(this.StartPosition, EndPosition);

            this.CheckKnockups();
        }

        #endregion

        #region Public Properties

        public int DangerValue { get; private set; }

        public float DashLenght { get; private set; }

        public float DashTime { get; private set; }

        public bool IsWallDash { get; private set; }

        public bool WallDashSavesTime { get; protected internal set; }

        #endregion

        #region Public Methods and Operators

        public void Draw()
        {
            var color = Color.White;

            if (this.EndPosition.CountEnemiesInRange(375) > 0)
            {
                color = Color.Red;
            }
            Drawing.DrawLine(
                Drawing.WorldToScreen(this.StartPosition),
                Drawing.WorldToScreen(this.EndPosition),
                4f,
                color);

            Render.Circle.DrawCircle(this.EndPosition, 350, color);
        }

        #endregion

        #region Methods

        private void CheckKnockups()
        {
            foreach (var enemy in HeroManager.Enemies)
            {
                if (Geometry.Distance(enemy, EndPosition) <= 375 && enemy.IsValid)
                {
                    KnockUpHeroes.Add(enemy);
                }
            }

            foreach (var minion in MinionManager.GetMinions(EndPosition, 350, MinionTypes.All, MinionTeam.NotAlly))
            {
                if (minion.IsValid)
                {
                    KnockUpMinions.Add(minion);
                }
            }
        }

        private void CheckSkillshot()
        {
            var skillshotDict = new Dictionary<Skillshot, Geometry.Polygon>();

            foreach (var skillshot in LeagueSharp.SDK.Tracker.DetectedSkillshots)
            {
                var polygon = new Geometry.Polygon();

                switch (skillshot.SData.SpellType)
                {
                    case SpellType.SkillshotLine:
                        polygon = new Geometry.Polygon.Rectangle(
                            skillshot.StartPosition,
                            skillshot.EndPosition,
                            skillshot.SData.Radius);
                        break;
                    case SpellType.SkillshotCircle:
                        polygon = new Geometry.Polygon.Circle(skillshot.EndPosition, skillshot.SData.Radius);
                        break;
                    case SpellType.SkillshotArc:
                        polygon = new Geometry.Polygon.Sector(
                            skillshot.StartPosition,
                            skillshot.Direction,
                            skillshot.SData.Angle,
                            skillshot.SData.Radius);
                        break;
                }

                skillshotDict.Add(skillshot, polygon);
            }

            foreach (var skillshot in skillshotDict)
            {
                var clipperpath = skillshot.Value.ToClipperPath();
                var connectionpolygon = new Geometry.Polygon.Line(StartPosition, EndPosition);
                var connectionclipperpath = connectionpolygon.ToClipperPath();

                if (clipperpath.Intersect(connectionclipperpath).Any())
                {
                    InSkillshot = true;
                }
            }
        }

        private void CheckWallDash(float minWallWidth = 50)
        {
            if (this.Over.IsWallDash(this.DashLenght, minWallWidth))
            {
                IsWallDash = true;
            }
        }

        // TODO: Add Path in Skillshot (Based on Skillshot Danger value) , Add Enemies Around (Based on Priority), Add Allies Around, Add Minions Around (?)
        private void SetDangerValue()
        {
            this.DangerValue = 0;
        }

        private void SetDashLength()
        {
            if (Utility.IsWall(this.EndPosition) && !this.IsWallDash)
            {
                var newEndPosition = WallDashLogicProvider.GetFirstWallPoint(StartPosition, EndPosition);

                // BUG: Navmesh seems broken and just returns Vector.Zero sometimes
                // Fixed with Broscience...
                if (Geometry.Distance(this.EndPosition, newEndPosition) <= Variables.Spells[SpellSlot.E].Range
                    && newEndPosition != Vector3.Zero)
                {
                    EndPosition = newEndPosition;
                }
            }
            this.DashLenght = Geometry.Distance(this.StartPosition, EndPosition);
        }

        private void SetDashTime()
        {
            this.DashTime = this.DashLenght / Variables.Spells[SpellSlot.E].Speed;
        }

        #endregion
    }
}