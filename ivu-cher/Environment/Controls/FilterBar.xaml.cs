using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AvengersUtd.Explore.Environment.Controls
{
    /// <summary>
    /// Interaction logic for FilterBar.xaml
    /// </summary>
    public partial class FilterBar : UserControl
    {
        
        public FilterBar()
        {
            InitializeComponent();
            foreach (FilterButton toggleButton in buttonPanel.Children)
            {
                toggleButton.IsChecked = true;
            }

        }
        
        public IEnumerable<FilterButton> Filters
        {
            get
            {
                return buttonPanel.Children.OfType<FilterButton>();
            }
        }

        internal UIElementCollection Controls
        {
            get { return buttonPanel.Children; }
        }

     
    }
}
