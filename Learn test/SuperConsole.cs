using System;
using System.Collections.Generic;
using System.Text;

namespace Learn_test
{
    /// <summary>
    /// A way to write some shit babyyyy
    /// </summary>
    public static class SuperConsole
    {
        public static ColoredChar[,] buffer;

        static SuperConsole()
        {
            buffer = new ColoredChar[Console.BufferWidth, Console.BufferHeight];
        }

        public static void WriteAt(string text, Vector2 position, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(position.x, position.y);
            Console.Write(text);

            for(int i = 0; i < text.Length; i++)
            {
                buffer[position.x + i, position.y] = new ColoredChar(text[i], color);
            }

            Console.SetCursorPosition(position.x, position.y);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void WriteAt(string text, int x, int y, ConsoleColor color = ConsoleColor.White)
        {
            WriteAt(text, new Vector2(x, y), color);
        }

        public static void WriteAt(char c, Vector2 position, ConsoleColor color = ConsoleColor.White)
        {
            WriteAt(c.ToString(), position, color);
        }

        public static void WriteAt(char c, int x, int y, ConsoleColor color = ConsoleColor.White)
        {
            WriteAt(c, new Vector2(x, y), color);
        }

        public static void ResetBuffer()
        {
            buffer = new ColoredChar[Console.BufferWidth, Console.BufferHeight];
        }

        public static ColoredChar GetCharAt(Vector2 position)
        {
            return buffer[position.x, position.y];
        }
        public static ColoredChar GetCharAt(int x, int y)
        {
            return buffer[x, y];
        }
    }

    public class ColoredChar
    {
        public char character;
        public ConsoleColor color;

        public ColoredChar(char ch, ConsoleColor color)
        {
            this.character = ch;
            this.color = color;
        }

        public override string ToString()
        {
            return character.ToString();
        }

        public static implicit operator char(ColoredChar ch)
        {
            return ch.character;
        }
    }
}
