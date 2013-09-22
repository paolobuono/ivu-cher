using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using AvengersUtd.Explore.Data.Elements;
using AvengersUtd.Explore.Environment.Controls.Elements;

namespace AvengersUtd.Explore.Environment.Controls
{
    public class ElementButton : ToggleButton
    {
        private Point dragStartPosition;
        protected internal ToolBar OwnerToolbar { get; set; }

        public ElementButton()
        {
            this.Checked += new RoutedEventHandler(BuildingBlockFilterButtonChecked);
            Global.WorkArea.MouseLeftButtonUp += new MouseButtonEventHandler(WorkAreaMouseLeftButtonUp);
        }

        public ElementType ElementType
        {
            get { return (ElementType)GetValue(ElementTypeProperty); }
            set { SetValue(ElementTypeProperty, value); }
        }

        public static readonly DependencyProperty ElementTypeProperty = DependencyProperty.Register("ElementType",
                                                                                typeof(ElementType),
                                                                                typeof(ElementButton),
                                                                                new UIPropertyMetadata(ElementType.Unknown));

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            
            dragStartPosition = MouseUtilities.CorrectGetPosition(this);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (e.LeftButton != MouseButtonState.Pressed || OwnerToolbar.IsDragInProgress)
                return;

            Point currentPosition = MouseUtilities.CorrectGetPosition(this);
            Vector diff = dragStartPosition - currentPosition;

            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance &&
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                OwnerToolbar.IsDragInProgress = true;
                IsChecked = true;
                DataObject dragData = new DataObject("ElementType",ElementType);
                DragDrop.DoDragDrop(this, dragData, DragDropEffects.Copy);
                IsChecked = false;
                OwnerToolbar.IsDragInProgress = false;
                Global.WorkArea.Cursor = Cursors.Arrow;
            }
        }

        
        void BuildingBlockFilterButtonChecked(object sender, RoutedEventArgs e)
        {
            if (IsChecked.HasValue && IsChecked.Value)
            {
                Global.WorkArea.DesignMode = true;
                Global.WorkArea.Cursor = Cursors.Cross;
            }
        }

        void WorkAreaMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(!Global.WorkArea.DesignMode || (IsChecked.HasValue && !IsChecked.Value))
                return;

            Point p = MouseUtilities.CorrectGetPosition(Global.WorkArea);

            Global.WorkArea.AddElement(ElementType, p);

            IsChecked = false;

            Global.WorkArea.DesignMode = false;
            Global.WorkArea.Cursor = Cursors.Arrow;
        }
    }
}
