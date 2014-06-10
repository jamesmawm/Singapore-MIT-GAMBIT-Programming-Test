using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SpaceCode
{
    // 2d vector value type.
    //  This means it passes by value, instead of by reference.
    public struct Vector2F
    {
        public double X;
        public double Y;

        public Vector2F(double x, double y)
        {
            X = x;
            Y = y;
        }

        // Builds a Vector2F out of a Size structure.
        public Vector2F(Size size)
            : this(size.Width, size.Height)
        { }

        public static Vector2F operator/(Vector2F v, double divisor)
        {
            return new Vector2F(v.X / divisor, v.Y / divisor);
        }

        public static Vector2F operator -(Vector2F v, Vector2F v2)
        {
            return new Vector2F(v.X-v2.X, v.Y-v2.Y);
        }

        public static Vector2F operator -(Vector2F v)
        {
            return new Vector2F(-v.X, -v.Y);
        }

        public static Vector2F operator +(Vector2F v, Vector2F v2)
        {
            return new Vector2F(v.X + v2.X, v.Y + v2.Y);
        }

        public static Vector2F operator *(Vector2F v, double multiplier)
        {
            return new Vector2F(v.X * multiplier, v.Y * multiplier);
        }

        public static implicit operator Vector2F(Point p)
        {
            return new Vector2F(p.X, p.Y);
        }

        public static implicit operator PointF(Vector2F v)
        {
            return new PointF((float)v.X, (float)v.Y);
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }

    }
}
