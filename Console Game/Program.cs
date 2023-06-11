using System;
using System.Collections.Generic;
using System.IO;

//The unicode for a full square is "\u2588"
namespace ConsoleGame
{
    class Program
    {
        public static string worldsDir = "C:\\Worlds";
        private static Simulation currentSimulation;

        static void Main(string[] args)
        {
            SuperConsole.KeyActions.Add(ConsoleKey.Escape, EscPressed);

            //Setup
            AdjustWindow();
            WindowUtility.DisableResize();
            Console.ForegroundColor = ConsoleColor.White;
            Directory.CreateDirectory(worldsDir);
            
            bool exit = false;
            while(!exit)
            {

                DirectoryInfo dirInfo = new DirectoryInfo(worldsDir);
                DirectoryInfo[] directories = dirInfo.GetDirectories();

                if(directories.Length == 0)
                {
                    Console.WriteLine("No worlds");
                    SuperConsole.ReadKey(true);
                    return;
                }

                currentSimulation = null;

                AdjustWindow();

                Console.Clear();

                DirectoryInfo worldDir = GetUserChoice(directories, d => d.Name, "Choose a world");

                Console.Clear();
                Console.WriteLine("Loading...");
                WorldData worldData = new WorldData(worldDir.FullName);
                currentSimulation = new Simulation(worldData);
                currentSimulation.Run();
                worldData.Dispose();
            }
        }

        private static void AdjustWindow()
        {
            Console.CursorVisible = true;
            Console.WindowTop = 0;
            Console.SetWindowSize(120, 30);
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            WindowUtility.MoveWindowToCenter();
        }

        private static void EscPressed(ConsoleKey key)
        {
            if(currentSimulation != null) currentSimulation.End();
            else Environment.Exit(0);
        }

        public static T GetUserChoice<T>(T[] values, Func<T, string> toString, string topMessage = "Choose an option:")
        {
            bool chose = false;
            string errorMessage = "";
            while(!chose)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;

                int i = 0;
                foreach(T value in values)
                {
                    Console.WriteLine(i + ".- " + toString.Invoke(value));
                    i++;
                }

                Console.WriteLine();

                if(errorMessage != "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(topMessage);
                string answer = SuperConsole.ReadLine();

                if(Int32.TryParse(answer, out int index))
                {
                    if(index >= values.Length || index < 0)
                    {
                        errorMessage = "Item not in list";
                        continue;
                    }
                    else
                    {
                        chose = true;
                        return values[index];
                    }
                }
                else errorMessage = "Please enter a number";
            }
            return default(T);
        }

        public static T GetUserChoice<T>(T[] values, string topMessage = "Choose an option:")
        {
            return GetUserChoice(values, v => v.ToString(), topMessage);
        }
    }
}
