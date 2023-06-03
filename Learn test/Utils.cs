using System;
using System.Collections.Generic;
using System.Text;

namespace Learn_test
{
    public static class Utils
    {
        public static void Populate<T>(this T[] arr, T value)
        {
            for(int i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
        }

        public static void Populate<T>(this T[,] arr, T value)
        {
            for(int i = 0; i < arr.GetLength(0); i++)
            {
                for(int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i,j] = value;
                }
            }
        }

        public static string GetLongestString(string[] array)
        {
            string longest = "";
            for(int i = 0; i < array.Length; i++)
            {
                if(array[i].Length > longest.Length) longest = array[i];
            }
            return longest;
        }
    }
}
