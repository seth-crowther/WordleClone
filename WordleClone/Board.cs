using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

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
                        Tiles[currentCol, currentRow].SetLetter((char)k);
                        currentCol++;
                    }
                    if (k == Keys.Back && currentCol > 0)
                    {
                        currentCol--;
                        Tiles[currentCol, currentRow].SetLetter(' ');
                    }

                    if (k == Keys.Enter && currentCol == boardSize.X && currentRow < boardSize.Y)
                    {
                        int validation = ValidateWord(currentRow);

                        switch (validation)
                        {
                            case 0:
                                HandleGuess(currentRow);
                                currentCol = 0;
                                currentRow++;
                                break;

                            case 1:
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
            if (word == Globals.wordPicker.word)
            {
                return 1;
            }
            if (Globals.wordPicker.ValidWords.Contains(word))
            {
                return 0;
            }
            Console.WriteLine(word + " is an invalid word...");
            return -1;
        }

        public void HandleGuess(int row)
        {
            string answer = Globals.wordPicker.word;
            string guess = "";

            Dictionary<char, int> foundCounts = new Dictionary<char, int>();

            for (int i = 0; i < boardSize.X; i++)
            {
                guess += Tiles[i, row].letter;
            }

            for(int i = 0; i < answer.Length; i++)
            {
                int charCount = 0;
                char current = guess[i];


                //Loop over answer and count how many occurences there are of the current character in the guess
                for (int j = 0; j < answer.Length; j++)
                {
                    if (answer[j] == current)
                        charCount++;
                }
                
                //Add an entry into the foundCounts dictionary for the current character in the guess if it doesn't already exist
                if (!foundCounts.ContainsKey(current))
                {
                    foundCounts.Add(current, 0);
                }

                //If the char in the answer is in the same position as the char in the guess, turn the tile green and increment the foundCount for that char
                if (answer[i] == current)
                {
                    Tiles[i, row].activeColour = Tile.green;
                    foundCounts[current]++;
                }
                //If the char in the guess isn't in the answer, turn tile grey
                else if (!answer.Contains(current))
                {
                    Tiles[i, row].activeColour = Tile.grey;
                }
                else
                {
                    //If the char in the guess is in the answer but is somewhere else, check if enough occurences have already been found and turn yellow, if not turn grey
                    if (foundCounts[current] == charCount)
                        Tiles[i, row].activeColour = Tile.grey;
                    else
                    {
                        Tiles[i, row].activeColour = Tile.yellow;
                        foundCounts[current]++;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!won)
            {
                spriteBatch.Draw(sprite, boardRectangle, Color.White);
                for (int x = 0; x < boardSize.X; x++)
                {
                    for (int y = 0; y < boardSize.Y; y++)
                    {
                        Tiles[x, y].Draw(spriteBatch);
                    }
                }
            }
            else
            {
                Globals.graphicsDevice.Clear(Color.Black);
                spriteBatch.DrawString(Globals.arial, "u win", new Vector2(1000, 500), Color.White);
            }
        }

        public Rectangle CalcBoardRect()
        {
            Vector2 dimensions = boardSize * (Tile.Dims + new Vector2(Tile.tileGap, Tile.tileGap));
            dimensions -= new Vector2(Tile.tileGap);
            Vector2 position = (Globals.ScreenDims - dimensions) / 2;

            return new Rectangle(Globals.Vector2ToPoint(position), Globals.Vector2ToPoint(dimensions));
        }
    }
}
