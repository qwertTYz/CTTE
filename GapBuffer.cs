using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CTTE
{
    public class GapBuffer
    {
        readonly RegexOptions lastWordOptions = RegexOptions.RightToLeft;
        private const string lastWordPattern = @"[a-zA-Z0-9-.,;:/_!?]+";
        private const string nextWordPattern = @"[a-zA-Z0-9]*\s";

        public char[] array;
        public int left;
        public int right;
        private const int offset = 2;
        
        int height = 0;

        public GapBuffer()
        {
            left = 0;
            right = 8;
            array = new char[32];
        }

        public void Left()
        {
            if (left != 0)
            {
                left -= 1;

                array[right] = array[left];
                right -= 1;
                array[left] = '\0';
            }
            PrintBuffer();
        }

        public void Right()
        {
            if (left != right)
            {
                left += 1;
            }
            PrintBuffer();
        }
        public void CheckPointers()
        {
            if (right == array.Length && left == right)
            {
                GrowArray();
            }
            if (left == right)
            {
                GrowGap();
            }
        }

        public void Insert(char x)
        {
            if (right == array.Length && left == right)
            {
                GrowArray();
            }
            if (left == right)
            {
                GrowGap();
            }

            array[left] = x;
            left++;
            PrintBuffer();
        }

        public void Delete()
        {
            if (left != 0)
            {
                array[left - 1] = '\0';
                left -= 1;
            }
            else array[left] = '\0';

            PrintBuffer();
        }
        public void DeletePrevWord()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in array) stringBuilder.Append(c);  
            string text = stringBuilder.ToString();

            Match lastWordMatch = Regex.Match(text, lastWordPattern, lastWordOptions);

            if (lastWordMatch.Success)
            {
                for (int i = left; i != lastWordMatch.Index; i--) Delete();
            }
            PrintBuffer();
        }

        /// <summary>
        /// Ctrl + Delete
        /// </summary>
        public void DeleteNextWord()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in array) stringBuilder.Append(c);
               string text = stringBuilder.ToString();

            Match nextWordMatch = Regex.Match(text, nextWordPattern);
            int saveleft = left;

            if (nextWordMatch.Success)
            {
                for (int i = saveleft; i != saveleft + nextWordMatch.Length; i++)
                {
                    left++;
                }
            }
            PrintBuffer();
        }

        public void GrowGap()
        {
            char[] rightContent = new char[array.Length - right];
            int index = 0;
            for (int j = right; j < array.Length; j++)
            {
                rightContent[index] = array[j];
                index++;
            }
            index = 0;
            for (int i = right; i < right + 8; i++)
            {
                array[i] = '\0';
            }
            right += 8;
            for (int y = right; y < rightContent.Length; y++)
            {
                array[y] = rightContent[index];
            }
        }

        public void GrowArray()
        {
            int newLength = array.Length * 2;
            char[] biggerArray = new char[newLength];
            for (int i = 0; i < array.Length; i++)
            {
                biggerArray[i] = array[i];
            }

            array = biggerArray;
        }
        public void PrintBuffer()
        {
            Console.SetCursorPosition(0, 0);

            string leftPart = new string(array, 0, left);
            string rightPart = new string(array, right, array.Length - right);
            string text = leftPart + rightPart;

            Console.Write(text);

            int totalLength = array.Length;
            for (int i = text.Length; i < totalLength; i++)
            {
                Console.Write(' ');
            }


            if (left < Console.BufferWidth) Console.SetCursorPosition(left, height);
            else
            {
                Console.SetCursorPosition(0, height);
            }
        }
    }
}