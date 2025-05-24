using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTTE
{
    public class ConsoleManager
    {
        public bool darkThemeOn = true;
        public bool fullScreenOn = false;

        static readonly int initWidth = Console.WindowWidth;
        static readonly int initHeight = Console.WindowHeight;
        public ConsoleManager() { }
        public void ToggleTheme()
        {
            if (darkThemeOn)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                darkThemeOn = !darkThemeOn;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.BackgroundColor = ConsoleColor.Black;
                darkThemeOn = !darkThemeOn;
            }
            Console.Clear();
        }
        public void ToggleFullScreen()
        {
            if (fullScreenOn)
            {
                Console.SetWindowSize(initWidth, initHeight);
                fullScreenOn = !fullScreenOn;
            }
            else
            {
                Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
                fullScreenOn = !fullScreenOn;
            }
        }

        public void ClearConsole()
        {
            Console.Clear();
        }
    }
}
