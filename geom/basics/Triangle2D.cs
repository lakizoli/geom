using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geom.basics {
    class Triangle2D {
        #region Data

        public Vector2D A { get; set; }

        public Vector2D B { get; set; }

        public Vector2D C { get; set; }

        #endregion

        #region Construction

        public Triangle2D () {
        }

        public Triangle2D (Vector2D a, Vector2D b, Vector2D c) {
            A = a;
            B = b;
            C = c;
        }

        #endregion

        #region Operations

        public Rect2D BoundingBox {
            get {
                return new Rect2D (Math.Min (A.X, Math.Min (B.X, C.X)),
                    Math.Min (A.Y, Math.Min (B.Y, C.Y)),
                    Math.Max (A.X, Math.Max (B.X, C.X)),
                    Math.Max (A.Y, Math.Max (B.Y, C.Y)));
            }
        }

        public Triangle2D Offset (Vector2D vec) {
            return new Triangle2D () { A = A + vec, B = B + vec, C = C + vec };
        }

        public static List<Vector2D> IntersectionPoints (Triangle2D triaA, Triangle2D triaB) {
            if (!triaA.BoundingBox.Intersects (triaB.BoundingBox))
                return null;

            List<Vector2D> res = new List<Vector2D> ();

            if (triaA.Contains (triaB)) { //Check if triaA contains the whole triaB
                res.Add (triaB.A);
                res.Add (triaB.B);
                res.Add (triaB.C);
                return res;
            } else if (triaB.Contains (triaA)) { //Check if triaB contains the whole triaA
                res.Add (triaA.A);
                res.Add (triaA.B);
                res.Add (triaA.C);
                return res;
            } else { //Check intersections
                Line2D[] sidesA = new Line2D[] {
                    new Line2D () { Start = triaA.A, End = triaA.B },
                    new Line2D () { Start = triaA.B, End = triaA.C },
                    new Line2D () { Start = triaA.C, End = triaA.A }
                };

                Line2D[] sidesB = new Line2D[] {
                    new Line2D () { Start = triaB.A, End = triaB.B },
                    new Line2D () { Start = triaB.B, End = triaB.C },
                    new Line2D () { Start = triaB.C, End = triaB.A }
                };

                for (int i = 0; i < 3; ++i) {
                    for (int j = 0; j < 3; ++j) {
                        Vector2D pt = Line2D.Intersection (sidesA[i], sidesB[j]);
                        if (pt != null)
                            res.Add (pt);
                    }
                }
            }

            if (res.Count <= 0)
                return null;

            return res;
        }

        public bool Contains (Vector2D pt) {
            return SameSide (pt, A, new Line2D () { Start = B, End = C }) &&
                SameSide (pt, B, new Line2D () { Start = C, End = A }) &&
                SameSide (pt, C, new Line2D () { Start = A, End = B });
        }

        public bool Contains (Triangle2D tria) {
            return Contains (tria.A) && Contains (tria.B) && Contains (tria.C);
        }

        #endregion

        #region Implementation

        private bool SameSide (Vector2D pt1, Vector2D pt2, Line2D side) {
            Vector2D sideVec = side.End - side.Start;
            double cp1 = Vector2D.Cross (sideVec, pt1 - side.Start);
            double cp2 = Vector2D.Cross (sideVec, pt2 - side.Start);
            return cp1 * cp2 >= 0.0;
        }

        #endregion
    }
}
