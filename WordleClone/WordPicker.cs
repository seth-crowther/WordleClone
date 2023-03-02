using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleClone
{
    public class WordPicker
    {
        public string word { get; private set; }
        public string[] ValidWords { get; private set; }

        private Random rnd = new Random();
        public WordPicker()
        {
            word = "";
            ValidWords = File.ReadAllLines("ValidWords.txt");
            for (int i = 0; i < ValidWords.Length; i++)
            {
                ValidWords[i] = ValidWords[i].ToUpper();
            }
        }

        public void PickWord()
        {
            int index = rnd.Next(0, ValidWords.Length);
            word = ValidWords[index];
            Console.WriteLine(word);
        }
    }
}
