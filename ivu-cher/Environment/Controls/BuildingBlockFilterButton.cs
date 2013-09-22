using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text;
using System.Windows; 
using System.Windows.Controls; 
using System.Windows.Controls.Primitives; 
using System.Windows.Documents; 
using System.Windows.Input;
using System.Windows.Media; 
using AvengersUtd.Explore.Data.Elements;
using AvengersUtd.Explore.Data.Resources;
using AvengersUtd.Explore.Environment.Controls.Elements;
using System.Windows.Media.Imaging;

namespace AvengersUtd.Explore.Environment.Controls
{
    /// <summary>
    /// Interaction logic for BuildingBlockFilterButton.xaml
    /// </summary>
    public class BuildingBlockFilterButton : ToggleButton
    {  
        StackPanel panel; 
        private Point dragStartPosition;
        protected internal BuildingBlockFilterBar OwnerToolbar { get; set; }
        readonly String resKey;

        public BuildingBlockFilterButton(String resKeyParam)
        {
            resKey = resKeyParam;  
            Focusable = false; 
             
            SetButtonImage(resKey);
             
            this.Checked += new RoutedEventHandler(BuildingBlockFilterButton_Checked); 
            Global.WorkArea.MouseLeftButtonUp += new MouseButtonEventHandler(WorkArea_MouseLeftButtonUp);
        }

        private void SetButtonImage(String vKeyImage)
        { 
            panel = new StackPanel(); 

            Image vImage = new System.Windows.Controls.Image(); 
            vImage.Source = (BitmapImage)FindResource(vKeyImage);

            panel.Children.Add(vImage);

            panel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            panel.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            this.Content = panel;
        }

        void BuildingBlockFilterButton_MouseLeave(object sender, MouseEventArgs e)
        {
            SetButtonImage(resKey);
            var toolTip1 = new System.Windows.Forms.ToolTip();

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.IsBalloon = true;
            toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            toolTip1.ToolTipTitle = "Title:";

        }  
                
        void BuildingBlockFilterButton_Checked(object sender, RoutedEventArgs e)
        {
            if (IsChecked.HasValue && IsChecked.Value)
            {
                Global.WorkArea.DesignMode = true;
                Global.WorkArea.Cursor = Cursors.Cross;
            }
        }

        public ElementType ElementType
        {
            get { return (ElementType)GetValue(ElementTypeProperty); }
            set { SetValue(ElementTypeProperty, value); }
        }

        public static readonly DependencyProperty ElementTypeProperty = DependencyProperty.Register("ElementType",
                                                                                typeof(ElementType),
                                                                                typeof(BuildingBlockFilterButton),
                                                                                new UIPropertyMetadata(ElementType.Unknown));

        void WorkArea_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!Global.WorkArea.DesignMode || (IsChecked.HasValue && !IsChecked.Value))
                return;

            Point p = MouseUtilities.CorrectGetPosition(Global.WorkArea);

            Global.WorkArea.AddElement(ElementType, p);

            IsChecked = false;

            Global.WorkArea.DesignMode = false;
            Global.WorkArea.Cursor = Cursors.Arrow;
        } 

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
                DataObject dragData = new DataObject("ElementType", ElementType);
                DragDrop.DoDragDrop(this, dragData, DragDropEffects.Copy);
                IsChecked = false;
                OwnerToolbar.IsDragInProgress = false;
                Global.WorkArea.Cursor = Cursors.Arrow;
            }
        } 

        public BitmapImage Image
        {
            get { return (BitmapImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(BitmapImage), typeof(BuildingBlockFilterButton), new PropertyMetadata(null));
         
        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(BuildingBlockFilterButton), new UIPropertyMetadata(16d));

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight", typeof(double), typeof(BuildingBlockFilterButton), new UIPropertyMetadata(16d));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(BuildingBlockFilterButton), new UIPropertyMetadata(""));

    }

}
