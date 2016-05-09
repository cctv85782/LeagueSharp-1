namespace Yasuo.Common.Algorithm.Djikstra
{
    using System.Drawing;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Yasuo.Common.LogicProvider;

    public class Connection
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Constructor for a connection
        /// </summary>
        /// <param name="from">start point</param>
        /// <param name="to">end point</param>
        /// <param name="unit">over unit (can be null)</param>
        public Connection(Point from, Point to, Obj_AI_Base unit = null)
        {
            var provider = new SweepingBladeLogicProvider();

            this.From = from;

            this.To = to;

            this.Unit = unit;

            this.Lenght = this.From.Position.Distance(this.To.Position);

            if (this.Unit != null)
            {
                this.IsDash = true;
            }

            if (this.Lenght > 0)
            {
                this.Time = !this.IsDash 
                    ? this.Lenght / GlobalVariables.Player.MoveSpeed 
                    : this.Lenght / provider.Speed();
            }
        }

        /// <summary>
        ///     Constructor for an empty connection
        /// </summary>
        public Connection() { }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Start Point
        /// </summary>
        public Point From { get; set; }

        /// <summary>
        ///     End Point
        /// </summary>
        public Point To { get; set; }

        /// <summary>
        ///     Distance from (Point)From to (Point)To
        /// </summary>
        public float Lenght { get; set; }

        /// <summary>
        ///     Dash Unit
        /// </summary>
        public Obj_AI_Base Unit { get; set; }

        /// <summary>
        ///     How long does it take to "walk/dash" the connection
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        ///     Connection is a dash
        /// </summary>
        public bool IsDash;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Draws a line from To to From
        /// </summary>
        /// <param name="points"></param>
        /// <param name="width"></param>
        /// <param name="color"></param>
        public void Draw(bool points = false, int width = 1, Color color = default(Color))
        {
            if (From != null && To != null)
            {
                Drawing.DrawLine(Drawing.WorldToScreen(From.Position), Drawing.WorldToScreen(To.Position), width, color);
            }

            if (this.Unit != null)
            {
                Render.Circle.DrawCircle(this.Unit.Position, 50, color);
            }

            if (points)
            {
                To?.Draw(50, 3, color);
                From?.Draw(50, 3, color);
            }
        }

        // Maybe redundant because the Equals extensions is the same
        /// <summary>
        ///     Compares two connections with each other
        /// </summary>
        /// <param name="connection">connection to compare with</param>
        /// <returns></returns>
        public bool SameAs(Connection connection)
        {
            if (connection.From == this.From && connection.To == this.To && connection.Unit == this.Unit)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}