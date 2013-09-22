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

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    /// <summary>
    /// Interaction logic for QuestProperties.xaml
    /// </summary>
    public partial class QuestProperties : UserControl
    {
        public QuestProperties()
        {
            InitializeComponent();
        }

        public string PlaceId
        {
            get
            {
                return placeId.Text;
            }
        }
    }
}
