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

namespace AvengersUtd.Explore.Environment.Controls.Elements.UG_Elements.UI
{
    /// <summary>
    /// Interaction logic for UG_CityControl.xaml
    /// </summary>
    public partial class UG_CityControl : UserControl
    {
        public UG_CityControl()
        {
            InitializeComponent();
        }

        public string Nome { get { return tbNome.Text; } }
        public string Descrizione { get { return tbDescrizione.Text; } }

    }
}
