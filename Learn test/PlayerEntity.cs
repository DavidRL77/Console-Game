using System;
using System.Collections.Generic;
using System.Text;

namespace Learn_test
{
    public class PlayerEntity : Entity
    {
        private Vector2 Input;

        public PlayerEntity(Vector2 position) : base(position, '#', ConsoleColor.White)
        {
        }

        public override void Simulate(Simulation simulation)
        {

            ConsoleKeyInfo key = SuperConsole.ReadKeyInstant(true);

            switch(key.Key)
            {
                case ConsoleKey.UpArrow:
                    Input = Vector2.Up;
                    break;
                case ConsoleKey.DownArrow:
                    Input = Vector2.Down;
                    break;
                case ConsoleKey.LeftArrow:
                    Input = Vector2.Left;
                    break;
                case ConsoleKey.RightArrow:
                    Input = Vector2.Right;
                    break;
                default:
                    Input = Vector2.Zero;
                    break;
            }

            if(Move(Input) && !simulation.IsValidPosition(position))
            {
                Move(-Input);
            }

            if(simulation.GetTileAt(position).tileType == Tile.TileType.Goal)
            {
                simulation.Win();
            }
        }
    }
}
