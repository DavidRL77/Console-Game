using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

//The unicode for a full square is "\u2588"
namespace Learn_test
{
    class Program
    {
        public static string worldsDir = "C:\\Worlds";
        private static Simulation currentSimulation;

        static void Main(string[] args)
        {
            SuperConsole.KeyActions.Add(ConsoleKey.Escape, EscPressed);

            //Setup
            WindowUtility.DisableResize();
            Console.ForegroundColor = ConsoleColor.White;
            Directory.CreateDirectory(worldsDir);
            DirectoryInfo dirInfo = new DirectoryInfo(worldsDir);
            DirectoryInfo[] directories = dirInfo.GetDirectories();

            if(directories.Length == 0)
            {
                Console.WriteLine("No worlds");
                SuperConsole.ReadKey(true);
                return;
            }

            //Loads the worlds
            string errorMessage = "";
            List<WorldData> worlds = new List<WorldData>();
            foreach(DirectoryInfo dir in directories)
            {
                try
                {
                    worlds.Add(new WorldData(dir.FullName));
                }
                catch(Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }

            }

            bool exit = false;
            while(!exit)
            {
                currentSimulation = null;

                //Adjusts window
                Console.CursorVisible = true;
                Console.WindowTop = 0;
                Console.SetWindowSize(120, 30);
                Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
                WindowUtility.MoveWindowToCenter();

                Console.Clear();

                WorldData world = GetUserChoice(worlds.ToArray(), "Choose a world");
                currentSimulation = new Simulation(world);
                currentSimulation.Run();
            }
        }

        private static void EscPressed(ConsoleKey key)
        {
            if(currentSimulation != null) currentSimulation.End();
            else Environment.Exit(0);
        }

        private static T GetUserChoice<T>(T[] values, string topMessage = "Choose an option:")
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
                    Console.WriteLine(i+".- " + value.ToString());
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
    }
}
