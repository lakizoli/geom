using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geom.basics
{
	class Vector2D
	{
		#region Data

		public double X { get; set; }

		public double Y { get; set; }

		#endregion

		#region Construction

		public Vector2D ()
		{
		}

		public Vector2D (double x, double y)
		{
			X = x;
			Y = y;
		}

		#endregion

		#region Operations

		public double SquareLength { get { return X * X + Y * Y; } }

		public double Length { get { return Math.Sqrt (SquareLength); } }

		public static Vector2D operator +(Vector2D v1, Vector2D v2)
		{
			return new Vector2D { X = v1.X + v2.X, Y = v1.Y + v2.Y };
		}

		public static Vector2D operator -(Vector2D v1, Vector2D v2)
		{
			return new Vector2D { X = v1.X - v2.X, Y = v1.Y - v2.Y };
		}

		public static Vector2D operator *(Vector2D vec, double multiplier)
		{
			return new Vector2D { X = vec.X * multiplier, Y = vec.Y * multiplier };
		}

		public static Vector2D operator *(double multiplier, Vector2D vec)
		{
			return new Vector2D { X = vec.X * multiplier, Y = vec.Y * multiplier };
		}

		public static Vector2D operator /(Vector2D vec, double divider)
		{
			return new Vector2D { X = vec.X / divider, Y = vec.Y / divider };
		}

		public static Vector2D operator /(double divider, Vector2D vec)
		{
			return new Vector2D { X = divider / vec.X, Y = divider / vec.Y };
		}

		public static double Dot (Vector2D v1, Vector2D v2)
		{
			return v1.X * v2.X + v1.Y * v2.Y;
		}

		public double Dot (Vector2D vec)
		{
			return Vector2D.Dot (this, vec);
		}

		public static double Cross (Vector2D v1, Vector2D v2)
		{
			return v1.X * v2.Y - v2.X * v1.Y;
		}

		public double Cross (Vector2D vec)
		{
			return Vector2D.Cross (this, vec);
		}

		#endregion
	}
}
