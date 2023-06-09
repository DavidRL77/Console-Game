using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleGame
{
    /*
    I'll write this here so I don't forget:
    The entity class has a list of components that can only be added through AddComponent.
    The reason is that the component itself is created dynamically through reflection. Why, you may ask?
    Well, because the type is not known at compile time, since all components are loaded from files.
    And the reason why it should *only* be done within the entity class, is because nothing else should have access to the component's "entity" property.
     */

    public class Entity
    {
        public Vector2 position { get; protected set; }
        private List<Component> components = new List<Component>();

        [JsonIgnore]
        protected Vector2 prevPos;

        public char display = '#';
        public ConsoleColor color;

        public Entity(char display, ConsoleColor color) : this(Vector2.Zero, display, color) { }

        public Entity(Vector2 position, char display, ConsoleColor color)
        {
            this.position = position;
            prevPos = position;
            this.display = display;
            this.color = color;
        }

        /// <summary>
        /// Sets the position if posible, else returns false
        /// </summary>
        /// <returns></returns>

        public bool SetPosition(int x, int y)
        {
            if(x >= Console.WindowWidth || x < 0 || y >= Console.WindowHeight || y < 0) return false;

            position = new Vector2(x, y);
            return true;
        }

        /// <summary>
        /// Sets the position if posible, else returns false
        /// </summary>
        /// <returns></returns>
        public bool SetPosition(Vector2 position)
        {
            return SetPosition(position.x, position.y);
        }

        public bool Move(int x, int y)
        {
            return SetPosition(position.x + x, position.y + y);
        }

        public bool Move(Vector2 move)
        {
            return SetPosition(position + move);
        }

        public virtual void Awake(Simulation simulation)
        {
            foreach(Component component in components)
            {
                if(!component.enabled) continue;
                component.Awake(simulation);
            }
        }

        public virtual void Simulate(Simulation simulation)
        {
            foreach(Component component in components)
            {
                if(!component.enabled) continue;
                component.Simulate(simulation);
            }
        }

        public virtual void Draw()
        {
            ConsoleColor prevColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.SetCursorPosition(position.x, position.y);
            Console.Write(display);

            if(!prevPos.Equals(position))
            {
                //Clears the previous character position
                ColoredChar coloredChar = SuperConsole.GetCharAt(prevPos);
                Console.SetCursorPosition(prevPos.x, prevPos.y);
                Console.ForegroundColor = coloredChar.color;
                Console.Write(coloredChar.character);

            }
            Console.SetCursorPosition(position.x, position.y);
            prevPos = position;
            Console.ForegroundColor = prevColor;

            foreach(Component component in components)
            {
                if(!component.enabled) continue;
                component.Draw();
            }
        }

        public T GetComponent<T>() where T : Component
        {
            foreach(Component component in components)
            {
                if(typeof(Component) == typeof(T)) return (T)component;
            }
            return null;
        }

        /// <summary>
        /// Adds a new component of the type T, with default parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AddComponent<T>() where T : Component
        {
            Type componentType = typeof(T);

            Component instance = (Component)Activator.CreateInstance(componentType);
            components.Add(instance);
        }

        /// <summary>
        /// Creates a new component of type <paramref name="componentType"/> and assigns all the parameters from <paramref name="component"/> (Since components shouldn't be created outside of the entity class)
        /// </summary>
        /// <param name="component"></param>
        /// <param name="componentType"></param>
        public void AddComponent(JObject component, Type componentType)
        {
            Component instance = (Component)Activator.CreateInstance(componentType, this);

            //Use reflection to set the properties of the component instance, since I can't cast it any other way
            PropertyInfo[] properties = componentType.GetProperties();
            foreach(PropertyInfo property in properties)
            {
                if(property.CanWrite)
                {
                    object value = component.GetValue(property.Name).ToObject(property.PropertyType);
                    property.SetValue(instance, value);
                }
            }

            //Fields too
            FieldInfo[] fields = componentType.GetFields();
            foreach(FieldInfo field in fields)
            {
                object value = component.GetValue(field.Name).ToObject(field.FieldType);
                field.SetValue(instance, value);
            }

            components.Add(instance);
        }
    }
}