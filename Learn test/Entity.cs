using System;
using System.Collections.Generic;

namespace Learn_test
{
    public class Entity
    {
        public bool StorePositions = true;

        public Vector2 position { get; protected set; }
        protected Vector2 prevPos;
        public List<Vector2> positions = new List<Vector2>();

        public char display { get; protected set; } = '#';
        public ConsoleColor color { get; protected set; }

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

        public virtual void Simulate(Simulation simulation)
        {

        }

        public virtual void Draw()
        {
            ConsoleColor prevColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.SetCursorPosition(position.x, position.y);
            Console.Write(display);
            if(StorePositions) positions.Add(position);

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

        }
    }
}
