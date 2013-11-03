#region Disclaimer
// /* 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com/blog
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */
#endregion

#region Using Directives

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using AvengersUtd.Explore.Environment.Controls.Elements;

#endregion

namespace AvengersUtd.Explore.Environment.Controls
{
    public class ConnectorAdorner : Adorner
    {
        private const int thumbSize = 16;
        private readonly Thumb left;
        private readonly Thumb right;
        private readonly Thumb top;
        private readonly Thumb bottom;
        readonly VisualCollection visualChildren;

        private Point startPoint;
        private bool isDrawCurveMode;
        private bool linkFound;

        private readonly GridCanvas ownerCanvas;
        private readonly Element ownerElement;

        private ThumbPosition tempTargetPosition;
        private Element tempTargetElement;

        public ConnectorAdorner(UIElement adornedElement, Element owner)
            : base(adornedElement)
        {
            ownerElement = owner;
            visualChildren = new VisualCollection(this);
            ownerCanvas = Global.WorkArea;

            // Call a helper method to initialize the Thumbs
            // with a customized cursors.
            BuildAdornerThumb(ref left, Cursors.Cross, ThumbPosition.Left);
            BuildAdornerThumb(ref right, Cursors.SizeNESW, ThumbPosition.Right);
            BuildAdornerThumb(ref top, Cursors.SizeNESW, ThumbPosition.Top);
            BuildAdornerThumb(ref bottom, Cursors.SizeNWSE, ThumbPosition.Bottom);
        }

