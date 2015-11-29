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

        public Triangle2D Offset (Vector2D vec) {
            return new Triangle2D () { A = A + vec, B = B + vec, C = C + vec };
        }

        public Rect2D BoundingBox {
            get {
                return new Rect2D (Math.Min (A.X, Math.Min (B.X, C.X)),
                    Math.Min (A.Y, Math.Min (B.Y, C.Y)),
                    Math.Max (A.X, Math.Max (B.X, C.X)),
                    Math.Max (A.Y, Math.Max (B.Y, C.Y)));
            }
        }

        public static List<Triangle2D> Cut (Triangle2D tria, Triangle2D cut) {
            if (!tria.BoundingBox.Intersects (cut.BoundingBox))
                return null;

            List<Triangle2D> res = null;

            if (tria.Contains (cut)) { //Check if tria contains the whole cut triangle
                Vector2D nearestA = NearestPoint (tria.A, cut);
                Vector2D nearestB = NearestPoint (tria.B, cut);
                Vector2D nearestC = NearestPoint (tria.C, cut);

                Line2D sideA = new Line2D () { Start = tria.A, End = tria.B };
                Line2D sideB = new Line2D () { Start = tria.B, End = tria.C };
                Line2D sideC = new Line2D () { Start = tria.C, End = tria.A };

                res = new List<Triangle2D> ();

                res.Add (new Triangle2D (sideA.Start, sideA.End, nearestA));
                res.Add (new Triangle2D (nearestA, sideB.Start, nearestB));

                res.Add (new Triangle2D (sideB.Start, sideB.End, nearestB));
                res.Add (new Triangle2D (nearestB, sideC.Start, nearestC));

                res.Add (new Triangle2D (sideC.Start, sideC.End, nearestC));
                res.Add (new Triangle2D (nearestC, sideA.Start, nearestA));
            } else if (cut.Contains (tria)) { //The cut triangle contains the whole tria (the whole tria will be cut off)
                //... nothing to do
            } else { //Intersection can happen...
                List<Vector2D> intersectionPoints = Triangle2D.IntersectionPoints (tria, cut);
                if (intersectionPoints != null) {
                    switch (intersectionPoints.Count) {
                        case 2:
                            {
                                Vector2D pt1 = intersectionPoints[0];
                                Vector2D pt2 = intersectionPoints[1];

                                Line2D sideA = new Line2D () { Start = tria.A, End = tria.B };
                                Line2D sideB = new Line2D () { Start = tria.B, End = tria.C };
                                Line2D sideC = new Line2D () { Start = tria.C, End = tria.A };

                                bool hasA1 = sideA.Contains (pt1);
                                bool hasA2 = sideA.Contains (pt2);

                                bool hasB1 = sideB.Contains (pt1);
                                bool hasB2 = sideB.Contains (pt2);

                                bool hasC1 = sideC.Contains (pt1);
                                bool hasC2 = sideC.Contains (pt2);

                                if (hasA1 && hasA2) { //sideA has two intersection point
                                    res = CutOneSideWithTwoPoint (tria, cut, pt1, pt2, sideA, tria.C);
                                } else if (hasB1 && hasB2) { //sideB has two intersection point
                                    res = CutOneSideWithTwoPoint (tria, cut, pt1, pt2, sideB, tria.A);
                                } else if (hasC1 && hasC2) { //sideC has two intersection point
                                    res = CutOneSideWithTwoPoint (tria, cut, pt1, pt2, sideC, tria.B);
                                } else if ((hasA1 || hasA2) && (hasB1 || hasB2)) { //each of sideA and sideB has one intersection point
                                    //Itt nincs lekezelve minden eset!!! (fejjel lefele...)
                                    res = new List<Triangle2D> ();
                                    res.Add (new Triangle2D (pt1, sideC.Start, sideC.End));
                                    res.Add (new Triangle2D (pt1, pt2, sideC.Start));
                                } else if ((hasB1 || hasB2) && (hasC1 || hasC2)) { //each of sideB and sideC has one intersection point
                                    //Itt nincs lekezelve minden eset!!!
                                    res = new List<Triangle2D> ();
                                    res.Add (new Triangle2D (pt1, sideA.Start, sideA.End));
                                    res.Add (new Triangle2D (pt1, pt2, sideA.Start));
                                } else if ((hasC1 || hasC2) && (hasA1 || hasA2)) { //each of sideC and sideA has one intersection point
                                    //Itt nincs lekezelve minden eset!!!
                                    res = new List<Triangle2D> ();
                                    res.Add (new Triangle2D (pt1, sideB.Start, sideB.End));
                                    res.Add (new Triangle2D (pt1, pt2, sideB.End));
                                }

                                break;
                            }
                        case 4:
                            {
                                Line2D sideA = new Line2D () { Start = tria.A, End = tria.B };
                                Line2D sideB = new Line2D () { Start = tria.B, End = tria.C };
                                Line2D sideC = new Line2D () { Start = tria.C, End = tria.A };

                                List<Vector2D> sideAPoints = new List<Vector2D> ();
                                List<Vector2D> sideBPoints = new List<Vector2D> ();
                                List<Vector2D> sideCPoints = new List<Vector2D> ();
                                for (int i = 0; i < 4; ++i) {
                                    if (sideA.Contains (intersectionPoints[i]))
                                        sideAPoints.Add (intersectionPoints[i]);
                                    else if (sideB.Contains (intersectionPoints[i]))
                                        sideBPoints.Add (intersectionPoints[i]);
                                    else if (sideC.Contains (intersectionPoints[i]))
                                        sideCPoints.Add (intersectionPoints[i]);
                                }

                                if (sideAPoints.Count == 2 && sideBPoints.Count == 2) {
                                    res = CutWithFourPointsOnTwoSide (tria.B, sideC, sideAPoints, sideBPoints);
                                } else if (sideBPoints.Count == 2 && sideCPoints.Count == 2) {
                                    res = CutWithFourPointsOnTwoSide (tria.C, sideA, sideBPoints, sideCPoints);
                                } else if (sideCPoints.Count == 2 && sideAPoints.Count == 2) {
                                    res = CutWithFourPointsOnTwoSide (tria.A, sideB, sideCPoints, sideAPoints);
                                } else if (sideAPoints.Count == 2 && sideBPoints.Count == 1 && sideCPoints.Count == 1) {
                                    res = CutWithFourPoint (sideA, sideAPoints, sideCPoints[0], sideBPoints[0]);
                                } else if (sideBPoints.Count == 2 && sideCPoints.Count == 1 && sideAPoints.Count == 1) {
                                    res = CutWithFourPoint (sideB, sideBPoints, sideAPoints[0], sideCPoints[0]);
                                } else if (sideCPoints.Count == 2 && sideAPoints.Count == 1 && sideBPoints.Count == 1) {
                                    res = CutWithFourPoint (sideC, sideCPoints, sideBPoints[0], sideAPoints[0]);
                                }

                                break;
                            }
                        case 6: //Star with 6 endpoint.
                            {
                                Line2D sideA = new Line2D () { Start = tria.A, End = tria.B };
                                Line2D sideB = new Line2D () { Start = tria.B, End = tria.C };
                                Line2D sideC = new Line2D () { Start = tria.C, End = tria.A };

                                List<Vector2D> sideAPoints = new List<Vector2D> ();
                                List<Vector2D> sideBPoints = new List<Vector2D> ();
                                List<Vector2D> sideCPoints = new List<Vector2D> ();
                                for (int i = 0; i < 6; ++i) {
                                    if (sideA.Contains (intersectionPoints[i]))
                                        sideAPoints.Add (intersectionPoints[i]);
                                    else if (sideB.Contains (intersectionPoints[i]))
                                        sideBPoints.Add (intersectionPoints[i]);
                                    else if (sideC.Contains (intersectionPoints[i]))
                                        sideCPoints.Add (intersectionPoints[i]);
                                }

                                if (sideAPoints.Count == 2 && sideBPoints.Count == 2 && sideCPoints.Count == 2) {
                                    //TODO: ...
                                }

                                break;
                            }
                        case 1: //If we have only one intersection point, then the tria triangle need to be untouched, because the cut triangle only touches it.
                        default: //All other not handled case is error case.
                            break;
                    }
                }
            }

            if (res != null && res.Count <= 0)
                return null;

            return res;
        }

        public static List<Vector2D> IntersectionPoints (Triangle2D triaA, Triangle2D triaB) {
            if (!triaA.BoundingBox.Intersects (triaB.BoundingBox))
                return null;

            List<Vector2D> res = new List<Vector2D> ();

            if (triaA.Contains (triaB)) { //Check if triaA contains the whole triaB
                res.Add (triaB.A);
                res.Add (triaB.B);
                res.Add (triaB.C);
            } else if (triaB.Contains (triaA)) { //Check if triaB contains the whole triaA
                res.Add (triaA.A);
                res.Add (triaA.B);
                res.Add (triaA.C);
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

        private static List<Triangle2D> CutOneSideWithTwoPoint (Triangle2D tria, Triangle2D cut, Vector2D intersectionPoint1, Vector2D intersectionPoint2, Line2D side, Vector2D triaNotOnSideNode) {
            List<Triangle2D> res = null;

            bool hasA = tria.Contains (cut.A);
            bool hasB = tria.Contains (cut.B);
            bool hasC = tria.Contains (cut.C);

            Vector2D insidePoint = null;

            if ((hasA && hasB) || (hasB && hasC) || (hasC && hasA)) { //The cut triangle only touches the sideA from outer
                //... nothing to do
            } else if (hasA) {
                insidePoint = cut.A;
            } else if (hasB) {
                insidePoint = cut.B;
            } else if (hasC) {
                insidePoint = cut.C;
            }

            if (insidePoint != null) {
                double dist1 = (intersectionPoint1 - side.Start).SquareLength;
                double dist2 = (intersectionPoint2 - side.Start).SquareLength;

                res = new List<Triangle2D> ();

                res.Add (new Triangle2D (side.Start, dist1 < dist2 ? intersectionPoint1 : intersectionPoint2, insidePoint));
                res.Add (new Triangle2D (side.Start, insidePoint, triaNotOnSideNode));
                res.Add (new Triangle2D (dist1 < dist2 ? intersectionPoint2 : intersectionPoint1, side.End, insidePoint));
                res.Add (new Triangle2D (side.End, triaNotOnSideNode, insidePoint));
            }

            return res;
        }

        private static List<Triangle2D> CutWithFourPointsOnTwoSide (Vector2D sideMiddlePoint, Line2D otherSide, List<Vector2D> side1Pts, List<Vector2D> side2Pts) {
            double distA1 = (side1Pts[0] - sideMiddlePoint).SquareLength;
            double distA2 = (side1Pts[1] - sideMiddlePoint).SquareLength;

            double distB1 = (side2Pts[0] - sideMiddlePoint).SquareLength;
            double distB2 = (side2Pts[1] - sideMiddlePoint).SquareLength;

            List<Triangle2D> res = res = new List<Triangle2D> ();

            res.Add (new Triangle2D (side1Pts[distA1 < distA2 ? 0 : 1], sideMiddlePoint, side2Pts[distB1 < distB2 ? 0 : 1]));
            res.Add (new Triangle2D (otherSide.End, side1Pts[distA1 < distA2 ? 1 : 0], otherSide.Start));
            res.Add (new Triangle2D (side1Pts[distA1 < distA2 ? 1 : 0], side2Pts[distB1 < distB2 ? 1 : 0], otherSide.Start));

            return res;
        }

        private static List<Triangle2D> CutWithFourPoint (Line2D side, List<Vector2D> sidePoints, Vector2D startIntersectionPoint, Vector2D endIntersectionPoint) {
            double dist1 = (side.Start - sidePoints[0]).SquareLength;
            double dist2 = (side.Start - sidePoints[1]).SquareLength;

            List<Triangle2D> res = new List<Triangle2D> ();
            res.Add (new Triangle2D (sidePoints[dist1 < dist2 ? 0 : 1], side.Start, startIntersectionPoint));
            res.Add (new Triangle2D (sidePoints[dist1 < dist2 ? 1 : 0], endIntersectionPoint, side.End));

            return res;
        }

        private static Vector2D NearestPoint (Vector2D triaPoint, Triangle2D cut) {
            double distA = (triaPoint - cut.A).SquareLength;

            Vector2D nearest = cut.A;
            if ((triaPoint - cut.B).SquareLength < distA)
                nearest = cut.B;
            if ((triaPoint - cut.C).SquareLength < distA)
                nearest = cut.C;

            return nearest;
        }

        #endregion
    }
}
