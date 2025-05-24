using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTTE
{
    public class Editor
    {   
        private ConsoleManager ConsoleManager {  get; set; }
        private CalculatorManager CalculatorManager { get; set; }

        private Spellchecker _Spellchecker { get; set; }
        public Editor(ConsoleManager consoleManager, CalculatorManager calculatorManager, Spellchecker spellchecker)
        {
            ConsoleManager = consoleManager;
            CalculatorManager = calculatorManager;
            _Spellchecker = spellchecker;
        }
        
        // fix encoding to be UTF-8
        public void OpenEditor(FileStream fs)
        {
            Console.Clear();
            GapBuffer gapBuffer = new GapBuffer();

            for (int i = 0; i < fs.Length; i++) gapBuffer.Insert((char)fs.ReadByte());

            Console.CursorVisible = true;
            Console.SetCursorPosition(0, 0);

            while (true)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Escape && cki.Modifiers == ConsoleModifiers.Shift)
                {
                    // exit and don't save
                    Console.Clear();
                    return;
                }
                else if (cki.Key == ConsoleKey.Escape)
                {
                    // save and exit
                    foreach (char c in gapBuffer.array) fs.WriteByte((byte)c);
                    Console.Clear();
                    return;
                }
                else if (cki.Key == ConsoleKey.LeftArrow)
                {
                    gapBuffer.Left();
                }
                else if (cki.Key == ConsoleKey.RightArrow)
                {
                    gapBuffer.Right();
                }
                else if (cki.Modifiers == ConsoleModifiers.Control && cki.Key == ConsoleKey.Backspace)
                {
                    if (gapBuffer.left != 0 && gapBuffer.array[gapBuffer.left - 1] != '\\')
                    {
                        // bugged
                        gapBuffer.DeletePrevWord();
                    }
                    else gapBuffer.Delete();
                }
                else if (cki.Modifiers == ConsoleModifiers.Control && cki.Key == ConsoleKey.Delete)
                {
                    gapBuffer.DeleteNextWord();
                }
                else if (cki.Key == ConsoleKey.Backspace)
                {
                    gapBuffer.Delete();
                }
                else if (cki.Key == ConsoleKey.Spacebar)
                {
                    gapBuffer.Insert(' ');
                }
                else if (cki.Key == ConsoleKey.Enter)
                {
                    gapBuffer.Insert('\n');
                }
                else if (cki.Modifiers == ConsoleModifiers.Control && (cki.Key == ConsoleKey.D8 || cki.Key == ConsoleKey.NumPad8))
                {
                    ConsoleManager.ToggleTheme();
                    gapBuffer.PrintBuffer();
                }
                else if (cki.Modifiers == ConsoleModifiers.Control && (cki.Key == ConsoleKey.D4 || cki.Key == ConsoleKey.NumPad4))
                {
                    ConsoleManager.ToggleFullScreen();
                    gapBuffer.PrintBuffer();
                }
                else if (cki.Modifiers == ConsoleModifiers.Control && (cki.Key == ConsoleKey.D6 || cki.Key == ConsoleKey.NumPad6))
                {
                    var digits = CalculatorManager.EnterExpression();
                    foreach(char d in digits)
                    {
                        gapBuffer.Insert(d);
                    }
                    Console.Clear();
                    gapBuffer.PrintBuffer();
                }
                else if (cki.Modifiers == ConsoleModifiers.Control && (cki.Key == ConsoleKey.D7 || cki.Key == ConsoleKey.NumPad7))
                {
                    _Spellchecker.Check(gapBuffer.array);
                    Console.Clear();
                    gapBuffer.PrintBuffer();
                }
                else
                {
                    gapBuffer.Insert(cki.KeyChar);
                }
            }
        }
    }
}
