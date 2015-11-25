using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geom.basics
{
    class Rect2D
    {
        #region Data

        public Vector2D LeftTop { get; set; }
        public Vector2D RightBottom { get; set; }

        #endregion

        #region Construction

        public Rect2D ()
        {
        }

        public Rect2D (double left, double top, double right, double bottom)
            : this (new Vector2D (left, top), new Vector2D (right, bottom))
        {
        }

        public Rect2D (Vector2D leftTop, Vector2D rightBottom)
        {
            LeftTop = leftTop;
            RightBottom = rightBottom;
        }

        #endregion

        #region Operations

        public bool Intersects (Rect2D rect)
        {
            return (LeftTop >= rect.LeftTop && LeftTop <= rect.RightBottom) ||
                (RightBottom >= rect.LeftTop && RightBottom <= rect.RightBottom) ||
                (rect.LeftTop >= LeftTop && rect.LeftTop <= RightBottom) ||
                (rect.RightBottom >= LeftTop && rect.RightBottom <= RightBottom);
        }

        #endregion
    }
}
