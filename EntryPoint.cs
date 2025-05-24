using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;

namespace CTTE;

// Console-Terminal Text Editor

public sealed class EntryPoint
{
    private static readonly string HelpText =
    "\nPress 0 to exit\n" +
    "Press 2 to clear console\n" +
    "Press 1 to enter\n" +
    "Press 3 to list all .txt files and subdirectories in current directory\n" +
    "Press 4 to toggle fullscreen (NOT IMPLEMENTED)\n" +
    "Press 5 to change directory\n" +
    "Press 6 to enter calculator\n" +
    "Press 7 to delete files\n" +
    "Press 8 to toggle between night/light mode (clears the console)\n" +
    "Press 9 to display help\n" +
    "Press Backspace to display NUM LOCK\n" +
    "Press Ctrl + 6 in editor to enter expression\n" +
    "Press Ctrl + 7 in editor to spellcheck\n" +
    "Press Ctrl + 8 in editor to toggle theme\n";

    public const string txt = ".txt";

    public static void Main(string[] args)
    {

        string[] vocab = new string[] { "hello", "world", "the", "quick", "brown", "fox", "jumps", "over", "lazy", "dog" };

        Console.ForegroundColor = ConsoleColor.Green;
        
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        string vocabPath = Path.Combine(path, "CTTE_VOCABULARY.txt");

        if (!File.Exists(vocabPath))
        {
            File.WriteAllLines(vocabPath, vocab);
        }

        var vocabulary = File.ReadAllLines(vocabPath);
        Lex lex = new Lex();
        Parser parser = new Parser();
        ConsoleManager consoleManager = new ConsoleManager();
        Spellchecker spellchecker = new Spellchecker(vocabulary, consoleManager);


        CalculatorManager calculatorManager = new CalculatorManager(lex, parser);
        Editor editor = new Editor(consoleManager, calculatorManager, spellchecker);
        FileManager fileManager = new FileManager(editor);
        DirectoryManager directoryManager = new DirectoryManager();
        TipsManager tipsManager = new TipsManager();

        //Environment.CommandLine     maybe this will work for passing current directory from the terminal?
        // use args to pass current directory from terminal

        tipsManager.PrintTipOfTheDay();
        Console.WriteLine($"Press 9 to display help. NUM LOCK: {Console.NumberLock}");
        while (true)
        {
            Console.Write(path + " ");
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.D0 || key.Key == ConsoleKey.NumPad0)
            {
                // exit application
                Console.ResetColor();
                return;
            }
            else if (key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.NumPad1)
            {
                // like 'touch' in ubuntu
                fileManager.CreateFile(path, txt);
            }
            else if (key.Key == ConsoleKey.D2 || key.Key == ConsoleKey.NumPad2)
            {
                // clears the console
                consoleManager.ClearConsole();
            }
            else if (key.Key == ConsoleKey.D3 || key.Key == ConsoleKey.NumPad3)
            {
                // ls
                Console.WriteLine("\n\n//----------------Directories----------------//");
                IEnumerable<string> dirs = Directory.EnumerateDirectories(path);
                foreach (var d in dirs) Console.WriteLine(d);
                Console.WriteLine("//-------------------Files-------------------//");
                IEnumerable<string> files = Directory.EnumerateFiles(path);
                foreach (var f in files) if (f.Substring(f.Length - 4) == txt) Console.WriteLine(f);
                Console.WriteLine("\n\n");
            }
            else if (key.Key == ConsoleKey.D4 || key.Key == ConsoleKey.NumPad4)
            {
                // partially working
                consoleManager.ToggleFullScreen();
            }
            else if (key.Key == ConsoleKey.D5 || key.Key == ConsoleKey.NumPad5)
            {
                path = directoryManager.ChangeDirectory(path);
            }
            else if (key.Key == ConsoleKey.D6 || key.Key == ConsoleKey.NumPad6)
            {
                // TODO: calculator
            }
            else if (key.Key == ConsoleKey.D7 || key.Key == ConsoleKey.NumPad7)
            {
                fileManager.DeleteFile(path, txt);
            }
            else if (key.Key == ConsoleKey.D8 || key.Key == ConsoleKey.NumPad8)
            {
                // switches between dark/white themes
                consoleManager.ToggleTheme();
            }
            else if (key.Key == ConsoleKey.D9 || key.Key == ConsoleKey.NumPad9)
            {
                // display help
                Console.WriteLine($"\nNUM LOCK: {Console.NumberLock} {HelpText}");
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                // show current numlock
                Console.WriteLine($"NUM LOCK: {Console.NumberLock}");
            }
            //switch (key.Key)
            //{
            //    case ConsoleKey.NumPad0:
            //        // exit application
            //        Console.ResetColor();
            //        return;

            //    case ConsoleKey.NumPad1:
            //        // like 'touch' in ubuntu

            //        //Console.Write("filename:  ");
            //        //string? fileName = Console.ReadLine();
            //        //if (fileName == "") break;

            //        //// refactor this to FileMode.OpenOrCreate   
            //        //if (!File.Exists(Path.Combine(path, fileName) + txt))
            //        //{
            //        //    using (FileStream fs = File.Create(Path.Combine(path, fileName) + txt));
            //        //}
            //        //else if (File.Exists(Path.Combine(path, fileName) + txt))
            //        //{
            //        //    FileStream fs = File.Open(Path.Combine(path, fileName) + txt, FileMode.Open, FileAccess.ReadWrite);
            //        //    editor.OpenEditor(fs); // CURRENTLY NOT CASE-SENSITIVE
            //        //    fs.Close();
            //        //}
            //        fileManager.CreateFile(path, txt);

            //        break;

            //    case ConsoleKey.NumPad2:
            //        // clears the console
            //        consoleManager.ClearConsole();
            //        break;

            //    case ConsoleKey.NumPad3:
            //        // ls
            //        Console.WriteLine("\n\n//----------------Directories----------------//");
            //        IEnumerable<string> dirs = Directory.EnumerateDirectories(path);
            //        foreach (var d in dirs) Console.WriteLine(d);
            //        Console.WriteLine("//-------------------Files-------------------//");
            //        IEnumerable<string> files = Directory.EnumerateFiles(path);
            //        foreach (var f in files) if (f.Substring(f.Length - 4) == txt) Console.WriteLine(f);
            //        Console.WriteLine("\n\n");
            //        break;

            //    case ConsoleKey.NumPad4:
            //        // partially working
            //        // toggle fullscreen
            //        consoleManager.ToggleFullScreen();
            //        break;

            //    case ConsoleKey.NumPad5:
            //        // change directory
            //        path = directoryManager.ChangeDirectory(path);
            //        break;

            //    case ConsoleKey.NumPad6:
            //        // TODO opening calculator
            //        break;

            //    case ConsoleKey.NumPad7:
            //        //delete file
            //        fileManager.DeleteFile(path, txt);
            //        break;

            //    // toggle between light and dark theme
            //    case ConsoleKey.NumPad8:
            //        consoleManager.ToggleTheme();      
            //        break;

            //    case ConsoleKey.NumPad9:
            //        // display help
            //        Console.WriteLine($"\nNUM LOCK: {Console.NumberLock} {HelpText}");
            //        break;

            //    case ConsoleKey.Backspace:
            //        // show current numlock
            //        Console.WriteLine($"NUM LOCK: {Console.NumberLock}");
            //        break;

            //    default:
            //        break;
            //}
        }
    }
}