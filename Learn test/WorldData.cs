using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleGame
{
    public class WorldData
    {
        public static readonly Dictionary<string, object> DEFAULT_ASSETS = new Dictionary<string, object>()
        {
            //Tiles
            {"tiles/wall", new TileDefinition('\u2588', Tile.TileType.Wall, ConsoleColor.White) },
            {"tiles/empty", new TileDefinition(' ', Tile.TileType.Empty, ConsoleColor.White) },
            {"tiles/goal", new TileDefinition('\u2588', Tile.TileType.Goal, ConsoleColor.Green) },

            //Entities
            {"entities/player", new EntityDefinition('#', ConsoleColor.White, new List<ComponentDefinition>()
            {
                new ComponentDefinition("components/PlayerComponent", new PlayerComponent())
            }) },

            //World definition
            {"world", new WorldDefinition(new Dictionary<char, string>()
            {
                {'p', "entities/player"},
                {' ', "tiles/empty"},
                {'g', "tiles/goal"},
                {'#', "tiles/wall"},
            })}
        };

        public string WorldName { get; private set; }
        public string WorldFolder { get; private set; }
        public string WorldFile { get => Path.Combine(WorldFolder, "world.txt"); }
        public string AssetsFolder { get => Path.Combine(WorldFolder, "assets"); }
        public bool TilesChanged { get; set; }


        public int WorldWidth { get => Tiles.GetLength(0); }
        public int WorldHeight { get => Tiles.GetLength(1); }
        public List<Entity> Entities { get; private set; } = new List<Entity>();
        public Tile[,] Tiles { get; private set; }
        public AssetRegistry Registry { get; private set; }

        private WorldDefinition worldDefinition;

        public WorldData(string directory)
        {
            WorldFolder = directory;

            if(!File.Exists(WorldFile)) throw new FileNotFoundException("No world file found");
            string world = File.ReadAllText(WorldFile);

            Registry = new AssetRegistry();
            CreateDefaultAssets();
            RegisterAssets();

            worldDefinition = Registry.Get<WorldDefinition>("world");

            Init(Path.GetFileName(WorldFolder), world);
        }

        //Creates the default asset files, if they are not present
        private void CreateDefaultAssets()
        {
            foreach(KeyValuePair<string, object> asset in DEFAULT_ASSETS)
            {
                string folder = GetAssetFolder(asset.Key);
                string file = GetAssetFile(asset.Key);

                if(File.Exists(file)) continue;
                if(folder != null) Directory.CreateDirectory(folder);
                File.WriteAllText(file, JsonConvert.SerializeObject(asset.Value, Formatting.Indented));
            }
        }

        private void RegisterAssets()
        {
            RegisterComponents();
            RegisterSounds();

            RegisterAssetsFromFolder<EntityDefinition>(Path.Combine(AssetsFolder, "entities"));
            RegisterAssetsFromFolder<TileDefinition>(Path.Combine(AssetsFolder, "tiles"));


            string worldDefinitionJson = File.ReadAllText(Path.Combine(AssetsFolder, "world.json"));
            WorldDefinition worldDefinition = JsonConvert.DeserializeObject<WorldDefinition>(worldDefinitionJson);
            Registry.Register("world", worldDefinition);

        }

        private void RegisterComponents()
        {
            Registry.Register("components/PlayerComponent", typeof(PlayerComponent));
        }

        //As much as I hate this, I need to register sounds separately because I want to register the actual streams, not the definition
        private void RegisterSounds()
        {
            string path = Path.Combine(AssetsFolder, "sounds");
            foreach(string file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
            {
                SoundDefinition soundDefinition = JsonConvert.DeserializeObject<SoundDefinition>(File.ReadAllText(file));
                Registry.Register(GetRelativePath(file), soundDefinition.GetValue(this));
            }
            Registry.Register("sounds/none", null);
        }

        private void RegisterAssetsFromFolder<T>(string folder)
        {
            foreach(string file in Directory.GetFiles(folder, "*.json", SearchOption.AllDirectories))
            {
                Registry.Register(GetRelativePath(file), JsonConvert.DeserializeObject<T>(File.ReadAllText(file)));
            }
        }

        private void Init(string worldName, string world)
        {
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

                    if(worldDefinition.world.TryGetValue(c, out string path))
                    {
                        string folder = GetFolder(path);
                        switch(folder)
                        {
                            case "items":
                                break;
                            case "entities":
                                Entity entity = Registry.Get<EntityDefinition>(path).GetValue(this);
                                entity.SetPosition(x, y);
                                Entities.Add(entity);
                                break;
                            case "tiles":
                                Tile tile = Registry.Get<TileDefinition>(path).GetValue(this);
                                SetTile(x, y, tile);
                                break;
                            default:
                                SetTile(x, y, new Tile(' ', Tile.TileType.Empty));
                                break;
                        }
                    }
                    else SetTile(x, y, new Tile(' ', Tile.TileType.Empty));
                }

            }
        }

        public void SetTile(int x, int y, Tile tile)
        {
            Tiles[x, y] = tile;
            TilesChanged = true;
        }

        public Tile GetTile(int x, int y)
        {
            if(x < 0 || x > WorldWidth - 1 || y < 0 || y > WorldHeight - 1) return null;
            return Tiles[x, y];
        }

        public override string ToString()
        {
            return WorldName;
        }

        public string GetAssetFolder(string relativePath)
        {
            if(relativePath.Length == 0) return null;

            int index = relativePath.Replace('\\', '/').LastIndexOf('/');
            if(index < 0) return relativePath;
            string folder = relativePath.Substring(0, index);
            return Path.Combine(AssetsFolder, folder);
        }

        public string GetAssetFile(string relativePath)
        {
            string file = relativePath + ".json";
            return Path.Combine(AssetsFolder, file);
        }

        public string GetFolder(string relativePath)
        {
            int index = relativePath.IndexOf('/');
            if(index < 0) return relativePath;
            return relativePath.Substring(0, index);
        }

        private string GetRelativePath(string path)
        {
            //+7 to start *after* "assets\"
            int startIndex = path.LastIndexOf("assets") + 7;
            int endIndex = path.LastIndexOf(".");
            return path.Substring(startIndex, endIndex - startIndex).Replace("\\", "/");
        }

        public string GetFullPath(string relativePath)
        {
            return Path.Combine(AssetsFolder, relativePath);
        }
    }
}
