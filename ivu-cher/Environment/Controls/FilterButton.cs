using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using AvengersUtd.Explore.Data.Resources;

namespace AvengersUtd.Explore.Environment.Controls
{
    public class FilterButton : ToggleButton
    {
        readonly Image image = null;
        readonly TextBlock textBlock = null;

        private readonly StackPanel panel;
        private readonly AdornerDecorator decorator;
        private readonly CounterAdorner counter;

        public FilterButton()
        {
            decorator = new AdornerDecorator();

            panel = new StackPanel
                                   {
                                           Orientation = Orientation.Vertical,
                                           Margin = new System.Windows.Thickness(0)
                                   };

            image = new Image {Margin = new System.Windows.Thickness(0)};
            panel.Children.Add(image);

            textBlock = new TextBlock{HorizontalAlignment =HorizontalAlignment.Center};
            panel.Children.Add(textBlock);

            Focusable = false;
            decorator.Child = panel;
            this.Content = decorator;
            AdornerLayer al = decorator.AdornerLayer;
            counter = new CounterAdorner(panel);
            al.Add(counter); 
        }


        public void SetCountDisplay(int number)
        {
            counter.CountDisplay = number.ToString();
        }


        public string Text
        {
            get
            {
                if (textBlock != null)
                    return textBlock.Text;
                else
                    return String.Empty;
            }
            set
            {
                if (textBlock != null)
                    textBlock.Text = value;
            }
        }

        public ImageSource Image
        {
            get
            {
                if (image != null)
                    return image.Source;
                else
                    return null;
            }
            set
            {
                if (image != null)
                    image.Source = value;
            }
        }

        public double ImageWidth
        {
            get
            {
                if (image != null)
                    return image.Width;
                else
                    return double.NaN;
            }
            set
            {
                if (image != null)
                    image.Width = value;
            }
        }

        public double ImageHeight
        {
            get
            {
                if (image != null)
                    return image.Height;
                else
                    return double.NaN;
            }
            set
            {
                if (image != null)
                    image.Height = value;
            }
        }


        public ResourceType ResourceType
        {
            get { return (ResourceType)GetValue(ResourceTypeProperty); }
            set { SetValue(ResourceTypeProperty, value); }
        }

        public static readonly DependencyProperty ResourceTypeProperty = DependencyProperty.Register("ResourceType",
                                                                                typeof(ResourceType),
                                                                                typeof(FilterButton),
                                                                                new UIPropertyMetadata(Data.Resources.ResourceType.Unknown));


    }
}
