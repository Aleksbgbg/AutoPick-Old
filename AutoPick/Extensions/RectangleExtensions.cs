namespace AutoPick.Extensions
{
    using System.Drawing;
    using System.Numerics;

    public static class RectangleExtensions
    {
        public static Vector2 Center(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X + (rectangle.Width / 2),
                               rectangle.Y + (rectangle.Height / 2));
        }
    }
}