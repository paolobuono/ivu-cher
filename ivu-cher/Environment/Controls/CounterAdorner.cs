using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace AvengersUtd.Explore.Environment.Controls
{
    public class CounterAdorner : Adorner
    {
        private UIElement ownerElement;
        readonly VisualCollection visualChildren;
        private readonly Label counter;
        public CounterAdorner(UIElement adornedElement) : base(adornedElement)
        {
            ownerElement = adornedElement;
            visualChildren = new VisualCollection(this);
            counter = new Label { Style = (Style)FindResource("CounterStyle"),
            Content=null};
            visualChildren.Add(counter);
        }

        public string CountDisplay
        {
            get { return counter.Content.ToString(); }
            set { counter.Content = value; }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            const int offset = 2;
            double desiredWidth = AdornedElement.DesiredSize.Width;
            double desiredHeight = AdornedElement.DesiredSize.Height;
            // adornerWidth & adornerHeight are used for placement as well.

            counter.Measure(new Size(counter.MaxWidth, counter.MaxHeight));
            double adornerWidth = counter.DesiredSize.Width;
            double adornerHeight = counter.DesiredSize.Height;

            counter.Arrange(new Rect(desiredWidth - adornerWidth - offset,
                                     desiredHeight - adornerHeight - offset,
                                     adornerWidth,
                                     adornerHeight));

            return finalSize;
        }

        // Override the VisualChildrenCount and GetVisualChild properties to interface with 
        // the adorner's visual collection.
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }

       
    }
}
