using System;
using System.Collections.Generic;
using System.IO;
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
using AvengersUtd.Explore.Data.Resources;
using AvengersUtd.Explore.Environment.Controls;
using AvengersUtd.Explore.Data;

namespace AvengersUtd.Explore.Environment.Controls
{
    /// <summary>
    /// Interaction logic for SearchInterface.xaml
    /// </summary>
    /// 
    
    public partial class SearchInterface : UserControl
    {

        private List<ResourceType> filteredTypes = new List<ResourceType>();

        private Point dragStartPosition;
        private readonly Dictionary<ResourceType, BitmapImage> imageTypes;

        IEnumerable<AbstractResource> data;

        public SearchInterface()
        {
            InitializeComponent();
            tabControl.Visibility = System.Windows.Visibility.Collapsed;
            imageTypes = new Dictionary<ResourceType, BitmapImage>
                         {
                             {ResourceType.Image, (BitmapImage) FindResource("Pictures")},
                             {ResourceType.Audio, (BitmapImage) FindResource("Sound")},
                             {ResourceType.Model, (BitmapImage) FindResource("3D")},
                             {ResourceType.Text, (BitmapImage) FindResource("Documents")},
                             {ResourceType.Video, (BitmapImage) FindResource("Video")}
                         };

            foreach (FilterButton filterButton in filterBar.Filters)
            {
                filterButton.Click += new RoutedEventHandler(filterButton_Click);
                filteredTypes.Add(filterButton.ResourceType);
            }
        }

        void filterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterButton filterButton = (FilterButton)sender;
            if (!filterButton.IsChecked.Value)
                 filteredTypes.RemoveAll(x => x == filterButton.ResourceType);
                //results.ItemsSource = data.Where(item => item.Type != filterButton.ResourceType);
            else
                filteredTypes.Add(filterButton.ResourceType);
               
                //results.ItemsSource = results.ItemsSource.Cast<AbstractResource>().Concat(data.Where(item=> item.Type == filterButton.ResourceType));
            results.ItemsSource = data.Where(i => filteredTypes.Contains(i.Type));
        }

        public event EventHandler<SearchEventArgs> SearchCompleted;

        protected void OnSearchCompleted(SearchEventArgs e)
        {
            UpdateCounters(e);

            EventHandler<SearchEventArgs> handler = SearchCompleted;
            if (handler != null) handler(this, e);
        }

        private void UpdateCounters(SearchEventArgs e)
        {
            foreach (FilterButton imageToggleButton in filterBar.Filters)
            {
                FilterButton filter = imageToggleButton;
                int count = e.Data.Count(item => item.Type == filter.ResourceType);

                filter.SetCountDisplay(count);
            }
        }

        public void FindResources(string searchTerm)
        {
            string searchTermLower = searchTerm.ToLowerInvariant();
            
            data = DataManager.Resources.Where(item => (item.Title.ToLowerInvariant().Contains(searchTermLower) || item.Tags.ToLowerInvariant().Contains(searchTermLower)));

            results.ItemsSource = data;
            this.DataContext = data;
            
            OnSearchCompleted(new SearchEventArgs(data.AsQueryable()));
        }
     
        private void StackPanelPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            dragStartPosition = MouseUtilities.CorrectGetPosition(this);
            tabControl.Visibility = Visibility.Visible;
        }

        private void StackPanel_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            Point currentPosition = MouseUtilities.CorrectGetPosition(this);
            Vector diff = dragStartPosition - currentPosition;

            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance &&
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                DataObject dragData = new DataObject("Resource", results.SelectedItem);
                DragDrop.DoDragDrop(this, dragData, DragDropEffects.Copy);
                Global.WorkArea.Cursor = Cursors.Arrow;
            }
        }

        private void results_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (results.SelectedItem == null)
                return;

            AbstractResource item = (AbstractResource)results.SelectedItem;

            switch (item.Type)
            {
                case ResourceType.Image:
                    imagePanel.ClearValue(Image.WidthProperty);
                    break;

                default:
                    imagePanel.Width = 64;
                    break;
            }

            DataContext = item;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AbstractResource resource = ((AbstractResource)results.SelectedItem);
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = ((AbstractResource)results.SelectedItem).Uri.LocalPath;
                process.Start();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File path specified for resource \"" + resource.Title + "\" was not found.\nFilename: " + resource.UriString,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }       

    }
}
