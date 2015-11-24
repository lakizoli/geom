using geom.basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace geom
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow ()
		{
			InitializeComponent ();
		}

		#region Event Handlers

		private void Button_Test1_Click (object sender, RoutedEventArgs e)
		{
			canvas.Children.Clear ();

			canvas.Background = new SolidColorBrush (Colors.DarkSlateBlue);

            canvas.Children.Add (new ShapeTriangle2D {
                Triangle = new Triangle2D { A = new Vector2D (0.1, 0.1), B = new Vector2D (0.2, 0.2), C = new Vector2D (0, 0.2) },
                Fill = new SolidColorBrush (Colors.LawnGreen),
                Stroke = new SolidColorBrush (Colors.DarkGreen)
            });

            Vector2D pos2 = new Vector2D (0.5, 0.5);
            canvas.Children.Add (new ShapeTriangle2D {
                Triangle = new Triangle2D { A = new Vector2D (0.1, 0.1) + pos2, B = new Vector2D (0.2, 0.2) + pos2, C = new Vector2D (0, 0.2) + pos2 },
                Fill = new SolidColorBrush (Colors.Orange),
                Stroke = new SolidColorBrush (Colors.DarkOrange)
            });
        }

        private void canvas_SizeChanged (object sender, SizeChangedEventArgs e)
        {
            foreach (Shape child in canvas.Children) {
                if (child is ShapeTriangle2D) {
                    child.RenderTransform = ((ShapeTriangle2D)child).ActualTransform;
                }
            }
        }

        private bool _isDrag = false;
        private Point _dragStartPt;

        private void canvas_MouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        {
            _dragStartPt = e.GetPosition (canvas);
            _isDrag = true;
        }

        private void canvas_MouseLeftButtonUp (object sender, MouseButtonEventArgs e)
        {
            _isDrag = false;
        }

        private void canvas_MouseEnter (object sender, MouseEventArgs e)
        {

        }

        private void canvas_MouseLeave (object sender, MouseEventArgs e)
        {
            _isDrag = false;
        }
        
        private void canvas_MouseMove (object sender, MouseEventArgs e)
        {
            if (_isDrag) {
                Point pt = e.GetPosition (canvas);
                foreach (Shape child in canvas.Children) {
                    if (child is ShapeTriangle2D) {
                        ShapeTriangle2D tria = (ShapeTriangle2D)child;
                        tria.Translate = new TranslateTransform (pt.X - _dragStartPt.X, pt.Y - _dragStartPt.X);
                        child.RenderTransform = tria.ActualTransform;
                    }
                }
            }
        }

        #endregion
    }

    #region Drawing classes

    class ShapeTriangle2D : Shape
	{
		public Triangle2D Triangle { get; set; }
        public TranslateTransform Translate { get; set; }
        public RotateTransform Rotation { get; set; }

        private Size _originalRenderSize;
        public TransformGroup ActualTransform
        {
            get
            {
                TransformGroup res = new TransformGroup ();

                Size renderSize = ((Canvas)Parent).RenderSize;
                res.Children.Add (new ScaleTransform (renderSize.Width / _originalRenderSize.Width, renderSize.Height / _originalRenderSize.Height));

                if (Rotation != null)
                    res.Children.Add (Rotation);

                if (Translate != null)
                    res.Children.Add (Translate);

                return res;
            }
        }

		protected override Geometry DefiningGeometry
		{
			get
			{
                Point start = new Point (Triangle.A.X, Triangle.A.Y);

                List<PathSegment> segments = new List<PathSegment> (3);
                segments.Add (new LineSegment (start, true));
                segments.Add (new LineSegment (new Point (Triangle.B.X, Triangle.B.Y), true));
                segments.Add (new LineSegment (new Point (Triangle.C.X, Triangle.C.Y), true));

                List<PathFigure> figures = new List<PathFigure> (1);
                figures.Add (new PathFigure (start, segments, true));

                _originalRenderSize = ((Canvas)Parent).RenderSize;
                return new PathGeometry (figures, FillRule.Nonzero, new ScaleTransform (_originalRenderSize.Width, _originalRenderSize.Height));
            }
		}
    }

	class ShapeLine2D : Shape
	{
		public Line2D Line { get; set; }

		protected override Geometry DefiningGeometry
		{
			get
			{
                return new LineGeometry (new Point (Line.Start.X, Line.Start.Y), new Point (Line.End.X, Line.End.Y));
			}
		}
	}

	#endregion
}
