using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using AvengersUtd.Explore.Data.Resources;
using AvengersUtd.Explore.Data.Elements;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    /// <summary>
    /// Interaction logic for Element.xaml
    /// </summary>
    public partial class Element : UserControl
    {
        
        private AdornerLayer adornerLayer;
        private bool isDragInProgress;
        private Point origCursorLocation;
        private double origHorizOffset;
        private double origVertOffset;
        private bool modifyLeftOffset;
        private bool modifyTopOffset;
        private bool collapsed;
        private bool editingCaption;
        public readonly int elementIndex;
      
        internal static int Index;

        #region Properties
        
        public string ElementCode
        {
            get
            {
                return string.Format("{0}{1}", Caption.Replace(' ', '_').Substring(0, 3), elementIndex);
            }
        }

        public IEnumerable<Link> IncomingLinks
        {
            get { return OwnerCanvas.GetIncomingLinks(this); }
        }

        public IEnumerable<Link> OutgoingLinks
        {
            get { return OwnerCanvas.GetOutgoingLinks(this); }
        }


        public new Brush Background
        {
            get { return elementBorder.Background; }
            set { elementBorder.Background = value; }
        }

        public string BuildingBlockLabel
        {
            get { return bbLabel.Text; }
            set { bbLabel.Text = value; }
        }

        public GridCanvas OwnerCanvas
        {
            get;
            internal set;
        }

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set
            {   
                SetValue(CaptionProperty, value);
            }
        }

        protected static void OnCaptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string oldValue = (string)e.OldValue;
            string newValue = (string)e.NewValue;
            Element element = (Element)d;
            if (element.OwnerCanvas != null)
                element.OwnerCanvas.OnElementCaptionChanged(new ElementCaptionArgs(element, oldValue, newValue));
        }

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption",
                                                                                        typeof(string),
                                                                                        typeof(Element),
                                                                                        new UIPropertyMetadata(
                                                                                                "Element caption",OnCaptionChanged));

        protected internal Dictionary<ThumbPosition, ConnectionType> Connections { get; set; }
        protected int ElementHeight { get; set; }
        protected Link LeftLink { get; set; }
        protected Link BottomLink { get; set; }
        protected Link TopLink { get; set; }
        protected Link RightLink { get; set; }   
        protected internal CircleAdorner CircleAdorner { get; private set; }

        protected Border CaptionBorder
        {
            get { return captionBorder; }
        }
        protected ToggleButton CollapseButton
        {
            get { return collapseButton; }
        }

        protected TextBlock CaptionTextBlock
        {
            get { return bCaption; }
        }

        protected Button CloseButton
        {
            get { return closeButton; }
        }

        protected Panel ContentPanel
        {
            get { return contentPanel; }
        }

        protected Label WatermarkLabel
        {
            get { return watermarkLabel; }
        } 
        #endregion

        #region Constructors
        public Element()
        {
            Index++;
            ElementHeight = 192;
            elementIndex = Index;
            InitializeComponent();
            Connections = new Dictionary<ThumbPosition, ConnectionType>();
            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);
            TopLink = new Link(this, ThumbPosition.Top);
            BottomLink = new Link(this, ThumbPosition.Bottom);
            RightLink = new Link(this, ThumbPosition.Right);
            LeftLink = new Link(this, ThumbPosition.Left);
            Collapse();
        } 
        #endregion

        #region Links
        public IEnumerable<Link> Links
        {
            get
            {
                if (ExistsLink(ThumbPosition.Top))
                    yield return TopLink;
                if (ExistsLink(ThumbPosition.Right))
                    yield return RightLink;
                if (ExistsLink(ThumbPosition.Bottom))
                    yield return BottomLink;
                if (ExistsLink(ThumbPosition.Left))
                    yield return LeftLink;
            }
        }

        public Link GetLink(ThumbPosition thumbPosition)
        {
            switch (thumbPosition)
            {
                case ThumbPosition.Left:
                    return LeftLink;

                case ThumbPosition.Top:
                    return TopLink;

                case ThumbPosition.Right:
                    return RightLink;

                case ThumbPosition.Bottom:
                    return BottomLink;

                default:
                    return default(Link);
            }
        }

        public void SetLink(ThumbPosition thumbPosition, Link link)
        {
            switch (thumbPosition)
            {
                case ThumbPosition.Left:
                    LeftLink = link;
                    break;
                case ThumbPosition.Top:
                    TopLink = link;
                    break;
                case ThumbPosition.Right:
                    RightLink = link;
                    break;

                case ThumbPosition.Bottom:
                    BottomLink = link;
                    break;

                default:
                    return;
            }
        }

        
        public void RemoveLink(ThumbPosition thumbPosition)
        {
            Link emptyLink = new Link(this, thumbPosition);

            switch (thumbPosition)
            {
                case ThumbPosition.Left:
                    LeftLink = emptyLink;
                    break;
                default:
                case ThumbPosition.Top:
                    TopLink = emptyLink;
                    break;
                case ThumbPosition.Right:
                    RightLink = emptyLink;
                    break;
                case ThumbPosition.Bottom:
                    BottomLink = emptyLink;
                    break;

            }
        }

        public bool ExistsLink(ThumbPosition thumbPosition)
        {
            switch (thumbPosition)
            {
                case ThumbPosition.Left:
                    return LeftLink.TargetElement != null;
                case ThumbPosition.Top:
                    return TopLink.TargetElement != null;
                case ThumbPosition.Right:
                    return RightLink.TargetElement != null;

                case ThumbPosition.Bottom:
                    return BottomLink.TargetElement != null;
                default:
                    return false;
            }
        }

        void UpdateLinks()
        {
            foreach (Link link in OwnerCanvas.GetOutgoingLinks(this))
            {
                OwnerCanvas.UpdateCurveSource(link.From, this, link.SourcePosition);
            }

            foreach (Link link in OwnerCanvas.GetIncomingLinks(this))
            {
                OwnerCanvas.UpdateCurveTarget(link.From, this, link.TargetPosition);
            }
        }

        #endregion

        #region Connections
        public virtual IEnumerable<ThumbPosition> AvailableIncomingConnectors
        {
            get
            {
                yield return ThumbPosition.Top;
                yield return ThumbPosition.Bottom;
            }
        }

        public bool HasConnectionsAllowedFrom(ThumbPosition source)
        {
            return Connections.ContainsKey(source);
        }

        public bool IsConnectionAllowedTo(ThumbPosition targetPosition, Element targetElement)
        {
            return Connections.Any(c =>
                                    c.Value.TargetPosition == targetPosition && c.Value.TargetTypes.Contains(targetElement.GetType()));
        } 
        #endregion

        #region Children events

        protected virtual void ElementContentLoaded(object sender, RoutedEventArgs e)
        {
            adornerLayer = AdornerLayer.GetAdornerLayer(elementBorder);
            CircleAdorner = new CircleAdorner(elementBorder, this);
            adornerLayer.Add(CircleAdorner);
            CircleAdorner.SetVisibility(ThumbPosition.Left, false);
            CircleAdorner.SetVisibility(ThumbPosition.Right, false);  
            Focus();
        }

        private void CollapseButtonClick(object sender, RoutedEventArgs e)
        {
            if (collapsed)
                Expand();
            else
                Collapse();
        }

        private void bCaption_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                bCaption.Visibility = Visibility.Collapsed;
                tbCaptionHidden.Visibility = Visibility.Visible;
                editingCaption = true;
                tbCaptionHidden.Focus();
                e.Handled = true;

            }
        }

        private void tbCaptionHidden_LostFocus(object sender, RoutedEventArgs e)
        {
            bCaption.Visibility = Visibility.Visible;
            tbCaptionHidden.Visibility = Visibility.Collapsed;
            editingCaption = false;
        }

        private void tbCaptionHidden_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Focus();
            }
        }

        private void tbCaptionHidden_GotFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            tbCaptionHidden.SelectAll();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            OwnerCanvas.RemoveLinksToSource(this);
            OwnerCanvas.RemoveLinksToTarget(this);
            OwnerCanvas.RemoveElement(this);
        }


        protected virtual void infoButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(AvengersUtd.Explore.Environment.Properties.Resources.DebugInfoElement,
                "Debug", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Overriden events
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateLinks();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (editingCaption)
            {
                e.Handled = true;
                return;
            }

            base.OnLostFocus(e);
            captionBorder.Background = (LinearGradientBrush)FindResource("DisabledCaption");
            elementBorder.Opacity = 0.85;
            Panel.SetZIndex(this, 1);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            captionBorder.Background = (LinearGradientBrush)FindResource("ElementCaption");
            elementBorder.Opacity = 1.0;
            Panel.SetZIndex(this, 100);
        }

        #endregion

        #region Thumbs
        public Point GetThumbPosition(ThumbPosition thumbPosition)
        {
            switch (thumbPosition)
            {
                case ThumbPosition.Left:
                    return new Point(0, ActualHeight / 2);
                case ThumbPosition.Top:
                    return new Point(ActualWidth / 2, 0);
                case ThumbPosition.Right:
                    return new Point(ActualWidth, ActualHeight / 2);
                default:
                case ThumbPosition.Bottom:
                    return new Point(ActualWidth / 2, ActualHeight);
            }
        }
                public Point GetCanvasThumbPosition(ThumbPosition thumbPosition)
        {
            return TransformToAncestor(OwnerCanvas).Transform(GetThumbPosition(thumbPosition));
        }

        #endregion

        #region Interactivity
        public void Collapse()
        {
            Height = elementBorder.Height = MinHeight;
            contentPanel.Height = contentPanel.MinHeight;
            contentPanel.Visibility = Visibility.Collapsed;
            watermarkLabel.Visibility = Visibility.Visible;
            collapsed = true;
        }

        public void Expand()
        {
            Height = elementBorder.Height = ElementHeight;
            contentPanel.Height = ElementHeight - 24;
            contentPanel.Visibility = Visibility.Visible;
            watermarkLabel.Visibility = Visibility.Collapsed;
            collapsed = false;
        } 
        #endregion

        #region Dragging code
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            if (editingCaption) return;

            Cursor = Cursors.Hand;
            Focus();
            Panel.SetZIndex(this,100);
            CaptureMouse();
            isDragInProgress = false;

            // Cache the mouse cursor location.
            origCursorLocation = MouseUtilities.CorrectGetPosition(OwnerCanvas);

            // Get the element's offsets from the four sides of the Canvas.
            double left = Canvas.GetLeft(this);
            double right = Canvas.GetRight(this);
            double top = Canvas.GetTop(this);
            double bottom = Canvas.GetBottom(this);

            // Calculate the offset deltas and determine for which sides
            // of the Canvas to adjust the offsets.
            origHorizOffset = ResolveOffset(left, right, out modifyLeftOffset);
            origVertOffset = ResolveOffset(top, bottom, out modifyTopOffset);

            // Set the Handled flag so that a control being dragged 
            // does not react to the mouse input.
            e.Handled = true;

            isDragInProgress = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            // If no element is being dragged, there is nothing to do.
            if (!isDragInProgress)
                return;

            // Get the position of the mouse cursor, relative to the Canvas.
            Point cursorLocation = MouseUtilities.CorrectGetPosition(OwnerCanvas);

            // These values will store the new offsets of the drag element.
            double newHorizontalOffset, newVerticalOffset;

            #region Calculate Offsets

            // Determine the horizontal offset.
            if (modifyLeftOffset)
                newHorizontalOffset = origHorizOffset + (cursorLocation.X - origCursorLocation.X);
            else
                newHorizontalOffset = origHorizOffset - (cursorLocation.X - origCursorLocation.X);

            // Determine the vertical offset.
            if (modifyTopOffset)
                newVerticalOffset = origVertOffset + (cursorLocation.Y - origCursorLocation.Y);
            else
                newVerticalOffset = origVertOffset - (cursorLocation.Y - origCursorLocation.Y);

            #endregion // Calculate Offsets

            #region Verify Drag Element Location

            // Get the bounding rect of the drag element.
            Rect elemRect = CalculateDragElementRect(newHorizontalOffset, newVerticalOffset);

            //
            // If the element is being dragged out of the viewable area, 
            // determine the ideal rect location, so that the element is 
            // within the edge(s) of the canvas.
            //


            bool leftAlign = elemRect.Left < 0;
            bool rightAlign = elemRect.Right > OwnerCanvas.ActualWidth;

            if (leftAlign)
                newHorizontalOffset = modifyLeftOffset ? 0 : OwnerCanvas.ActualWidth - elemRect.Width;
            else if (rightAlign)
                newHorizontalOffset = modifyLeftOffset ? OwnerCanvas.ActualWidth - elemRect.Width : 0;

            bool topAlign = elemRect.Top < 0;
            bool bottomAlign = elemRect.Bottom > OwnerCanvas.ActualHeight;

            if (topAlign)
                newVerticalOffset = modifyTopOffset ? 0 : OwnerCanvas.ActualHeight - elemRect.Height;
            else if (bottomAlign)
                newVerticalOffset = modifyTopOffset ? OwnerCanvas.ActualHeight - elemRect.Height : 0;

            #endregion // Verify Drag Element Location

            #region Move Drag Element

            if (modifyLeftOffset)
                Canvas.SetLeft(this, newHorizontalOffset);
            else
                Canvas.SetRight(this, newHorizontalOffset);

            if (modifyTopOffset)
                Canvas.SetTop(this, newVerticalOffset);
            else
                Canvas.SetBottom(this, newVerticalOffset);

            #endregion // Move Drag Element

            //Update Links
            UpdateLinks();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);

            // Reset the field whether the left or right mouse button was 
            // released, in case a context menu was opened on the drag element.
            if (isDragInProgress)
            {
                isDragInProgress = false;
                ReleaseMouseCapture();
                Cursor = Cursors.Arrow;

                // Update curve position one last time to avoid "drifting"
                foreach (Link incomingLink in IncomingLinks)
                {
                    OwnerCanvas.UpdateCurveTarget(incomingLink.From, incomingLink.TargetElement, incomingLink.TargetPosition);
                }
               
            }
            Panel.SetZIndex(this, 99);
        }

        /// <summary>
        /// Determines one component of a UIElement's location 
        /// within a Canvas (either the horizontal or vertical offset).
        /// </summary>
        /// <param name="side1">
        /// The value of an offset relative to a default side of the 
        /// Canvas (i.e. top or left).
        /// </param>
        /// <param name="side2">
        /// The value of the offset relative to the other side of the 
        /// Canvas (i.e. bottom or right).
        /// </param>
        /// <param name="useSide1">
        /// Will be set to true if the returned value should be used 
        /// for the offset from the side represented by the 'side1' 
        /// parameter.  Otherwise, it will be set to false.
        /// </param>
        private static double ResolveOffset(double side1, double side2, out bool useSide1)
        {
            // If the Canvas.Left and Canvas.Right attached properties 
            // are specified for an element, the 'Left' value is honored.
            // The 'Top' value is honored if both Canvas.Top and 
            // Canvas.Bottom are set on the same element.  If one 
            // of those attached properties is not set on an element, 
            // the default value is Double.NaN.
            useSide1 = true;
            double result;
            if (Double.IsNaN(side1))
            {
                if (Double.IsNaN(side2))
                {
                    // Both sides have no value, so set the
                    // first side to a value of zero.
                    result = 0;
                }
                else
                {
                    result = side2;
                    useSide1 = false;
                }
            }
            else
            {
                result = side1;
            }
            return result;
        }

        /// <summary>
        /// Returns a Rect which describes the bounds of the element being dragged.
        /// </summary>
        private Rect CalculateDragElementRect(double newHorizOffset, double newVertOffset)
        {

            Size elemSize = RenderSize;

            double x, y;

            if (modifyLeftOffset)
                x = newHorizOffset;
            else
                x = ActualWidth - newHorizOffset - elemSize.Width;

            if (modifyTopOffset)
                y = newVertOffset;
            else
                y = ActualHeight - newVertOffset - elemSize.Height;

            var elemLoc = new Point(x, y);

            return new Rect(elemLoc, elemSize);
        }

        #endregion

        

        public override string ToString()
        {
            return Caption;
        }


    }
}
