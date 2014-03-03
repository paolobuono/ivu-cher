using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using System.Windows.Threading;
using AvengersUtd.Explore.Environment;
using AvengersUtd.Explore.Environment.Controls.Elements.UG_Elements.UI;
using AvengersUtd.Explore.Environment.GeocodeService;
using AvengersUtd.Explore.Environment.ImageryService;
using AvengersUtd.Explore.Environment.Windows; 
using Microsoft.Maps.MapControl.WPF;
using System.Reflection;
using System.Net;

namespace AvengersUtd.Explore.Environment.Controls
{
    /// <summary>
    /// Interaction logic for MapPreviewPanel.xaml
    /// </summary>
    public partial class MapPreviewPanel : UserControl
    {
        public ObservableCollection<GeocodeService.GeocodeResult> m_SearchResults = new ObservableCollection<GeocodeService.GeocodeResult>();

        private const string m_BingKey = "AjgMjZ34niVS6kDDGkgT7rBzvy5ZhvDoN0cJUKgbySksGtbZzd_SlGBVFftl93j7"; 

        private bool m_Refresh = false;
        private string m_Longitude = string.Empty;
        private string m_Latitude = string.Empty;
        private string vPosition;
        List<LastPinAdded> vListLastPins = new List<LastPinAdded>();
        private Assembly thisExe;
      
        #region Constructor
        public MapPreviewPanel()
        {
            InitializeComponent(); 

            ObjectForScriptingHelper helper = new ObjectForScriptingHelper(this);
            wbMain.ObjectForScripting = helper;
            wbMain.LoadCompleted += new LoadCompletedEventHandler(OnLoadCompleted);
             
            thisExe = System.Reflection.Assembly.GetExecutingAssembly();

            LoadPage();
        }
        #endregion
        
        private void LoadPage()
        {
            // Load the specified HTML file which is marked as the project's embedded resource.
            Stream source = Assembly.GetExecutingAssembly().GetManifestResourceStream("AvengersUtd.Explore.Environment.BingMapPage.htm");
            wbMain.NavigateToStream(source);
        }

        private void OnLoadCompleted(object sender, NavigationEventArgs e)
        {
            try
            {                
                wbMain.InvokeScript("SetKey", new object[] { m_BingKey });
                wbMain.InvokeScript("GetMap", new object[] { });


                string encoded = string.Empty;
                using (System.IO.Stream file = thisExe.GetManifestResourceStream("AvengersUtd.Explore.Environment.Icons.pinPosition.gif"))
                {
                    encoded = Base64Encode(file);
                }
                wbMain.InvokeScript("SetPinIcon", new object[] { encoded }); 

                btnSearch.IsEnabled = true; 
                btnPinDelete.IsEnabled = true;
                lwResults.IsEnabled = true;

                if (m_Refresh)
                {
                    if (!string.IsNullOrEmpty(m_Latitude) && !string.IsNullOrEmpty(m_Longitude))
                    {
                        wbMain.InvokeScript("SetLatLong", new object[] { m_Latitude, m_Longitude });
                        wbMain.InvokeScript("SetPin", new object[] { m_Latitude, m_Longitude, 0, "" });
                    }
                    m_Refresh = false;
                }
                btnSendCoords.IsEnabled = false;
                btnSendCoords.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception)
            {
            }
        }

        public ObservableCollection<GeocodeService.GeocodeResult> SearchResults
        {
            get { return m_SearchResults; }
        }

