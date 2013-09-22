using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AvengersUtd.Explore.Environment.Controls
{
    public class ImageButton : Button
    {
        readonly Image image = null;
        readonly TextBlock textBlock = null;

        public ImageButton()
        {
            StackPanel panel = new StackPanel
                                   {
                                           Orientation = Orientation.Vertical,
                                           Margin = new System.Windows.Thickness(0)
                                   };

            image = new Image {Margin = new System.Windows.Thickness(0)};
            panel.Children.Add(image);

            textBlock = new TextBlock{HorizontalAlignment = HorizontalAlignment.Center};
            panel.Children.Add(textBlock);

            this.Content = panel;
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
    }
}
