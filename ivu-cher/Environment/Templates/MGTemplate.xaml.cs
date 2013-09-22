using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AvengersUtd.Explore.Environment.Templates
{
    /// <summary>
    /// Interaction logic for MuseumGuideMain.xaml
    /// </summary>
    public partial class MuseumGuideMain : UserControl
    {
        public MuseumGuideMain() : this("Titolo","Descrizione" ,null)
        {
            Image = (BitmapImage)FindResource("Museum");
            InitializeComponent();
        }

        public MuseumGuideMain(string caption, string description, BitmapImage image)
        {
            Caption = caption;
            Description = description;
            Image = image;
            InitializeComponent();
        }

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public BitmapImage Image
        {
            get { return (BitmapImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption",
                                                                                        typeof(string),
                                                                                        typeof(MuseumGuideMain),
                                                                                        new UIPropertyMetadata(
                                                                                                "Museum guide caption"));
        
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description",
                                                                                        typeof(string),
                                                                                        typeof(MuseumGuideMain),
                                                                                        new UIPropertyMetadata(
                                                                                                "Museum guide description"));

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register
            ("Image",
             typeof(BitmapImage),
             typeof (MuseumGuideMain),
             new PropertyMetadata());
    }
}
