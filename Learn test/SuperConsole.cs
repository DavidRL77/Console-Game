using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Learn_test
{
    /// <summary>
    /// A way to write some shit babyyyy
    /// </summary>
    public static class SuperConsole
    {
        public static ColoredChar[,] buffer = new ColoredChar[Console.BufferWidth, Console.BufferHeight];
        public static Dictionary<ConsoleKey, Action<ConsoleKey>> KeyActions { get; private set; } = new Dictionary<ConsoleKey, Action<ConsoleKey>>();

        private static CancellationTokenSource backgroundReadCancellationTokenSource = new CancellationTokenSource();

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

        public static void ClearReadBuffer()
        {
            while(Console.KeyAvailable) Console.ReadKey(true);
        }

        public static ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey(false);
        }

        public static ConsoleKeyInfo ReadKey(bool intercept)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept);
            if(KeyActions.TryGetValue(keyInfo.Key, out Action<ConsoleKey> action))
            {
                action?.Invoke(keyInfo.Key);
            }
            return keyInfo;
        }

        /// <summary>
        /// Listens for a key, clearing the previous buffer
        /// </summary>
        /// <returns></returns>
        public static ConsoleKeyInfo ReadKeyInstant()
        {
            return ReadKeyInstant(false);
        }

        /// <summary>
        /// Listens for a key, clearing the previous buffer
        /// </summary>
        /// <returns></returns>
        public static ConsoleKeyInfo ReadKeyInstant(bool intercept)
        {
            while(Console.KeyAvailable) ReadKey(true);

            return ReadKey(intercept);
        }

        public static string ReadLine()
        {
            StringBuilder sb = new StringBuilder();
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = ReadKey(intercept: true);

                if(keyInfo.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    // Remove the last character from the StringBuilder
                    sb.Length--;
                    Console.Write("\b \b"); // Erase the character on the console
                }
                else if(keyInfo.Key != ConsoleKey.Enter)
                {
                    // Append the pressed key to the StringBuilder
                    sb.Append(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                }
            } while(keyInfo.Key != ConsoleKey.Enter);

            Console.WriteLine(); // Move to the next line after Enter key press

            return sb.ToString();
        }

        /// <summary>
        /// Reads a line of text, clearing the previous buffer
        /// </summary>
        /// <returns></returns>
        public static string ReadLineInstant()
        {
            ClearReadBuffer();

            return ReadLine();
        }

        public static void StartBackgroundRead()
        {
            backgroundReadCancellationTokenSource.Cancel();
            backgroundReadCancellationTokenSource = new CancellationTokenSource();
            Thread bgRead = new Thread(() => BackgroundRead(backgroundReadCancellationTokenSource.Token));
            bgRead.Start();
        }

        public static void EndBackgroundRead()
        {
            backgroundReadCancellationTokenSource.Cancel();
        }

        private static void BackgroundRead(CancellationToken token)
        {
            while(!token.IsCancellationRequested)
            {
                if(Console.KeyAvailable) {ReadKey(true); }
                Thread.Sleep(200);
            }
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
