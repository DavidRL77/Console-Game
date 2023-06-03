using System;
using System.Collections.Generic;
using System.Text;

namespace Learn_test
{
    public class Extraction
    {
        public int index = 0;

        public Extraction() { }

        /// <summary>
        /// Starting from 'index', it extracts the next number
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int ExtractNextNumber(string text)
        {
            if(index >= text.Length) return -1;
            return ExtractNumber(text, index + 1);
        }

        /// <summary>
        /// Extracts the first number found from a string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public int ExtractNumber(string text, int startIndex = 0)
        {
            StringBuilder num = new StringBuilder();
            index = startIndex;

            //Loops through the string piecing together the numbers
            for(int i = startIndex; i < text.Length; i++)
            {
                //If the character is a number, add it, otherwise, if we already have a number extracted, break from the loop
                char c = text[i];
                if(Char.IsNumber(c))
                {
                    num.Append(c);
                }
                else if(num.Length > 0) break;

                index = i;
            }
            //Returns the number, and if there isn't one, return -1
            string res;
            if(num.Length > 0) res = num.ToString();
            else res = "-1";

            return Convert.ToInt32(res);
        }

        /// <summary>
        /// Extracts an amount of numbers specified
        /// </summary>
        /// <param name="text"></param>
        /// <param name="amount"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public int[] ExtractNumbers(string text, int amount, int startIndex = 0)
        {
            int[] numbers = new int[amount];
            int numsExtracted = 0;

            for(int i = startIndex; i < text.Length && numsExtracted < amount; i++)
            {
                int num = ExtractNextNumber(text);
                if(num != -1)
                {
                    numbers[numsExtracted] = num;
                    numsExtracted++;
                }
            }

            return numbers;
        }

        /// <summary>
        /// Extracts the letters until a certain character is met
        /// </summary>
        /// <param name="text"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public string ExtractUntil(string text, char end, int startIndex = 0)
        {
            StringBuilder res = new StringBuilder();
            index = startIndex;

            for(int i = startIndex; i < text.Length; i++)
            {
                char c = text[i];
                index = i;

                if(c != end) res.Append(c);
                else break;
            }
            //Console.WriteLine(index + ": " + res);

            return res.ToString();
        }

        /// <summary>
        /// Same as extract until, but repeats it
        /// </summary>
        /// <param name="text"></param>
        /// <param name="end"></param>
        /// <param name="repeat"></param>
        /// <returns></returns>
        public string[] ExtractUntil(string text, char end, int repeat, int startIndex = 0)
        {
            string[] extractions = new string[repeat];
            index = startIndex - 1;

            for(int i = 0; i < repeat; i++)
            {
                extractions[i] = ExtractUntil(text, end, index + 1);
            }

            return extractions;
        }

    }
}
