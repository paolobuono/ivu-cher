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
    /// Interaction logic for PuzzleProperties.xaml
    /// </summary>
    public partial class PuzzleProperties : UserControl
    {
        public PuzzleProperties()
        {
            InitializeComponent();
        }

        public int Rows
        {
            get { return (int)tbRows.Value; }
        }

        public int Columns
        {
            get { return (int)tbColumns.Value; }
        }
    }

    
}
