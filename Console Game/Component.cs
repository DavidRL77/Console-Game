using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace ConsoleGame
{
    public abstract class Component
    {
        [JsonIgnore]
        public Entity entity { get; private set; }

        public abstract void Awake(Simulation simulation);
        public abstract void Simulate(Simulation simulation);
        public abstract void Draw();

        public void SetEntity(Entity entity)
        {
            this.entity = entity;
        }

        protected T GetComponent<T>() where T : Component
        {
            return entity.GetComponent<T>();
        }
    }

    public class PlayerComponent : Component
    {
        public override void Awake(Simulation simulation)
        {
            
        }

        public override void Draw()
        {
            
        }

        public override void Simulate(Simulation simulation)
        {
            Vector2 Input = Vector2.Zero;
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

            if(entity.Move(Input) && !simulation.IsValidPosition(entity.position))
            {
                entity.Move(-Input);
            }

            if(simulation.GetTile(entity.position).tileType == Tile.TileType.Goal)
            {
                simulation.Win();
            }
        }
    }

    public class MoveSoundComponent : Component
    {
        public string sound;

        [JsonIgnore]
        private Vector2 prevPos;

        public override void Awake(Simulation simulation)
        {
            prevPos = entity.position;
        }

        public override void Draw()
        {
            
        }

        public override void Simulate(Simulation simulation)
        {
            if(prevPos != entity.position)
            {
                simulation.WorldData.AudioManager.Play("sfx", simulation.WorldData.Registry.Get<WaveStream>(sound));
            }
        }
    }
}
