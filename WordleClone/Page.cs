using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace WordleClone
{
    public class Page
    {
        private Color BackgroundColour;
        public static Board board;
        public Page()
        {
            BackgroundColour = new Color(18, 18, 19);
            board = new Board();
        }

        public void Initialise()
        {
            board.Initialise();
        }

        public void Update()
        {
            board.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Globals.graphicsDevice.Clear(BackgroundColour);
            board.Draw(spriteBatch);
        }
    }
}
