using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geom.basics {
    class Vector2D {
        #region Data

        public double X { get; set; }

        public double Y { get; set; }

        #endregion

        #region Construction

        public Vector2D () {
        }

        public Vector2D (double x, double y) {
            X = x;
            Y = y;
        }

        #endregion

        #region Operations

        public double SquareLength { get { return X * X + Y * Y; } }

        public double Length { get { return Math.Sqrt (SquareLength); } }

        public static Vector2D operator +(Vector2D v1, Vector2D v2) {
            return new Vector2D { X = v1.X + v2.X, Y = v1.Y + v2.Y };
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2) {
            return new Vector2D { X = v1.X - v2.X, Y = v1.Y - v2.Y };
        }

        public static Vector2D operator *(Vector2D vec, double multiplier) {
            return new Vector2D { X = vec.X * multiplier, Y = vec.Y * multiplier };
        }

        public static Vector2D operator *(double multiplier, Vector2D vec) {
            return new Vector2D { X = vec.X * multiplier, Y = vec.Y * multiplier };
        }

        public static Vector2D operator /(Vector2D vec, double divider) {
            return new Vector2D { X = vec.X / divider, Y = vec.Y / divider };
        }

        public static Vector2D operator /(double divider, Vector2D vec) {
            return new Vector2D { X = divider / vec.X, Y = divider / vec.Y };
        }

        public static double Dot (Vector2D v1, Vector2D v2) {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public double Dot (Vector2D vec) {
            return Vector2D.Dot (this, vec);
        }

        public static double Cross (Vector2D v1, Vector2D v2) {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        public double Cross (Vector2D vec) {
            return Vector2D.Cross (this, vec);
        }

        public static bool operator >(Vector2D v1, Vector2D v2) {
            return v1.X > v2.X && v1.Y > v2.Y;
        }

        public static bool operator <(Vector2D v1, Vector2D v2) {
            return v1.X < v2.X || v1.Y < v2.Y;
        }

        public static bool operator >=(Vector2D v1, Vector2D v2) {
            return !(v1 < v2);
        }

        public static bool operator <=(Vector2D v1, Vector2D v2) {
            return !(v1 > v2);
        }

        public static bool operator ==(Vector2D v1, Vector2D v2) {
            // Testing trivial cases
            if ((object)v1 == null && (object)v2 == null)
                return true;

            if ((object)v1 == null || (object)v2 == null)
                return false;

            // If they are equal anyway, just return True.
            if (v1.X == v2.X && v1.Y == v2.Y)
                return true;

            // Handle NaN, Infinity.
            if (Double.IsInfinity (v1.X) | Double.IsNaN (v1.X) | Double.IsInfinity (v2.X) | Double.IsNaN (v2.X))
                return v1.X.Equals (v2.X);
            else if (Double.IsInfinity (v1.Y) | Double.IsNaN (v1.Y) | Double.IsInfinity (v2.Y) | Double.IsNaN (v2.Y))
                return v1.Y.Equals (v2.Y);

            // Handle zero to avoid division by zero
            double divisorX = Math.Max (v1.X, v2.X);
            if (divisorX.Equals (0))
                divisorX = Math.Min (v1.X, v2.X);

            double divisorY = Math.Max (v1.Y, v2.Y);
            if (divisorY.Equals (0))
                divisorY = Math.Min (v1.Y, v2.Y);

            return Math.Abs (v1.X - v2.X) / divisorX <= 0.00001 && Math.Abs (v1.Y - v2.Y) / divisorY <= 0.00001;
        }

        public static bool operator !=(Vector2D v1, Vector2D v2) {
            return !(v1 == v2);
        }

        public override bool Equals (object obj) {
            if (obj == null || !(obj is Double))
                return false;

            return this == (Vector2D)obj;
        }

        public override int GetHashCode () {
            string val = X.ToString () + ":" + Y.ToString ();
            return val.GetHashCode ();
        }

        #endregion
    }
}