        public void SetVisibility(ThumbPosition position, bool value)
        {
            Thumb control;
            switch (position)
            {
                default:
                case ThumbPosition.Left:
                    control = left;
                    break;
                case ThumbPosition.Top:
                    control = top;
                    break;
                case ThumbPosition.Right:
                    control = right;
                    break;
                case ThumbPosition.Bottom:
                    control = bottom;
                    break;
            }

            control.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        public void SetVisibility(bool valueForAll)
        {
            SetVisibility(ThumbPosition.Left, valueForAll);
            SetVisibility(ThumbPosition.Top, valueForAll);
            SetVisibility(ThumbPosition.Right, valueForAll);
            SetVisibility(ThumbPosition.Bottom, valueForAll);
        }

        public void SetBackground(ThumbPosition position, Brush brush)
        {
            Thumb control;
            switch (position)
            {
                default:
                case ThumbPosition.Left:
                    control = left;
                    break;
                case ThumbPosition.Top:
                    control = top;
                    break;
                case ThumbPosition.Right:
                    control = right;
                    break;
                case ThumbPosition.Bottom:
                    control = bottom;
                    break;
            }

            control.Background = brush;
        }

        public void AddEffect(ThumbPosition position, Effect effect)
        {
            Thumb control;
            switch (position)
            {
                default:
                case ThumbPosition.Left:
                    control = left;
                    break;
                case ThumbPosition.Top:
                    control = top;
                    break;
                case ThumbPosition.Right:
                    control = right;
                    break;
                case ThumbPosition.Bottom:
                    control = bottom;
                    break;
            }

            control.Effect = effect;
        }

        #region Exposed events
        public event RoutedEventHandler LeftThumbMouseClick;
        public event RoutedEventHandler BottomThumbMouseClick;

        protected void OnBottomThumbMouseClick(RoutedEventArgs e)
        {
            RoutedEventHandler handler = BottomThumbMouseClick;
            if (handler != null) handler(this, e);
        }

        protected void OnLeftThumbMouseClick(RoutedEventArgs e)
        {
            RoutedEventHandler handler = LeftThumbMouseClick;
            if (handler != null) handler(this, e);
        }
        #endregion


        // Helper method to instantiate the corner Thumbs, set the Cursor property, 
        // set some appearance properties, and add the elements to the visual tree.
        void BuildAdornerThumb(ref Thumb cornerThumb, Cursor customizedCursor, ThumbPosition thumbPosition)
        {
            if (cornerThumb != null) return;

            bool multiConnector = ownerElement.Connections.ContainsKey(thumbPosition) ?
                (ownerElement.Connections[thumbPosition].MultiConnector ? true : false) : false;
            string resourceType = multiConnector ? "MultiConnector" : 
                thumbPosition == ThumbPosition.Top ? "IncomingConnector" : "SingleConnector";

            cornerThumb = new Thumb
                              {
                                  Name = ownerElement.Name + "_" + thumbPosition,
                                  Cursor = customizedCursor,
                                  Style = (Style)FindResource(resourceType),
                                  Tag = thumbPosition,
                                  IsHitTestVisible = true
                              };
            cornerThumb.PreviewMouseLeftButtonDown += ThumbMouseLeftButtonDown;
            cornerThumb.PreviewMouseMove += ThumbMouseMove;
            cornerThumb.PreviewMouseLeftButtonUp += ThumbPreviewMouseLeftButtonUp;
            // Set some arbitrary visual characteristics.

            visualChildren.Add(cornerThumb);
        }

        static Path CreateCurve(Color color, int thickness, string label, Point startPoint)
        {
            Path curve = new Path
            {
                Stroke = new SolidColorBrush(color),
                StrokeThickness = thickness,
                Name = label
            };
            Panel.SetZIndex(curve, 0);
            PathGeometry curveGeometry = new PathGeometry();
            PathFigure curveFigure = new PathFigure { StartPoint = startPoint };
            curveGeometry.Figures.Add(curveFigure);
            curve.Data = curveGeometry;
            return curve;
        }


        #region Thumb mouse events

        void UpdateCurve(ThumbPosition thumbPosition, Point currentPosition)
        {
            Point startBezierPoint, endBezierPoint;

            switch (thumbPosition)
            {
                case ThumbPosition.Top:
                    startBezierPoint = new Point(startPoint.X, startPoint.Y - GridCanvas.BezierYOffset);
                    endBezierPoint = new Point(currentPosition.X, currentPosition.Y - GridCanvas.BezierYOffset);
                    break;

                default:
                case ThumbPosition.Bottom:
                    startBezierPoint = new Point(startPoint.X, startPoint.Y + GridCanvas.BezierYOffset);
                    endBezierPoint = new Point(currentPosition.X, currentPosition.Y - GridCanvas.BezierYOffset);
                    break;

                case ThumbPosition.Right:
                    startBezierPoint = new Point(startPoint.X + GridCanvas.BezierYOffset, startPoint.Y);
                    endBezierPoint = new Point(currentPosition.X - GridCanvas.BezierYOffset, currentPosition.Y);
                    break;

                case ThumbPosition.Left:
                    startBezierPoint = new Point(startPoint.X - GridCanvas.BezierYOffset, startPoint.Y);
                    endBezierPoint = new Point(currentPosition.X + GridCanvas.BezierYOffset, currentPosition.Y);
                    break;
            }

            BezierSegment bezierSegment = new BezierSegment(startBezierPoint, endBezierPoint, currentPosition, true);

            Path[] curves = ownerCanvas.Curves.Values.Where(
                path => path.Name.StartsWith(ownerElement.GetLink(thumbPosition).From)).ToArray();

            foreach (PathGeometry curveGeometry in curves.Select(curve => (PathGeometry)curve.Data))
            {
                if (curveGeometry.Figures[0].Segments.Count == 0)
                    curveGeometry.Figures[0].Segments.Add(bezierSegment);
                else
                    curveGeometry.Figures[0].Segments[0] = bezierSegment;
            }
        }

        void ThumbPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDrawCurveMode = false;

            bottom.ReleaseMouseCapture();
            Thumb thumb = (Thumb)sender;
            ThumbPosition thumbPosition = (ThumbPosition)(thumb).Tag;
            
            if (!linkFound)
            {
                foreach (Path curve in ownerCanvas.Curves.Values.Where(
                path => path.Name.StartsWith(ownerElement.GetLink(thumbPosition).From)).ToArray())
                {
                    ownerCanvas.Children.Remove(curve);
                    ownerCanvas.Curves.Remove(curve.Name);
                }

                if (ownerElement.ExistsLink(thumbPosition))
                {
                    ownerCanvas.RemoveLink(ownerElement.GetLink(thumbPosition));
                }
            }
            else
            {
                ownerCanvas.EstablishLink(ownerElement.GetLink(thumbPosition).From,
                    ownerElement, thumbPosition, tempTargetElement, tempTargetPosition);
                linkFound = false;
            }
            ownerCanvas.UnhighlightConnectors(ownerElement, thumbPosition);
        }

