using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordleClone
{
    public class Tile
    {
        public static Vector2 Dims = new Vector2(100, 100);
        public static float tileGap = 10;
        public static Texture2D grey;
        public static Texture2D green;
        public static Texture2D yellow;
        public Texture2D activeColour;

        public string letter { get; private set; }

        private Vector2 position;
        private Rectangle myRect;

        public Tile(int x, int y)
        {
            letter = " ";

            myRect = CalcRect(x, y);
        }

        public void Initialise()
        {
            grey = new Texture2D(Globals.graphicsDevice, 1, 1);
            grey.SetData(new[] { Color.LightGray });
            activeColour = grey;

            green = new Texture2D(Globals.graphicsDevice, 1, 1);
            green.SetData(new[] { Color.Green });

            yellow = new Texture2D(Globals.graphicsDevice, 1, 1);
            yellow.SetData(new[] { Color.Yellow });
        }

        public void SetLetter(string input)
        {
            letter = input;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(activeColour, myRect, Color.White);
            Vector2 charDims = Globals.arial.MeasureString(letter);
            Vector2 charPos = position + (Globals.PointToVector2(myRect.Size) / 2) - (charDims / 2);
            spriteBatch.DrawString(Globals.arial, letter.ToString(), charPos, Color.Black);
        }

        public Rectangle CalcRect(int x, int y)
        {
            Vector2 boardPos = new Vector2(Board.boardRectangle.Left, Board.boardRectangle.Top);

            position = boardPos + (new Vector2(x, y) * (Dims + new Vector2(tileGap, tileGap)));
            return new Rectangle(Globals.Vector2ToPoint(position), Globals.Vector2ToPoint(Dims));
        }
    }
}