        private void StartSearch(string address)
        {
            GeocodeRequest geocodeRequest = new GeocodeRequest();

            // Set the credentials using a valid Bing Maps key
            geocodeRequest.Credentials = new Credentials();
            geocodeRequest.Credentials.ApplicationId = m_BingKey;

            // Set the full address query
            geocodeRequest.Query = address;

            // Set the options to only return high confidence results 
            ConfidenceFilter[] filters = new ConfidenceFilter[1];
            filters[0] = new ConfidenceFilter();
            filters[0].MinimumConfidence = GeocodeService.Confidence.High;

            // Add the filters to the options
            GeocodeOptions geocodeOptions = new GeocodeOptions();
            geocodeOptions.Filters = filters;
            geocodeRequest.Options = geocodeOptions;

            ServicePointManager.UseNagleAlgorithm = true;
            // Switch off 100 continue expectation
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.DefaultConnectionLimit = ServicePointManager.DefaultPersistentConnectionLimit;

            // Make the geocode request
            m_SearchResults.Clear();
            try
            {
                GeocodeServiceClient geocodeService = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
                GeocodeResponse geocodeResponse = geocodeService.Geocode(geocodeRequest);

                foreach (GeocodeService.GeocodeResult r in geocodeResponse.Results)
                {
                    m_SearchResults.Add(r);
                    lwResults.SelectedIndex = 0;
                    btnSendCoords.IsEnabled = (m_SearchResults.Count > 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public Border PreviewArea
        {
            get { return previewArea; }
        }
        
        public void ActionChanged(string action)
        {
            if (action == "pin")
                btnPin.IsChecked = true;

        }

        public void PinAdded(string latitude, string longitude)
        {   
            m_Latitude = latitude.Replace(",", ".");
            m_Longitude = longitude.Replace(",", ".");

            GeocodeService.GeocodeResult res = lwResults.SelectedItem as GeocodeService.GeocodeResult;

            if (res != null)
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.InstalledUICulture;
                System.Globalization.NumberFormatInfo ni = (System.Globalization.NumberFormatInfo)ci.NumberFormat.Clone();

                ni.NumberDecimalSeparator = ".";
                ni.NumberGroupSeparator = ",";

                string lat = res.Locations[0].Latitude.ToString(ni);
                string longi = res.Locations[0].Longitude.ToString(ni);

                try
                {
                    btnSendCoords.IsEnabled = true;
                    btnSendCoords.Visibility = System.Windows.Visibility.Visible;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (latitude.Contains(','))
            {
                AddressFromLatLng(latitude, longitude);

                if (!string.IsNullOrWhiteSpace(vPosition))
                {
                    if (vListLastPins.Count > 0)
                    {
                        wbMain.InvokeScript("ClearPins", new object[] { });
                        vListLastPins.Clear(); 
                    }
                    wbMain.InvokeScript("SetPin", new object[] { m_Latitude, m_Longitude, 17, vPosition }); 
                }
            }

        }

        private string AddressFromLatLng(string pLatitude, string pLongitude)
        {
            GeocodeService.ReverseGeocodeRequest reverseGeocodeRequest = new GeocodeService.ReverseGeocodeRequest();

            // Set the credentials using a valid Bing Maps key
            reverseGeocodeRequest.Credentials = new Credentials();
            reverseGeocodeRequest.Credentials.ApplicationId = m_BingKey;

            // Set the point to use to find a matching address
            GeocodeService.Location point = new GeocodeService.Location();
            point.Latitude = Convert.ToDouble(pLatitude);
            point.Longitude = Convert.ToDouble(pLongitude);

            reverseGeocodeRequest.Location = point;

            // Make the reverse geocode request
            GeocodeService.GeocodeServiceClient geocodeService =
            new GeocodeService.GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
            GeocodeService.GeocodeResponse geocodeResponse = geocodeService.ReverseGeocode(reverseGeocodeRequest);

            vPosition = geocodeResponse.Results[0].DisplayName;

            if (vPosition != null)             
                StartSearch(vPosition);
            
            return vPosition;
        }

        private string Base64Encode(System.IO.Stream fs)
        {
            byte[] filebytes = new byte[fs.Length];
            fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
            string encodedData =
                Convert.ToBase64String(filebytes,
                                       Base64FormattingOptions.InsertLineBreaks);
            return encodedData;
        }
        
        private string RequestImageMetadata(string locationString)
        {
            string results = "";

            ImageryMetadataRequest metadataRequest = new ImageryMetadataRequest();

            // Set credentials using a valid Bing Maps key
            metadataRequest.Credentials = new Credentials();
            metadataRequest.Credentials.ApplicationId = m_BingKey;

            // Set the imagery metadata request options
            ImageryService.Location centerLocation = new ImageryService.Location();
            string[] digits = locationString.Split(',');
            NumberFormatInfo nfi = NumberFormatInfo.InvariantInfo;
            centerLocation.Latitude = Convert.ToDouble(digits[0].Replace(",", nfi.NumberDecimalSeparator), nfi);
            centerLocation.Longitude = Convert.ToDouble(digits[1].Replace(",", nfi.NumberDecimalSeparator), nfi);

            metadataRequest.Options = new ImageryMetadataOptions();
            metadataRequest.Options.Location = centerLocation;
            metadataRequest.Options.ZoomLevel = 10;
            metadataRequest.Style = MapStyle.AerialWithLabels;

            // Make the imagery metadata request 
            ImageryServiceClient imageryService = new ImageryServiceClient();
            ImageryMetadataResponse metadataResponse =
              imageryService.GetImageryMetadata(metadataRequest);

            ImageryMetadataResult result = metadataResponse.Results[0];
            if (metadataResponse.Results.Length > 0)
                results = String.Format("Uri: {0}\n", result.ImageUri);            //Vintage: {1} to {2}\nZoom Levels: {3} to {4}",
            else
                results = "Metadata is not available";
            
            return results;
        }
        
        public void RetrieveCoordsFromMousePosition()
        {
            //If Pin is checked, add pin at mouse coords
            if (btnPin.IsChecked.Value)
            {
                Point mousePosition = Mouse.GetPosition(this);
                //Convert the mouse coordinates to a locatoin on the map
                Microsoft.Maps.MapControl.WPF.Location pinLocation = myMap.ViewportPointToLocation(mousePosition);

                // The pushpin to add to the map.
                Microsoft.Maps.MapControl.WPF.Pushpin pin = new Microsoft.Maps.MapControl.WPF.Pushpin();
                pin.Location = pinLocation;

                // Adds the pushpin to the map.
                myMap.Children.Add(pin);
                vListLastPins.Add(new LastPinAdded()
                {
                    _Latitude = pin.Location.Latitude.ToString(),
                    _Longitude = pin.Location.Longitude.ToString()
                });
                btnSendCoords.IsEnabled = true;
                btnSendCoords.Visibility = System.Windows.Visibility.Visible;
                btnPin.IsChecked = false;
            }
        }
        
        #region Events
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
                return;

            string address = txtSearch.Text;

            StartSearch(address);
        }

        private void lwResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //AddPinFromResults();
            
        }

        //private void lwResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    AddPinFromResults();
        //}

        private void AddPinFromResults()
        {
            GeocodeService.GeocodeResult res = lwResults.SelectedItem as GeocodeService.GeocodeResult;
            if (res != null)
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.InstalledUICulture;
                System.Globalization.NumberFormatInfo ni = (System.Globalization.NumberFormatInfo)ci.NumberFormat.Clone();

                ni.NumberDecimalSeparator = ".";
                ni.NumberGroupSeparator = ",";

                string lat = res.Locations[0].Latitude.ToString(ni);
                string longi = res.Locations[0].Longitude.ToString(ni);
                try
                {

                    wbMain.InvokeScript("SetLatLong", new object[] { lat, longi });
                    wbMain.InvokeScript("SetPin", new object[] { lat, longi, 17, res.DisplayName });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        } 

        private void btnPin_Checked(object sender, RoutedEventArgs e)
        {
            wbMain.InvokeScript("SetAction", new object[] { "pin" });
        }

        private void btnPinDelete_Click(object sender, RoutedEventArgs e)
        {
            wbMain.InvokeScript("ClearPins", new object[] { });
            m_Longitude = string.Empty;
            m_Latitude = string.Empty;
        }
        
        private void btnSendCoords_Click(object sender, RoutedEventArgs e)
        {
            UG_MonumentControl ugMonument = null;
            Window window = MainWindow.GetWindow(this);
            GridCanvas gridCanvas = (((MainWindow)(window)).workArea) as GridCanvas;

            IEnumerable<Elements.Element> vResults = gridCanvas.Elements.Where(x => x.Caption.Equals("Monument"));

            if (vResults.Count() == 0)
            {
                MessageBox.Show("No one Monument Building Block, pleass add it", "Explore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            else 
                ugMonument = (((AvengersUtd.Explore.Environment.Controls.Elements.UG_Elements.UG_ContentItem)(vResults.First()))).mControl;                    
            
            if (ugMonument == null)
                return;
            
            if((GeocodeResult)lwResults.SelectedItem == null)
            {
                MessageBox.Show("No coordinates for address specified", "Explore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ugMonument.tbNome.Text = ((GeocodeResult)(lwResults.SelectedItem)).DisplayName;
            ugMonument.tbLatitudine.Text = m_Latitude;
            ugMonument.tbLongitudine.Text = m_Longitude;
        }
        #endregion

        private void lwResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddPinFromResults();
        }

        private void myMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
                return;

            string address = txtSearch.Text;

            StartSearch(address);
        }


    }

    public class LastPinAdded
    {
        public string _Address {get; set;}
        public string _Latitude {get; set;}
        public string _Longitude {get; set;}

    }

}

