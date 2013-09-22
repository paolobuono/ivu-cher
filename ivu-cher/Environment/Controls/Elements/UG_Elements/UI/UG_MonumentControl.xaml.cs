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
    /// Interaction logic for UG_MonumentControl.xaml
    /// </summary>
    public partial class UG_MonumentControl : UserControl
    {
        public UG_MonumentControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }


        public string NomeMonumento { get { return tbNome.Text; } }
        public string Storia { get { return tbStoria.Text; } }
        public string Latitudine { get { return tbLatitudine.Text; } }
        public string Longitudine { get { return tbLongitudine.Text; } }
        public string Info { get { return tbInfo.Text; } }
        public string Descrizione { get { return tbDescrizione.Text; } }
        public string Tipo { get { return cbTipo.Text; } } 
    }
}
