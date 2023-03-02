using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public char letter { get; private set; }

        private Vector2 position;
        private Rectangle myRect;

        public Tile(int x, int y)
        {
            letter = ' ';

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

        public void SetLetter(char input)
        {
            letter = input;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(activeColour, myRect, Color.White);
            if (letter != ' ')
            {
                spriteBatch.DrawString(Globals.arial, letter.ToString(), position, Color.Black);
            }
        }

        public Rectangle CalcRect(int x, int y)
        {
            Vector2 boardPos = new Vector2(Board.boardRectangle.Left, Board.boardRectangle.Top);

            position = boardPos + (new Vector2(x, y) * (Dims + new Vector2(tileGap, tileGap)));
            return new Rectangle(Globals.Vector2ToPoint(position), Globals.Vector2ToPoint(Dims));
        }
    }
}
