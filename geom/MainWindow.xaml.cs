using geom.basics;
using geom.tests;
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

namespace geom {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private TestBase _currentTest;
        public MainWindow () {
            InitializeComponent ();
        }

        #region Event Handlers

        private void Button_Test1_Click (object sender, RoutedEventArgs e) {
            _currentTest = new Test1 (canvas);
            _currentTest.Init ();
        }

        private void canvas_SizeChanged (object sender, SizeChangedEventArgs e) {
            foreach (Shape child in canvas.Children) {
                if (child is ShapeTriangle2D) {
                    child.RenderTransform = ((ShapeTriangle2D)child).ActualTransform;
                }
            }
        }

        private void canvas_MouseLeftButtonDown (object sender, MouseButtonEventArgs e) {
            if (_currentTest != null)
                _currentTest.OnMouseLeftButtonDown (sender, e);
        }
        private void canvas_MouseLeftButtonUp (object sender, MouseButtonEventArgs e) {
            if (_currentTest != null)
                _currentTest.OnMouseLeftButtonUp (sender, e);
        }
        private void canvas_MouseEnter (object sender, MouseEventArgs e) {
            if (_currentTest != null)
                _currentTest.OnMouseEnter (sender, e);
        }
        private void canvas_MouseLeave (object sender, MouseEventArgs e) {
            if (_currentTest != null)
                _currentTest.OnMouseLeave (sender, e);
        }
        private void canvas_MouseMove (object sender, MouseEventArgs e) {
            if (_currentTest != null)
                _currentTest.OnMouseMove (sender, e);
        }

        #endregion
    }
}
