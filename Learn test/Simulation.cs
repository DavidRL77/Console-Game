using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Learn_test
{
    class Simulation
    {
        private Agent agent;
        private Random rnd = new Random();
        private int time;
        public int iterations;
        private Tile[,] tiles;
        private List<Vector2> positions = new List<Vector2>();
        private Dictionary<char, string> definitions = new Dictionary<char, string>()
        {
            {'#', "tiles:wall"},
            {'p', ""},
            {'g', new Definition("tile", new Tile('\u2588', Tile.TileType.Goal, ConsoleColor.Green))}
        };

        private bool running;

        public Simulation(string fileContents)
        {
            Extraction e = new Extraction();

            //Sets shit up
            iterations = 0;
            Console.CursorVisible = false;


            string[] splitFileContents = fileContents.Split('{');
            string world = splitFileContents[0];
            string definitionsJson = splitFileContents.Length > 1 ? splitFileContents[1] : null;
            

            string[] splitWorld = world.Split("\n");

            string longest = Utils.GetLongestString(splitWorld);
            if(longest.Length > Console.BufferWidth || splitWorld.Length > Console.BufferHeight)
            {
                throw new Exception("World is too big");
            }

            tiles = new Tile[Utils.GetLongestString(splitWorld).Length, splitWorld.Length];
            tiles.Populate(new Tile(' ', Tile.TileType.Empty));

            //Loops through the text file
            for(int y = 0; y < splitWorld.Length; y++)
            {
                string line = splitWorld[y].Trim();

                for(int x = 0; x < line.Length; x++)
                {
                    char c = line[x];

                    switch(c)
                    {
                        case 'p':
                            agent = new Agent(new Vector2(x, y));
                            break;
                        case '#':
                            tiles[x, y] = new Tile('\u2588', Tile.TileType.Wall);
                            break;
                        case 'g':
                            tiles[x, y] = new Tile('\u2588', Tile.TileType.Goal, ConsoleColor.Green);
                            break;
                        default:
                            tiles[x, y] = new Tile(' ', Tile.TileType.Empty);
                            break;
                    }
                }

            }

            Draw();
            WindowUtility.MoveWindowToCenter();
        }

        public void Draw()
        {
            Console.SetWindowSize(Math.Max(tiles.GetLength(0), 15), tiles.GetLength(1)+1);
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            SuperConsole.ResetBuffer();
            Console.Clear();

            //Draws the tiles
            for(int y = 0; y < tiles.GetLength(1); y++)
            {
                for(int x = 0; x < tiles.GetLength(0); x++)
                {
                    Tile tile = tiles[x, y];
                    SuperConsole.WriteAt(tile.displayChar, x, y, tile.tileColor);
                }

            }
            agent?.Update();

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("Time: " + time);
        }

        public void Run()
        {
            if(agent == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("No agent");
                Console.ReadLine();
                return;
            }
            Console.ResetColor();

            #region Movement
            running = true;
            while(running)
            {

                Console.SetCursorPosition(0,Console.WindowHeight-1);
                Console.Write("Time: " + time);

                positions.Add(agent.position);
                int x = 0;
                int y = 0;

                ConsoleKeyInfo key = Console.ReadKey();

                if(key.Key == ConsoleKey.UpArrow) y = -1;
                else if(key.Key == ConsoleKey.DownArrow) y = 1;
                else if(key.Key == ConsoleKey.LeftArrow) x = -1;
                else if(key.Key == ConsoleKey.RightArrow) x = 1;

                //When the agent moves into a wall
                if(agent.Move(x,y) && !IsValidPosition(agent.position.x, agent.position.y))
                {
                    Debug.WriteLine("Invalid pos");
                    agent.Move(-x, -y);
                }

                agent.Update();
                time++;

                if(tiles[agent.position.x, agent.position.y].tileType == Tile.TileType.Goal)
                {
                    End();
                }
            }
            #endregion

            #region AI
            //for(int i = 0; i < goals.Length; i++)
            //{
            //    Console.SetCursorPosition(0, 0);
            //    Console.Write(i);

            //    //Gets the closest goal
            //    Vector2 goal = goals.Where(g => g != null).OrderBy(g => (agent.position - g).magnitude).First();

            //    Vector2 distance = goal - agent.position;
            //    int signX = Math.Sign(distance.x);
            //    int signY = Math.Sign(distance.y);

            //    for(int x = 0; x < Math.Abs(distance.x); x++)
            //    {
            //        agent.Move(1 * signX, 0);
            //        agent.Update();
            //        Thread.Sleep(100);
            //    }

            //    for(int y = 0; y < Math.Abs(distance.y); y++)
            //    {
            //        agent.Move(0, 1 * signY);
            //        agent.Update();
            //        Thread.Sleep(100);
            //    }

            //    goals[Array.IndexOf(goals, goal)] = null;
            //    //Thread.Sleep(500);
            //}
            #endregion

        }

        public void End()
        {
            running = false;
            ReplaySteps();
            Console.Clear();
            Console.WriteLine("You win!");
            Console.ReadLine();
        }

        private void ReplaySteps()
        {
            Draw();
            for(int i = 0; i < positions.Count; i++)
            {
                agent.SetPosition(positions[i]);
                agent.Update();
                Thread.Sleep(100);

            }
        }

        public bool IsValidPosition(int x, int y)
        {
            if(x > Console.WindowWidth || x < 0 || y > Console.WindowHeight || y < 0 || tiles[x,y].tileType == Tile.TileType.Wall) return false;
            else return true;
        }
    }
}
