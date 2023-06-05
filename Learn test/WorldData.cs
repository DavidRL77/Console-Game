using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn_test
{
    public class WorldData
    {
        public string WorldName { get; private set; }

        public int WorldWidth { get => Tiles.GetLength(0); }
        public int WorldHeight { get => Tiles.GetLength(1); }
        public List<Entity> Entities { get; private set; } = new List<Entity>();
        public Tile[,] Tiles { get; private set; }

        private WorldSettings worldSettings;

        public WorldData(string directory)
        {
            string worldFilePath = Path.Combine(directory, "world.txt");
            if(!File.Exists(worldFilePath)) throw new FileNotFoundException("No world file found");

            string world = File.ReadAllText(worldFilePath);

            string worldSettingsFilePath = Path.Combine(directory, "settings.json");
            string settings = null;
            if(File.Exists(worldSettingsFilePath))
            {
                settings = File.ReadAllText(worldSettingsFilePath);
            }

            Init(Path.GetDirectoryName(directory), world, settings);
        }

        private void Init(string worldName, string world, string settings)
        {
            if(settings == null) worldSettings = WorldSettings.DEFAULT;
            else worldSettings = JsonConvert.DeserializeObject<WorldSettings>(settings); 

            this.WorldName = worldName;
            string[] splitWorld = world.Split('\n');

            string longest = Utils.GetLongestString(splitWorld);
            if(longest.Length > Console.BufferWidth || splitWorld.Length > Console.BufferHeight)
            {
                throw new Exception("World is too big");
            }

            Tiles = new Tile[Utils.GetLongestString(splitWorld).Length, splitWorld.Length];
            Tiles.Populate(new Tile(' ', Tile.TileType.Empty));

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
                            Entities.Add(new PlayerEntity(new Vector2(x, y)));
                            break;
                        case '#':
                            Tiles[x, y] = new Tile('\u2588', Tile.TileType.Wall);
                            break;
                        case 'g':
                            Tiles[x, y] = new Tile('\u2588', Tile.TileType.Goal, ConsoleColor.Green);
                            break;
                        default:
                            Tiles[x, y] = new Tile(' ', Tile.TileType.Empty);
                            break;
                    }
                }

            }
        }

        public override string ToString()
        {
            return WorldName;
        }
    }

    public class WorldSettings
    {
        public static readonly WorldSettings DEFAULT = new WorldSettings();
    }
}
