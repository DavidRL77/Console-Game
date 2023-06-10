using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ConsoleGame
{
    public class Simulation
    {
       public WorldData WorldData { get; private set; }

        public int Time { get; private set; }

        public Random rnd { get; private set; } = new Random();
        private bool running;
        private long lastTime = 0;

        public Simulation(WorldData worldData)
        {
            Time = 0;
            Console.CursorVisible = false;
            WorldData = worldData;
        }

        public void Draw()
        {
            Console.SetWindowSize(Math.Max(WorldData.WorldWidth, 15), WorldData.WorldHeight+1);
            Console.SetBufferSize(Console.WindowWidth, WorldData.WorldHeight + 10);

            //Draws the tiles
            if(WorldData.TilesChanged)
            {
                SuperConsole.ResetBuffer();
                Console.Clear();
                for(int y = 0; y < WorldData.Tiles.GetLength(1); y++)
                {
                    for(int x = 0; x < WorldData.Tiles.GetLength(0); x++)
                    {
                        Tile tile = WorldData.Tiles[x, y];
                        SuperConsole.WriteAt(tile.displayChar, x, y, tile.tileColor);
                    }

                }
                WorldData.TilesChanged = false;
            }

            DrawEntities();

            Console.SetCursorPosition(0, WorldData.Tiles.GetLength(1));
            Console.Write("Time: " + Time);
        }

        private void InitializeEntities()
        {
            for(int i = 0; i < WorldData.Entities.Count; i++)
            {
                WorldData.Entities[i].Awake(this);
            }
        }

        private void DrawEntities()
        {
            for(int i = 0; i < WorldData.Entities.Count; i++)
            {
                WorldData.Entities[i].Draw();
            }
        }

        private void SimulateEntities()
        {
            for(int i = 0; i < WorldData.Entities.Count; i++)
            {
                WorldData.Entities[i].Simulate(this);
            }
        }

        public void Run()
        {
            Console.ResetColor();
            InitializeEntities();

            Draw();
            WindowUtility.MoveWindowToCenter();

            #region Movement
            running = true;
            while(running)
            {
                long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                long difference = currentTime - lastTime;
                if(difference < 100)
                {
                    Thread.Sleep(100 - (int)difference);
                }
                lastTime = currentTime;

                SimulateEntities();
                Draw();
                Time++;
            }
            #endregion
        }

        public void End()
        {
            running = false;
        }

        public void Win()
        {
            Console.Clear();
            Console.WriteLine("You win!");
            SuperConsole.ReadKeyInstant(true);
            running = false;
        }

        public bool IsValidPosition(Vector2 pos)
        {
            return IsValidPosition(pos.x, pos.y);
        }

        public bool IsValidPosition(int x, int y)
        {
            if(GetTile(x,y) == null || GetTile(x,y).tileType == Tile.TileType.Wall) return false;
            else return true;
        }

        public Tile GetTile(Vector2 pos)
        {
            return GetTile(pos.x, pos.y);
        }

        public Tile GetTile(int x, int y)
        {
            return WorldData.GetTile(x, y);
        }
    }
}
