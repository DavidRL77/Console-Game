using NAudio.Wave;
using Newtonsoft.Json.Converters;
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
                entity.AddComponent(componentDefinition.component, type);
            }
            return entity;
        }
    }

    public struct ComponentDefinition : IDefinition<ComponentDefinition>
    {
        public string componentPath; //The path to the *type* of component
        public object component; //The value of the component, later "cast" into the type

        public ComponentDefinition(string componentPath, object component)
        {
            this.componentPath = componentPath;
            this.component = component;
        }

        public ComponentDefinition GetValue(WorldData worldData)
        {
            return this;
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
        public bool looping;

        public SoundDefinition(string filePath, bool looping)
        {
            this.filePath = filePath;
            this.looping = looping;
        }

        public WaveStream GetValue(WorldData worldData)
        {
            bool isRelative = !Path.IsPathFullyQualified(filePath);
            string fullPath = isRelative ? worldData.GetFullPath(filePath) : filePath;

            return new AudioFileReader(fullPath);
        }
    }
}
