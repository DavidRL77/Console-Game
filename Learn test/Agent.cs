using System;
using System.Collections.Generic;
using System.Text;

namespace Learn_test
{
    public class Agent
    {
        public Vector2 position { get; private set; }
        private Vector2 prevPos;
        private Vector2 defaultPosition;

        public Agent(Vector2 position)
        {
            this.position = position;
            prevPos = position;
            defaultPosition = position;
            Update();
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

        public void Update()
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.Write("#");


            if(!prevPos.Equals(position))
            {
                ConsoleColor prevColor = Console.ForegroundColor;

                //Clears the previous character position
                ColoredChar coloredChar = SuperConsole.GetCharAt(prevPos);
                Console.SetCursorPosition(prevPos.x, prevPos.y);
                Console.ForegroundColor = coloredChar.color;
                Console.Write(coloredChar.character);

                Console.ForegroundColor = prevColor;
            }
            Console.SetCursorPosition(position.x, position.y);
            prevPos = position;
        }

        public void Reset()
        {
            position = defaultPosition;
            Update();
        }
    }
}
