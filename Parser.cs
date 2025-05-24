using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CTTE
{
    public class Parser
    {
        private const string numberPattern = @"[0-9]+\.?[0-9]*";

        readonly Stack<double> numberStack = new();

        readonly Stack<string> operatorStack = new();

        double register = 0;

        int? popOpsCnt = null;

        public Parser() { }

        // very bugged
        public double ParseTokens(List<string> tokens)
        {
            register = 0;
            bool execFlag = false;
            foreach (var token in tokens)
            {
                if (Regex.Match(token, numberPattern).Success)
                {
                    numberStack.Push(Double.Parse(token));
                    if (popOpsCnt == null && execFlag)
                    {
                        register = numberStack.Pop();
                        if (operatorStack.Pop() == "*")
                        {
                            register *= numberStack.Pop();
                        }
                        numberStack.Push(register);
                        execFlag = false;
                    }
                    if (popOpsCnt != null)
                    {
                        popOpsCnt += 1;
                    }
                }
                if (token == "(")
                {
                    popOpsCnt = 0;
                }
                if (token == ")")
                {
                    while (popOpsCnt != 0)
                    {
                        register += numberStack.Pop();
                        popOpsCnt -= 1;


                        if (operatorStack.Peek() == "+")
                        {
                            operatorStack.Pop();
                            register += numberStack.Pop();
                            popOpsCnt -= 1;
                        }

                    }
                    numberStack.Push(register);
                    register = 0;
                    popOpsCnt = null;
                }

                if (token == "+" || token == "-")
                {
                    operatorStack.Push(token);
                }
                if (token == "*" || token == "/")
                {
                    operatorStack.Push(token);
                    execFlag = true;
                }
            }

            while (operatorStack.Count != 0)
            {
                if (operatorStack.Peek() == "+")
                {
                    operatorStack.Pop();
                    register = numberStack.Pop();
                    register += numberStack.Pop();
                    numberStack.Push(register);
                }
                else if (operatorStack.Peek() == "-")
                {
                    operatorStack.Pop();
                    register = numberStack.Pop();
                    register = numberStack.Pop() - register;
                    numberStack.Push(register);
                }
                else if (operatorStack.Peek() == "*")
                {
                    operatorStack.Pop();
                    register = numberStack.Pop();
                    register *= numberStack.Pop();
                    numberStack.Push(register);
                }
                else if (operatorStack.Peek() == "/")
                {
                    operatorStack.Pop();
                    register = numberStack.Pop();
                    register *= numberStack.Pop() / register;
                    numberStack.Push(register);
                }

            }

            register = numberStack.Pop();

            return register;
        }
    }
}
