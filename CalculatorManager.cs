using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CTTE
{
    public class CalculatorManager
    {

        const string operationsInfo = "n: is negative\n" +
            "p: is positive\n" +
            "f: factorial\n" +
            "c: cube\n" +
            "q: sqrt\n" +
            "s: square\n" +
            "w: insert in the text and exit\n" +
            "x: exit";
        Lex _lex { get; set; }
        Parser _parser { get; set; }

        public CalculatorManager(Lex lex, Parser parser)
        {
            _lex = lex;
            _parser = parser;
        }

        public char[] EnterExpression()
        {
            bool factorialSet = false;

            Console.Write("\nEnter expression: ");
            string expression = Console.ReadLine();

            List<string> tokens = _lex.GenerateTokens(expression);

            if (tokens.Count == 0)
            {
                Console.WriteLine("NOTHING TO PARSE: EXPRESSION IS EMPTY OR INCORRECT");
                return Array.Empty<char>();
            }

            double expressionResult = _parser.ParseTokens(tokens);

            SmartNumber smartNumber = new SmartNumber() { Value = expressionResult };
            smartNumber.Coroutine();


            Console.WriteLine(smartNumber.Value);

            Console.WriteLine(operationsInfo);
            while (true)
            {
                var cki = Console.ReadKey();

                switch (cki.Key)
                {
                    case ConsoleKey.N:
                        Console.WriteLine($"IsNegative: {smartNumber.IsNegative}");
                        break;
                    case ConsoleKey.P:
                        Console.WriteLine($"IsPositive: {smartNumber.IsPositive}");
                        break;
                    case ConsoleKey.F:
                        if (factorialSet)
                        {
                            smartNumber.Value = expressionResult;
                            factorialSet = !factorialSet;
                        }
                        else
                        {
                            smartNumber.CalculateFactorial();
                            Console.WriteLine($"Factorial: {smartNumber.Factorial}");
                            smartNumber.Value = smartNumber.Factorial;
                            factorialSet = !factorialSet;
                        }
                        break;
                    case ConsoleKey.Q:
                        Console.WriteLine($"Sqrt: {smartNumber.Sqrt}");
                        break;
                    case ConsoleKey.S:
                        Console.WriteLine($"Square: {smartNumber.Square}");
                        break;
                    case ConsoleKey.C:
                        Console.WriteLine($"Cube: {smartNumber.Cube}");
                        break;
                    case ConsoleKey.W:
                        string num = Convert.ToString(smartNumber.Value);
                        char[] digits = new char[num.Length];
                        for (int i = 0; i < num.Length; i++)
                        {
                            digits[i] = Convert.ToChar(num[i]);
                        }
                        return digits;
                    case ConsoleKey.X:
                        return Array.Empty<char>();
                    //break;
                    default:
                        break;
                }
            }
        }
    }
}
