using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace Learn_test
{
    public class Entity
    {
        public Vector2 position { get; protected set; }
        private List<Component> components;

        [JsonIgnore]
        protected Vector2 prevPos;

        public char display = '#';
        public ConsoleColor color;



        public Entity(char display, ConsoleColor color, List<Component> components) : this(Vector2.Zero, display, color, components) { }

        public Entity(Vector2 position, char display, ConsoleColor color, List<Component> components)
        {
            this.position = position;
            prevPos = position;
            this.display = display;
            this.color = color;
            this.components = components;
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
                component.SetEntity(this);
                component.Awake(simulation);
            }
        }

        public virtual void Simulate(Simulation simulation)
        {
            foreach(Component component in components)
            {
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
    }
}
