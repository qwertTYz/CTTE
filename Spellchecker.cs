using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CTTE
{
    public class Spellchecker
    {
        // Levenstein distance maybe?

        public string[] Vocabulary { get; set; }

        public ConsoleManager _ConsoleManager { get; set; }

        public Spellchecker(string[] vocabulary, ConsoleManager consoleManager)
        {
            Vocabulary = vocabulary;
            _ConsoleManager = consoleManager;
        }

        public void Check(char[] text)
        {
            Console.Clear();

            string s = new string(text);

            string[] words = s.Split(' ');

            foreach (string word in words)
            {
                if (!Vocabulary.Contains(word))
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(word + ' ');                   
                   
                    if (_ConsoleManager.darkThemeOn)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.Write(word + ' ');
                }
            }

            Console.WriteLine("\n\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
