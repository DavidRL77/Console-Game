using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

//The unicode for a full square is "\u2588"
namespace Learn_test
{
    class Program
    {
        public static string worldsDir = "C:\\Worlds";

        private static Dictionary<char, Definition> definitions = new Dictionary<char, Definition>()
        {
            {'#', new Definition("tile", new Tile('\u2588', Tile.TileType.Wall))},
            {'p', new Definition("agent", new Agent(Vector2.Zero))},
            {'g', new Definition("tile", new Tile('\u2588', Tile.TileType.Goal, ConsoleColor.Green))}
        };

        static void Main(string[] args)
        {

            Debug.WriteLine(JsonConvert.SerializeObject(definitions, Formatting.Indented));

            WindowUtility.DisableResize();
            Console.ForegroundColor = ConsoleColor.White;
            Directory.CreateDirectory(worldsDir);
            DirectoryInfo dirInfo = new DirectoryInfo(worldsDir);
            FileInfo[] files = dirInfo.GetFiles();

            if(files.Length == 0)
            {
                Console.WriteLine("No worlds");
                Console.ReadLine();
                return;
            }
            
            string world = "";
            string errorMessage = "";
            bool exit = false;
            while(!exit)
            {
                Console.Clear();

                //Displays the worlds
                for(int i = 0; i < files.Length; i++)
                {
                    Console.WriteLine(i+".- " + files[i].Name);
                }

                Console.WriteLine();

                //Displays error message
                if(errorMessage != "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                    Console.ForegroundColor = ConsoleColor.White;

                    errorMessage = "";
                }

                Console.WriteLine("Select a world:");
                string answer = Console.ReadLine();
                if(answer == "exit") break;

                if(Int32.TryParse(answer, out int index))
                {
                    if(index < 0 || index >= files.Length)
                    {
                        errorMessage = "Number too big/small";
                        continue;
                    }
                    FileInfo file = files[index];
                    world = File.ReadAllText(file.FullName);

                    //Run simulation
                    Simulation s = new Simulation(world);
                    try
                    {
                        s.Run();
                    }
                    catch(Exception e)
                    {
                        errorMessage = e.Message;
                    }

                }
                else
                {
                    errorMessage = "Please input a number";
                }
            }
        }
    }
}
