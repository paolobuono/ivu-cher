using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AvengersUtd.Explore.Data.Resources;
using AvengersUtd.Explore.Data;

namespace AvengersUtd.Explore.Environment.Controls
{
    /// <summary>
    /// Interaction logic for SearchPanel.xaml
    /// </summary>
    public partial class SearchPanel : UserControl
    {
        public SearchPanel()
        {
            InitializeComponent();
            ResetInterface();
            //to initialize resources
            
        }

        public void ResetInterface()
        {
            searchInterface.FindResources("");
        }

        public void ClosePopup()
        {
            //popup.IsOpen = false;
        }

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            //if (tbSearch.Text.Length >= 3)
            //    popup.IsOpen = true;

        }

        private void popup_LostFocus(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }

        private void tbSearch_Search(object sender, RoutedEventArgs e)
        {
            //expander.IsExpanded = true;
            searchInterface.FindResources(tbSearch.Text);
        }

    }
}
