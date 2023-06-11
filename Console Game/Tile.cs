using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConsoleGame
{
    public class Tile
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum TileType
        {
            Wall,
            Empty,
            Goal
        }

        public char displayChar;
        public TileType tileType;
        public ConsoleColor tileColor;
        public string material;

        public Tile(char displayChar, TileType tileType, ConsoleColor tileColor, string material)
        {
            this.displayChar = displayChar;
            this.tileType = tileType;
            this.tileColor = tileColor;
            this.material = material;
        }

        public Tile(char displayChar, TileType tileType, string material) : this(displayChar, tileType, ConsoleColor.White, material)
        {
           
        }

        public Tile() : this(' ', TileType.Empty, ConsoleColor.White, "") { }
    }
}
