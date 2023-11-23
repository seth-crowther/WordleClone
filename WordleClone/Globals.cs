using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordleClone
{
    public static class Globals
    {
        public static GraphicsDevice graphicsDevice { get; set; }
        public static Vector2 ScreenDims = new Vector2(1920, 1080);
        public static SpriteFont arial;
        public static WordPicker wordPicker;

        public static Point Vector2ToPoint(Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }

        public static Vector2 PointToVector2(Point point)
        {
            return new Vector2(point.X, point.Y);

        }
    }
}
