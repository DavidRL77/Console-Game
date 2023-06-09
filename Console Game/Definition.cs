using NAudio.Wave;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace ConsoleGame
{
    //This contains all the definitions that are used in the json files

    public interface IDefinition<T>
    {
        T GetValue(WorldData worldData);
    }

    public struct TileDefinition : IDefinition<Tile>
    {
        public char displayChar;
        public Tile.TileType tileType;

        [JsonConverter(typeof(StringEnumConverter))]
        public ConsoleColor tileColor;

        public TileDefinition(char displayChar, Tile.TileType tileType, ConsoleColor tileColor)
        {
            this.displayChar = displayChar;
            this.tileType = tileType;
            this.tileColor = tileColor;
        }

        public Tile GetValue(WorldData worldData)
        {
            return new Tile(displayChar, tileType, tileColor);
        }
    }

    public struct EntityDefinition : IDefinition<Entity>
    {
        public char display;
        [JsonConverter(typeof(StringEnumConverter))]
        public ConsoleColor color;
        public List<ComponentDefinition> components;

        public EntityDefinition(char display, ConsoleColor color, List<ComponentDefinition> components)
        {
            this.display = display;
            this.color = color;
            this.components = components;
        }

        public Entity GetValue(WorldData worldData)
        {
            Entity entity = new Entity(display, color);

            foreach(ComponentDefinition componentDefinition in components)
            {
                Type type = worldData.Registry.Get<Type>(componentDefinition.componentPath);
                entity.AddComponent(componentDefinition.GetValue(worldData), type);
            }
            return entity;
        }
    }

    public struct ComponentDefinition : IDefinition<JObject>
    {
        public string componentPath; //The path to the *type* of component
        public JObject component; //The value of the component, later "cast" into the type

        public ComponentDefinition(string componentPath, JObject component)
        {
            this.componentPath = componentPath;
            this.component = component;
        }

        public JObject GetValue(WorldData worldData)
        {
            return component;
        }
    }

    //Yeah, i don't like this circular reference either
    public struct WorldDefinition : IDefinition<WorldDefinition>
    {
        public Dictionary<char, string> world; //The file character with its associated asset path

        public WorldDefinition(Dictionary<char, string> world)
        {
            this.world = world;
        }

        public WorldDefinition GetValue(WorldData worldData)
        {
            return this;
        }
    }

    public struct SoundDefinition : IDefinition<WaveStream>
    {
        public string filePath;
        public float volume = 1;

        [JsonIgnore]
        public List<WaveStream> streams { get; private set; } = null;

        public SoundDefinition(string filePath, float volume)
        {
            this.filePath = filePath;
            this.volume = volume;
        }

        public WaveStream GetValue(WorldData worldData)
        {
            bool isRelative = !Path.IsPathFullyQualified(filePath);
            string fullPath = isRelative ? worldData.GetFullPath(filePath) : filePath;

            if(streams == null)
            {
                streams = new List<WaveStream>();
                FileAttributes attributes = File.GetAttributes(fullPath);
                if(attributes.HasFlag(FileAttributes.Directory)) //Load the whole directory the path is pointing to
                {
                    foreach(string file in Directory.GetFiles(fullPath))
                    {
                        streams.Add(new AudioFileReader(file));
                    }
                }
                else streams.Add(new AudioFileReader(fullPath)); //Load the file the path is pointing to

                if(streams.Count == 0) throw new Exception("The directory " + fullPath + " is empty");
            }

            return streams.Count == 1 ? streams[0] : streams.RandomElement();
        }
    }
}
