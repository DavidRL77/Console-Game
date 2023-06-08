using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Learn_test
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

        public Tile(char displayChar, TileType tileType, ConsoleColor tileColor)
        {
            this.displayChar = displayChar;
            this.tileType = tileType;
            this.tileColor = tileColor;
        }

        public Tile(char displayChar, TileType tileType) : this(displayChar, tileType, ConsoleColor.White)
        {
           
        }

        public Tile() : this(' ', TileType.Empty, ConsoleColor.White) { }
    }
}
