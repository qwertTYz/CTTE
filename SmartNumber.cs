using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CTTE
{
    public class SmartNumber
    {
        public bool IsOdd { get; set; }
        public bool IsEven { get; set; }
        public bool IsNegative { get; set; }
        public bool IsNotNegative { get; set; }
        public bool IsPositive { get; set; }

        public bool IsPrime { get; set; }

        public double Value { get; set; }

        public double Sqrt { get; set; }
        public double Square { get; set; }
        public double Cube { get; set; }

        public double Factorial { get; set; }

        public SmartNumber() { }

        /// <summary>
        /// Sets all the basic number properties that don't require O(n) or O(n!) time.
        /// </summary>
        public void Coroutine()
        {
            IsOdd = Value % 2 != 0;
            IsEven = Value % 2 == 0;

            IsNegative = Value < 0;
            IsNotNegative = Value >= 0;
            IsPositive = Value > 0;

            Sqrt = Math.Sqrt(Value);
            Square = Math.Pow(Value, 2);
            Cube = Math.Pow(Value, 3);

        }

        public void CalculateFactorial()
        {
            double result = 1;
            for (double i = Value; i > 0; i--)
            {
                result *= i;
            }

            Factorial = result;
        }
    }
}
