using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace geom.tests
{
    abstract class TestBase
    {
        public Canvas canvas { get; set; }
        public abstract void Init ();
        public TestBase (Canvas _canvas)
        {
            canvas = _canvas;
        }
        public virtual void OnMouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        {
        }
        public virtual void OnMouseLeftButtonUp (object sender, MouseButtonEventArgs e)
        {
        }
        public virtual void OnMouseEnter (object sender, MouseEventArgs e)
        {

        }
        public virtual void OnMouseLeave (object sender, MouseEventArgs e)
        {
        }
        public virtual void OnMouseMove (object sender, MouseEventArgs e)
        {
        }
    }
}
