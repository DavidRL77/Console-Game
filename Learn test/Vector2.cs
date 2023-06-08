using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Learn_test
{
    public class Vector2
    {
        public int x;
        public int y;

        [JsonIgnore]
        public double magnitude
        {
            get { return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)); }
        }

        [JsonIgnore]
        public double sqrMagnitude
        {
            get { return Math.Pow(x, 2) + Math.Pow(y, 2); }
        }

        //Default value of zero
        public Vector2()
        {
            x = 0;
            y = 0;
        }

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// 0,0
        /// </summary>
        public static readonly Vector2 Zero = new Vector2(0, 0);
        /// <summary>
        /// 1,1
        /// </summary>
        public static readonly Vector2 One = new Vector2(1, 1);
        /// <summary>
        /// 0,1
        /// </summary>
        public static readonly Vector2 Right = new Vector2(1, 0);
        /// <summary>
        /// 0,-1
        /// </summary>
        public static readonly Vector2 Left = new Vector2(-1, 0);
        /// <summary>
        /// 1,0
        /// </summary>
        public static readonly Vector2 Up = new Vector2(0, -1);
        /// <summary>
        /// -1,0
        /// </summary>
        public static readonly Vector2 Down = new Vector2(0, 1);

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(-a.x, -a.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }
        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x / b.x, a.y / b.y);
        }
        public static Vector2 operator *(Vector2 a, int b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public static Vector2 operator /(Vector2 a, int b)
        {
            return new Vector2(a.x / b, a.y / b);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2 vector &&
                   x == vector.x &&
                   y == vector.y;
        }
    }
}