        void ThumbMouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawCurveMode)
                return;

            Thumb clickedThumb = (Thumb)sender;
            ThumbPosition thumbPosition = (ThumbPosition)clickedThumb.Tag;
            Point currentPosition = MouseUtilities.CorrectGetPosition(ownerCanvas);

            
            // Auto-link to target
            linkFound = ownerCanvas.HandleCollisions(currentPosition, ownerElement, thumbPosition,
                out tempTargetElement, out tempTargetPosition);

            if (linkFound)
                return;
            // move curve endpoint to mouse position
            UpdateCurve(thumbPosition, currentPosition);

        }

        void ThumbMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            Thumb thumb = (Thumb)sender;
            ThumbPosition thumbPosition = (ThumbPosition)(thumb).Tag;
            if (thumbPosition == ThumbPosition.Top)
                return;

            isDrawCurveMode = true;
            thumb.CaptureMouse();
            
            startPoint = ownerElement.GetCanvasThumbPosition(thumbPosition);
            
            string insideCurveId = ownerElement.GetLink(thumbPosition).From + "_i";
            string outlineCurveId = ownerElement.GetLink(thumbPosition).From + "_o";

            Path curveOutline;
            Path curveInside;

            if (!ownerCanvas.CurveExists(outlineCurveId, out curveOutline))
            {
                curveOutline = CreateCurve(Colors.Black, 6, outlineCurveId, startPoint);
                ownerCanvas.Children.Add(curveOutline);
                ownerCanvas.Curves.Add(curveOutline.Name, curveOutline);
            }
            else
            {
                ((PathGeometry) curveOutline.Data).Figures[0].Segments.Clear();
                ownerCanvas.RemoveLink(ownerElement.GetLink(thumbPosition));
            }

            if (!ownerCanvas.CurveExists(insideCurveId, out curveInside))
            {
                curveInside = CreateCurve(Colors.DarkGray, 4, insideCurveId, startPoint);
                ownerCanvas.Children.Add(curveInside);
                ownerCanvas.Curves.Add(curveInside.Name, curveInside);
            }
            else
            {
                curveInside.Stroke = new SolidColorBrush(Colors.DarkGray);
                ((PathGeometry)curveInside.Data).Figures[0].Segments.Clear();
            }

            ownerCanvas.HighlightAvailableConnectors(ownerElement, thumbPosition);
        }
        #endregion

        protected override Size ArrangeOverride(Size finalSize)
        {
            // desiredWidth and desiredHeight are the width and height of the element that's being adorned.  
            // These will be used to place the ResizingAdorner at the corners of the adorned element.  
            double desiredWidth = AdornedElement.DesiredSize.Width;
            double desiredHeight = AdornedElement.DesiredSize.Height;
            // adornerWidth & adornerHeight are used for placement as well.
            const double adornerWidth = thumbSize;
            const double adornerHeight = thumbSize;

            left.Arrange(new Rect(-adornerWidth / 2, desiredHeight / 2 - adornerHeight / 2, adornerWidth, adornerHeight));
            right.Arrange(new Rect(desiredWidth - adornerWidth / 2, desiredHeight / 2 - adornerHeight / 2, adornerWidth, adornerHeight));
            top.Arrange(new Rect(desiredWidth / 2 - adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            bottom.Arrange(new Rect(desiredWidth / 2 - adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            return finalSize;
        }

        // Override the VisualChildrenCount and GetVisualChild properties to interface with 
        // the adorner's visual collection.
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }

    }
}
