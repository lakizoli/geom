using geom.basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace geom.tests {
    #region class TestShape
    abstract class TestShape : Shape {
        public TranslateTransform Translate { get; set; }
        public RotateTransform Rotation { get; set; }
        private Size OriginalRenderSize { get; set; }
        public TransformGroup ActualTransform {
            get {
                TransformGroup res = new TransformGroup ();

                Size renderSize = ((Canvas)Parent).RenderSize;
                res.Children.Add (new ScaleTransform (renderSize.Width / OriginalRenderSize.Width, renderSize.Height / OriginalRenderSize.Height));

                if (Rotation != null)
                    res.Children.Add (Rotation);

                if (Translate != null)
                    res.Children.Add (Translate);

                return res;
            }
        }

        protected sealed override Geometry DefiningGeometry {
            get {
                OriginalRenderSize = ((Canvas)Parent).RenderSize;
                Geometry res = DefiningTestGeometry;
                res.Transform = new ScaleTransform (OriginalRenderSize.Width, OriginalRenderSize.Height);
                return res;
            }
        }

        protected abstract Geometry DefiningTestGeometry { get; }
    } 
    #endregion

    #region Test shapes
    class ShapeTriangle2D : TestShape {
        public Triangle2D Triangle { get; set; }
        protected override Geometry DefiningTestGeometry {
            get {
                Point start = new Point (Triangle.A.X, Triangle.A.Y);

                List<PathSegment> segments = new List<PathSegment> (3);
                segments.Add (new LineSegment (start, true));
                segments.Add (new LineSegment (new Point (Triangle.B.X, Triangle.B.Y), true));
                segments.Add (new LineSegment (new Point (Triangle.C.X, Triangle.C.Y), true));

                List<PathFigure> figures = new List<PathFigure> (1);
                figures.Add (new PathFigure (start, segments, true));

                return new PathGeometry (figures, FillRule.Nonzero, null);
            }
        }
    }

    class ShapeLine2D : TestShape {
        public Line2D Line { get; set; }
        protected override Geometry DefiningTestGeometry {
            get {
                return new LineGeometry (new Point (Line.Start.X, Line.Start.Y), new Point (Line.End.X, Line.End.Y));
            }
        }
    }

    class ShapeCircle2D : TestShape {
        public Vector2D Center { get; set; }
        public double Radius { get; set; }
        protected override Geometry DefiningTestGeometry {
            get {
                return new EllipseGeometry (new Point (Center.X, Center.Y), Radius, Radius);
            }
        }
    }
    #endregion
}
