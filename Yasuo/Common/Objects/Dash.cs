namespace Yasuo.Common.Objects
{
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Yasuo.Common.Utility;

    using Yasuo.Common.Provider;
    using SharpDX;

    using Color = System.Drawing.Color;

    public class Dash
    {
        public Vector3 StartPosition;

        public Vector3 EndPosition;

        public float Distance;

        public Obj_AI_Base Over;

        public Dash(Vector3 from, Obj_AI_Base over)
        {
            this.Over = over;

            this.StartPosition = from;
            this.EndPosition = StartPosition.Extend(over.ServerPosition, Variables.Spells[SpellSlot.E].Range);

            this.SetDashLength();
            this.SetDangerValue();

            this.CheckWallDash();

            this.Distance = StartPosition.Distance(EndPosition);

            //this.CheckKnockups();
        }

        public int DangerValue { get; private set; }

        public float DashTime { get; private set; }

        public float DashLenght { get; private set; }

        public bool IsWallDash { get; private set; }

        public bool WallDashSavesTime { get; protected internal set; }

        public List<Obj_AI_Hero> KnockUpHeroes { get; private set; }

        public List<Obj_AI_Minion> KnockUpMinions { get; private set; } 

        // TODO: Add Path in Skillshot (Based on Skillshot Danger value) , Add Enemies Around (Based on Priority), Add Allies Around, Add Minions Around (?)
        public void SetDangerValue()
        {
            this.DangerValue = 0;
        }

        public void SetDashTime()
        {
            this.DashTime = this.DashLenght / Variables.Spells[SpellSlot.E].Speed;
        }

        public void SetDashLength()
        {
            if (EndPosition.IsWall() && !this.IsWallDash)
            {
                var newEndPosition = WallDashLogicProvider.GetFirstWallPoint(StartPosition, EndPosition);


                // BUG: Navmesh seems broken and just returns Vector.Zero sometimes
                // Fixed with Broscience...
                if (EndPosition.Distance(newEndPosition) <= Variables.Spells[SpellSlot.E].Range
                    && newEndPosition != Vector3.Zero)
                {
                    EndPosition = newEndPosition;
                }
            }
            this.DashLenght = StartPosition.Distance(EndPosition);
        }

        public void CheckWallDash(float minWallWidth = 50)
        {
            if (this.Over.IsWallDash(this.DashLenght, minWallWidth))
            {
                IsWallDash = true;
            }
        }

        public void CheckKnockups()
        {
            foreach (var enemy in HeroManager.Enemies)
            {
                if (enemy.Distance(EndPosition) <= 375
                    && enemy.IsValid)
                {
                    KnockUpHeroes.Add(enemy);
                }
            }

            foreach (var minion in MinionManager.GetMinions(EndPosition, 375, MinionTypes.All, MinionTeam.NotAlly))
            {
                if (minion.IsValid)
                {
                    KnockUpMinions.Add((Obj_AI_Minion) minion);
                }
            }
        }

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

    }
}
