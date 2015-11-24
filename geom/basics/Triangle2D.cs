using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geom.basics
{
	class Triangle2D
	{
		#region Data

		public Vector2D A { get; set; }

		public Vector2D B { get; set; }

		public Vector2D C { get; set; }

		#endregion

		#region Construction

		public Triangle2D ()
		{
		}

		public Triangle2D (Vector2D a, Vector2D b, Vector2D c)
		{
			A = a;
			B = b;
			C = c;
		}

		#endregion

		#region Operations

		public List<Vector2D> IntersectionPoints (Triangle2D tria)
		{
			return null;
		}

		public bool Contains (Vector2D pt)
		{
			return SameSide (pt, A, new Line2D () { Start = B, End = C }) &&
				SameSide (pt, B, new Line2D () { Start = C, End = A }) &&
				SameSide (pt, C, new Line2D () { Start = A, End = B });
		}

		#endregion

		#region Implementation

		private bool SameSide (Vector2D pt1, Vector2D pt2, Line2D side)
		{
			Vector2D sideVec = side.End - side.Start;
            double cp1 = Vector2D.Cross (sideVec, pt1 - side.Start);
            double cp2 = Vector2D.Cross (sideVec, pt2 - side.Start);
			return cp1 * cp2 >= 0.0;
		}

		#endregion
	}
}
