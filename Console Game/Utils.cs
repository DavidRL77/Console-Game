using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ConsoleGame
{
    public static class Utils
    {

        public static Random rnd = new Random();
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
                    arr[i, j] = value;
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

        public static string Escaped(this string input)
        {

            string[] specialCharacters = { "\\", "\"", "\b", "\f", "\n", "\r", "\t" };
            string[] escapedCharacters = { "\\\\", "\\\"", "\\b", "\\f", "\\n", "\\r", "\\t" };

            for(int i = 0; i < specialCharacters.Length; i++)
            {
                input = input.Replace(specialCharacters[i], escapedCharacters[i]);
            }

            return input;

        }

        //Thanks chatgpt
        public static string GetJsonTip(JsonReaderException ex)
        {
            string tip = "";

            // Check if the exception message indicates an invalid character
            if(ex.Message.Contains("Invalid character"))
            {
                tip = "Check for any special characters or invalid characters in your JSON data.";
            }
            // Check if the exception message indicates a missing closing delimiter
            else if(ex.Message.Contains("Unexpected end when reading JSON"))
            {
                tip = "Make sure all opening brackets, braces, and quotes have corresponding closing brackets, braces, and quotes.";
            }
            // Check if the exception message indicates an invalid property name
            else if(ex.Message.Contains("Unexpected character encountered while parsing value"))
            {
                tip = "Ensure that your property names are enclosed in double quotes and that the value for the property is valid.";
            }
            // Check if the exception message indicates a duplicate property name
            else if(ex.Message.Contains("A duplicate key property was found in the JSON object"))
            {
                tip = "Ensure that your JSON object doesn't contain duplicate property names.";
            }
            // Check if the exception message indicates an unterminated string
            else if(ex.Message.Contains("Unterminated string"))
            {
                tip = "Make sure all strings in your JSON data are properly enclosed in double quotes and terminated.";
            }
            // Check if the exception message indicates a value outside the root object or array
            else if(ex.Message.Contains("Additional text encountered after finished reading JSON content"))
            {
                tip = "Check if there is any additional content (e.g., extra characters, invalid syntax) outside the root object or array in your JSON data.";
            }
            // Add more specific checks for other common JsonReaderException scenarios if needed

            // If none of the specific checks match, provide a generic tip
            if(string.IsNullOrEmpty(tip))
            {
                tip = "Make sure to check all parentheses, commas, quotes and brackets";
            }

            return tip;
        }

        public static T RandomElement<T>(this ICollection<T> collection)
        {
            int randIndex = rnd.Next(0, collection.Count);

            int i = 0;
            foreach(T item in collection)
            {
                if(i == randIndex) return item;
                i++;
            }
            return default(T);
        }
    }
}
