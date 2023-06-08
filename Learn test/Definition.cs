using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Learn_test
{

    public interface IDefinition<T>
    {
        T GetValue(AssetRegistry registry);
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

        public Tile GetValue(AssetRegistry registry)
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

        public Entity GetValue(AssetRegistry registry)
        {
            return new Entity(display, color, components.Select(def => def.GetValue(registry)).ToList());
        }
    }

    public struct ComponentDefinition : IDefinition<Component>
    {
        public string componentPath;
        public object component;

        public ComponentDefinition(string componentPath, object component)
        {
            this.componentPath = componentPath;
            this.component = component;
        }

        public Component GetValue(AssetRegistry registry)
        {
            Type componentType = registry.Get<Type>(componentPath);
            Component instance = (Component)Activator.CreateInstance(componentType);

            //Use reflection to set the properties of the component instance, since I can't cast it any other way
            PropertyInfo[] properties = componentType.GetProperties();
            foreach(PropertyInfo property in properties)
            {
                if(property.CanWrite)
                {
                    object value = property.GetValue(component);
                    property.SetValue(instance, value);
                }
            }

            //Fields too
            FieldInfo[] fields = componentType.GetFields();
            foreach(FieldInfo field in fields)
            {
                object value = field.GetValue(component);
                field.SetValue(instance, value);
            }

            return instance;
        }
    }

    public struct WorldDefinition : IDefinition<Dictionary<char, string>>
    {
        public Dictionary<char, string> world;

        public WorldDefinition(Dictionary<char, string> world)
        {
            this.world = world;
        }

        public Dictionary<char, string> GetValue(AssetRegistry registry)
        {
            return world;
        }
    }
}
