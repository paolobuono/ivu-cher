using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace AvengersUtd.Explore.Environment.Controls
{
    public static class Extensions
    {
        public static Point GetLocation(this FrameworkElement uiElement)
        {
            return uiElement.TransformToAncestor((Visual)uiElement.Parent).Transform(new Point(0,0));
        }

        public static Point GetLocation(this FrameworkElement uiElement, Point pointOnControl)
        {
            return uiElement.TransformToAncestor((Visual)uiElement.Parent).Transform(pointOnControl);
        }

        public static double DistanceSquared(this Point point, Point targetPoint)
        {
            return Math.Pow(point.X - targetPoint.X, 2.0) +Math.Pow(point.Y-targetPoint.Y,2.0);
        }

        public static double Distance(this Point point, Point targetPoint)
        {
            return Math.Sqrt(DistanceSquared(point, targetPoint));
        }
    }
}
