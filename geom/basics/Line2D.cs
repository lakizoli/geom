using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geom.basics
{
	class Line2D
	{
		#region Data

		public Vector2D Start { get; set; }

		public Vector2D End { get; set; }

		#endregion

		#region Construction

		public Line2D ()
		{
		}

		public Line2D (Vector2D start, Vector2D end)
		{
			Start = start;
			End = end;
		}

		#endregion

		#region Operations

		#endregion
	}
}
