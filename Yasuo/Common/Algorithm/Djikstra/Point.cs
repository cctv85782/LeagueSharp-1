namespace Yasuo.Common.Algorithm.Djikstra
{
    using LeagueSharp.Common;

    using SharpDX;

    public class Point
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public Point(Vector3 position)
        {
            if (position.IsValid())
            {
                this.Position = position;
            }
        }

        /// <summary>
        ///     Empty Constructor
        /// </summary>
        public Point() { }

        /// <summary>
        ///     Position of the point
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        ///     Draws a circle around the point
        /// </summary>
        /// <param name="radius">Radius of the circle</param>
        /// <param name="width">Width of the circle</param>
        /// <param name="color">Color of the circle</param>
        public void Draw(int radius, int width, System.Drawing.Color color)
        {
            Render.Circle.DrawCircle(Position, radius, color, width);
        }
    }
}
