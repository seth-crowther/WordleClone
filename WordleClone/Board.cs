using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WordleClone
{
    public class Board
    {
        private static Vector2 boardSize = new Vector2(5, 6);
        public static Rectangle boardRectangle;
        private Tile[,] Tiles;
        private static Texture2D sprite;
        private int currentRow;
        private int currentCol;
        private const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private KeyboardState ks;
        private Keys[] lastFramePressedKeys;
        private bool won;
        private bool invalidGuess = false;
        private bool lost = false;

        public Board()
        {
            Tiles = new Tile[(int)boardSize.X, (int)boardSize.Y];
            currentRow = 0;
            currentCol = 0;
            lastFramePressedKeys = new Keys[0];
        }

        public void Initialise()
        {
            sprite = new Texture2D(Globals.graphicsDevice, 1, 1);
            sprite.SetData(new[] { Color.White });

            boardRectangle = CalcBoardRect();

            for (int x = 0; x < boardSize.X; x++)
            {
                for (int y = 0; y < boardSize.Y; y++)
                {
                    Tiles[x, y] = new Tile(x, y);
                    Tiles[x, y].Initialise();
                }
            }
        }

        public void Update()
        {
            ks = Keyboard.GetState();
            foreach(Keys k in ks.GetPressedKeys())
            {
                if (!lastFramePressedKeys.Contains(k))
                {
                    if (validChars.Contains((char)k) && currentCol < boardSize.X)
                    {
                        Tiles[currentCol, currentRow].SetLetter("" + (char)k);
                        currentCol++;
                    }
                    if (k == Keys.Back && currentCol > 0)
                    {
                        currentCol--;
                        Tiles[currentCol, currentRow].SetLetter(" ");
                    }

                    if (k == Keys.Enter && currentCol == boardSize.X && currentRow < boardSize.Y)
                    {
                        switch (ValidateWord(currentRow))
                        {
                            case -1:
                                // Invalid guess, don't do anything
                                break;
                            
                            case 0:
                                // Valid guess but not correct, handle as normal
                                HandleGuess(currentRow);
                                currentCol = 0;
                                currentRow++;

                                if (currentRow >= boardSize.Y)
                                {
                                    lost = true;
                                }

                                break;

                            case 1:
                                // Correct answer! Stop game
                                HandleGuess(currentRow);
                                won = true;
                                break;
                        }
                    }
                }
            }
            lastFramePressedKeys = ks.GetPressedKeys();
        }

        public int ValidateWord(int row)
        {
            string word = "";
            for (int i = 0; i < boardSize.X; i++)
            {
                word += Tiles[i, row].letter;
            }

            if (word == Globals.wordPicker.answer)
            {
                invalidGuess = false;
                return 1;
            }
            if (Globals.wordPicker.IsValidGuess(word))
            {
                invalidGuess = false;
                return 0;
            }
            invalidGuess = true;
            return -1;
        }

        public void HandleGuess(int row)
        {
            StringBuilder answer = new StringBuilder(Globals.wordPicker.answer);
            string guess = "";

            // Get guess from input 
            for (int i = 0; i < boardSize.X; i++)
            {
                guess += Tiles[i, row].letter;
            }

            // Mark all green tiles
            for (int i = 0; i < answer.Length; i++)
            {
                if (guess[i] == answer[i])
                {
                    Tiles[i, row].activeColour = Tile.green;
                    answer[i] = ' ';
                }
            }

            // Mark yellow tiles
            for (int i = 0; i < answer.Length; i++)
            {
                if (answer.ToString().Contains(guess[i]) && !(Tiles[i, row].activeColour == Tile.green))
                {
                    Tiles[i, row].activeColour = Tile.yellow;
                    // Find first occurrence of character and make it " "
                    int index = answer.ToString().IndexOf(guess[i]);
                    answer[index] = ' ';
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, boardRectangle, Color.White);
            for (int x = 0; x < boardSize.X; x++)
            {
                for (int y = 0; y < boardSize.Y; y++)
                {
                    Tiles[x, y].Draw(spriteBatch);
                }
            }

            if (invalidGuess)
            {
                DrawMessage(spriteBatch, "Invalid Word");
            }
            else if (won)
            {
                DrawMessage(spriteBatch, "Nice Work!");
            }
            else if (lost)
            {
                DrawMessage(spriteBatch, "Unlucky!");
            }
        }

        public Rectangle CalcBoardRect()
        {
            Vector2 dimensions = boardSize * (Tile.Dims + new Vector2(Tile.tileGap, Tile.tileGap));
            dimensions -= new Vector2(Tile.tileGap);
            Vector2 position = (Globals.ScreenDims - dimensions) / 2;

            return new Rectangle(Globals.Vector2ToPoint(position), Globals.Vector2ToPoint(dimensions));
        }

        public void DrawMessage(SpriteBatch spriteBatch, string message)
        {
            Vector2 textDims = Globals.arial.MeasureString(message);
            Vector2 textPos = (Globals.ScreenDims / 2) - (textDims / 2) + new Vector2(0, 400);
            spriteBatch.DrawString(Globals.arial, message, textPos, Color.White);
        }
    }
}
