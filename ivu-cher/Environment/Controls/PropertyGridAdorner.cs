using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using AvengersUtd.Explore.Environment.Controls.Elements;

namespace AvengersUtd.Explore.Environment.Controls
{
    public class PropertyGridAdorner : Adorner
    {
        readonly VisualCollection visualChildren;
        private readonly GridCanvas ownerCanvas;
        private readonly Element ownerElement;
        private DockPanel dockPanel;
        private WpfPropertyGrid wpfPropertyGrid;

        public PropertyGridAdorner(UIElement adornedElement) : base(adornedElement)
        {
            ownerElement = ((Element)adornedElement);
            ownerCanvas = (GridCanvas)ownerElement.OwnerCanvas;
            visualChildren = new VisualCollection(this);

            BuildPropertyGrid();
        }

        void BuildPropertyGrid()
        {
            dockPanel = new DockPanel {LastChildFill = true, Width = 200, Height = 400,};
            wpfPropertyGrid = new WpfPropertyGrid{SelectedObject = AdornedElement, HelpVisible = true};
            dockPanel.Children.Add(wpfPropertyGrid);
            visualChildren.Add(dockPanel);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            const int padding = 4;
            double controlWidth = AdornedElement.DesiredSize.Width;
            double controlHeight = AdornedElement.DesiredSize.Height;
            dockPanel.Arrange(new Rect(controlWidth + padding, 0, dockPanel.Width, dockPanel.Height));
            return finalSize;
        }

        // Override the VisualChildrenCount and GetVisualChild properties to interface with 
        // the adorner's visual collection.
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
    }
}
