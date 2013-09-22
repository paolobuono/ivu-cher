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
    public partial class UGTemplateContenuti : UserControl
    {
        public UGTemplateContenuti()
            : this("Titolo", "Descrizione", null)
        {   // TODO: impostare imm icon UrbanGame
            Image = (BitmapImage)FindResource("tmplAndContenuti");
            InitializeComponent();
        }

        public UGTemplateContenuti(string caption, string description, BitmapImage image)
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
                                                                                        typeof(UGTemplateContenuti),
                                                                                        new UIPropertyMetadata(
                                                                                                "Urban game caption"));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description",
                                                                                        typeof(string),
                                                                                        typeof(UGTemplateContenuti),
                                                                                        new UIPropertyMetadata(
                                                                                                "Urban game description"));

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register
            ("Image",
             typeof(BitmapImage),
             typeof(UGTemplateContenuti),
             new PropertyMetadata());
    }
}
