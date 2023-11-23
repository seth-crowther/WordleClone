using System;
using System.Diagnostics;
using System.IO;

namespace WordleClone
{
    public class WordPicker
    {
        public string answer { get; private set; }
        public string[] validAnswers { get; private set; }
        public string[] validGuesses { get; private set; }

        private Random rnd = new Random();
        public WordPicker()
        {
            answer = "";

            validAnswers = File.ReadAllLines("validAnswers.txt");
            validGuesses = File.ReadAllLines("validGuesses.txt");

            for (int i = 0; i < validAnswers.Length; i++)
            {
                validAnswers[i] = validAnswers[i].ToUpper();
            }

            for (int i = 0; i < validGuesses.Length; i++)
            {
                validGuesses[i] = validGuesses[i].ToUpper();
            }
        }

        public void PickWord()
        {
            int index = rnd.Next(0, validAnswers.Length);
            // answer = "SCALD";
            answer = validAnswers[index];
            // Debug.WriteLine(word);
        }

        public bool IsValidGuess(string guess)
        {
            // Binary search of both lists here
            if (binaryStringSearch(0, validAnswers.Length -1, guess, validAnswers) || binaryStringSearch(0, validGuesses.Length - 1, guess, validGuesses))
            {
                return true;
            }
            return false;
        }

        public bool binaryStringSearch(int L, int R, string key, string[] array)
        {
            if (L > R)
            {
                return false;
            }
            
            int diff = key.CompareTo(array[(L + R) / 2]);
            if (diff == 0)
            {
                return true;                
            }
            else if (diff < 0)
            {
                return binaryStringSearch(L, R - 1, key, array);
            }
            else
            {
                return binaryStringSearch(L + 1, R, key, array);
            }
        }
    }
}
