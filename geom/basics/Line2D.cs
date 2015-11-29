using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geom.basics {
    class Line2D {
        #region Data

        public Vector2D Start { get; set; }

        public Vector2D End { get; set; }

        #endregion

        #region Construction

        public Line2D () {
        }

        public Line2D (Vector2D start, Vector2D end) {
            Start = start;
            End = end;
        }

        #endregion

        #region Operations

        public static Vector2D Intersection (Line2D lineA, Line2D lineB) {
            Vector2D x = lineB.Start - lineA.Start;
            Vector2D d1 = lineA.End - lineA.Start;
            Vector2D d2 = lineB.End - lineB.Start;

            double cross = Vector2D.Cross (d1, d2);
            if (Math.Abs (cross) < 1e-8)
                return null;

            double t1 = Vector2D.Cross (x, d2) / cross;
            Vector2D pt = lineA.Start + d1 * t1;

            //Check point validity by distance
            double sqlenA = d1.SquareLength;
            if ((pt - lineA.Start).SquareLength > sqlenA || (pt - lineA.End).SquareLength > sqlenA)
                return null;

            double sqlenB = d2.SquareLength;
            if ((pt - lineB.Start).SquareLength > sqlenB || (pt - lineB.End).SquareLength > sqlenB)
                return null;

            return pt;
        }

        public Vector2D Intersection (Line2D line) {
            return Line2D.Intersection (this, line);
        }

        #endregion
    }
}
