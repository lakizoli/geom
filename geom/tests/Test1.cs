using geom.basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace geom.tests {
    class Test1 : TestBase {
        #region Data
        private Point _dragStartPt;
        private ShapeTriangle2D _draggedTria;
        private Point _draggedTriaPt;

        //private ShapeTriangle2D _tria1 = new ShapeTriangle2D {
        //    Triangle = new Triangle2D {
        //        A = new Vector2D (0.1, 0.1),
        //        B = new Vector2D (0.2, 0.2),
        //        C = new Vector2D (0, 0.2)
        //    },
        //    Fill = new SolidColorBrush (Colors.LawnGreen),
        //    Stroke = new SolidColorBrush (Colors.DarkGreen)
        //};

        //private ShapeTriangle2D _tria1 = new ShapeTriangle2D {
        //    Triangle = new Triangle2D {
        //        A = new Vector2D (0.5, 0.1),
        //        B = new Vector2D (0.9, 0.2),
        //        C = new Vector2D (0, 0.2)
        //    },
        //    Fill = new SolidColorBrush (Colors.LawnGreen),
        //    Stroke = new SolidColorBrush (Colors.DarkGreen)
        //};

        private ShapeTriangle2D _tria1 = new ShapeTriangle2D {
            Triangle = new Triangle2D {
                A = new Vector2D (0.1, 0.2),
                B = new Vector2D (0.2, 0.1),
                C = new Vector2D (0, 0.1)
            },
            Fill = new SolidColorBrush (Colors.LawnGreen),
            Stroke = new SolidColorBrush (Colors.DarkGreen)
        };

        private ShapeTriangle2D _tria2 = new ShapeTriangle2D {
            Triangle = new Triangle2D {
                A = new Vector2D (0.1, 0.1) * 1.4 + new Vector2D (0.5, 0.5),
                B = new Vector2D (0.2, 0.2) * 1.4 + new Vector2D (0.5, 0.5),
                C = new Vector2D (0, 0.2) * 1.4 + new Vector2D (0.5, 0.5)
            },
            Fill = new SolidColorBrush (Colors.Orange),
            Stroke = new SolidColorBrush (Colors.DarkOrange)
        };

        private SolidColorBrush _brushTransparent = new SolidColorBrush (Colors.Transparent);
        private SolidColorBrush _brushRed = new SolidColorBrush (Colors.Red);
        private SolidColorBrush _brushBlue = new SolidColorBrush (Colors.Blue);
        private SolidColorBrush _brushBlueTrans = new SolidColorBrush (new Color () { R = 0, G = 0, B = 255, A = 192 } );
        #endregion

        public Test1 (Canvas _canvas) : base (_canvas) {
        }

        public override void Init () {
            canvas.Children.Clear ();
            canvas.Background = new SolidColorBrush (Colors.DarkSlateBlue);
            canvas.Children.Add (_tria1);
            canvas.Children.Add (_tria2);
        }

        #region Mouse events
        public override void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e) {
            Point pt = e.GetPosition (canvas);
            Vector2D local = new Vector2D (pt.X / canvas.RenderSize.Width, pt.Y / canvas.RenderSize.Height);
            Vector2D tria1Local = _tria1.Translate == null ? new Vector2D () : new Vector2D (_tria1.Translate.X / canvas.RenderSize.Width, _tria1.Translate.Y / canvas.RenderSize.Height);
            Vector2D tria2Local = _tria2.Translate == null ? new Vector2D () : new Vector2D (_tria2.Translate.X / canvas.RenderSize.Width, _tria2.Translate.Y / canvas.RenderSize.Height);
            if (_tria1.Triangle.Contains (local - tria1Local))
                _draggedTria = _tria1;
            else if (_tria2.Triangle.Contains (local - tria2Local))
                _draggedTria = _tria2;

            if (_draggedTria != null) {
                _dragStartPt = pt;
                _draggedTriaPt.X = _draggedTria.Translate == null ? 0 : _draggedTria.Translate.X;
                _draggedTriaPt.Y = _draggedTria.Translate == null ? 0 : _draggedTria.Translate.Y;
            }
        }
        public override void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e) {
            _draggedTria = null;
        }
        public override void OnMouseEnter (object sender, MouseEventArgs e) {

        }
        public override void OnMouseLeave (object sender, MouseEventArgs e) {
            _draggedTria = null;
        }
        public override void OnMouseMove (object sender, MouseEventArgs e) {
            if (_draggedTria != null) {
                Point pt = e.GetPosition (canvas);
                _draggedTria.Translate = new TranslateTransform (pt.X - _dragStartPt.X + _draggedTriaPt.X, pt.Y - _dragStartPt.Y + _draggedTriaPt.Y);
                _draggedTria.RenderTransform = _draggedTria.ActualTransform;

                //Testing intersection points
                Triangle2D test1 = _draggedTria.Triangle.Offset (new Vector2D ((pt.X - _dragStartPt.X + _draggedTriaPt.X) / canvas.RenderSize.Width, (pt.Y - _dragStartPt.Y + _draggedTriaPt.Y) / canvas.RenderSize.Height));
                Triangle2D test2 = null;
                if (_draggedTria == _tria1)
                    test2 = _tria2.Triangle.Offset (_tria2.Translate == null ? new Vector2D () : new Vector2D (_tria2.Translate.X / canvas.RenderSize.Width, _tria2.Translate.Y / canvas.RenderSize.Height));
                else if (_draggedTria == _tria2)
                    test2 = _tria1.Triangle.Offset (_tria1.Translate == null ? new Vector2D () : new Vector2D (_tria1.Translate.X / canvas.RenderSize.Width, _tria1.Translate.Y / canvas.RenderSize.Height));

                //Remove all intersection points from canvas
                for (int i = canvas.Children.Count; i > 0; --i) {
                    UIElement element = canvas.Children[i - 1];
                    if (element is Shape) {
                        object tag = ((Shape)element).Tag;
                        if (tag != null && ((int)tag == 100 || (int)tag == 101))
                            canvas.Children.RemoveAt (i - 1);
                    }
                }

                if (test2 != null) {
                    List<Vector2D> intersectionPoints = Triangle2D.IntersectionPoints (test1, test2);
                    if (intersectionPoints != null) {
                        //Add new intersection points to canvas
                        foreach (Vector2D item in intersectionPoints) {
                            canvas.Children.Add (new ShapeCircle2D () {
                                Center = item,
                                Radius = 0.01,
                                Fill = _brushTransparent,
                                Stroke = _brushRed,
                                StrokeThickness = 3,
                                Tag = 100
                            });
                        }
                    }

                    List<Triangle2D> intersectionTriangles = Triangle2D.Cut (test2, test1); //Cut with dragged triangle
                    if (intersectionTriangles != null) {
                        foreach (Triangle2D tria in intersectionTriangles) {
                            canvas.Children.Add (new ShapeTriangle2D () {
                                Triangle = tria,
                                Fill = _brushBlueTrans,
                                Stroke = _brushBlue,
                                StrokeThickness = 1.5,
                                Tag = 101
                            });
                        }
                    }
                }
            }
        }
        #endregion
    }
}
