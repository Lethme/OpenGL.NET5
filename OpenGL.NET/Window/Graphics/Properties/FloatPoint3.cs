using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.Window.Graphics.Properties
{
    public class FloatPoint3 : IEquatable<FloatPoint3>
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public static FloatPoint3 DefaultPoint => FloatPoint3.Create(0f, 0f, 0f);
        public FloatPoint3(float x = 0.0f, float y = 0.0f, float z = 0.0f)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public override bool Equals(object obj)
        {
            return obj is FloatPoint3 other && this.Equals(other);
        }
        public bool Equals(FloatPoint3 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }
        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }
        public static FloatPoint3 operator +(FloatPoint3 first, FloatPoint3 second)
        {
            return new FloatPoint3(first.X + second.X, first.Y + second.Y, first.Z + second.Z);
        }
        public static FloatPoint3 operator -(FloatPoint3 first, FloatPoint3 second)
        {
            return new FloatPoint3(first.X - second.X, first.Y - second.Y, first.Z - second.Z);
        }
        public static implicit operator (float X, float Y, float Z)(FloatPoint3 point)
        {
            return (point.X, point.Y, point.Z);
        }
        public static implicit operator FloatPoint3((float X, float Y, float Z) point)
        {
            return new FloatPoint3(point.X, point.Y, point.Z);
        }
        public static implicit operator float[](FloatPoint3 point)
        {
            return new float[] { point.X, point.Y, point.Z };
        }
        public static FloatPoint3 Create(float x = 0.0f, float y = 0.0f, float z = 0.0f) => new FloatPoint3(x, y, z);
    }
}
