using System.Windows;
using System.Windows.Controls;
using AvengersUtd.Explore.Environment.Controls;
using AvengersUtd.Explore.Environment.Controls.Elements;
using System.IO;
using AvengersUtd.Explore.Environment.Templates;

namespace AvengersUtd.Explore.Environment.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Global.WorkArea = workArea;

        }

        public Border WMPreview
        {
            get { return wmPreview.PreviewArea; }
        }

        public WebBrowser WebPreview
        {
            get { return webBrowser; }
        }

        public MTPreviewPanel MtPreview
        {
            get { return mtPreview; }
        }

        public Border ANDPreview
        {
            get { return andPreview.PreviewArea; }
        }

        public Border MAPPreview
        {
            get { return mapPreview.PreviewArea; }
        }
        public bool IsPreviewPanelExpanded
        {
            get { return previewExpander.IsExpanded; }
        }

        public string ActivePreview
        {
            get { return ((string)((TabItem)tabPreview.SelectedItem).Tag); }
        }

        public void InitHistoryPuzzle()
        {
            HistoryPuzzleElement e = new HistoryPuzzleElement
            {
                Caption = "History-Puzzle"
            };

            exportToXml.IsEnabled = true;
            exportToHtml.IsEnabled = false;
            wmTab.IsEnabled = mtTab.IsEnabled = false;
            mtTab.IsEnabled = true;
            tabPreview.SelectedIndex = 2;

            workArea.AddElement(e);
            mtPreview.Content = new HPTemplate();
        }

        internal void InitUrbanGame()
        {
            exportToHtml.IsEnabled = false;
            exportToXml.IsEnabled = true;
            publishXml.IsEnabled = true;
            webTab.IsEnabled = mtTab.IsEnabled = wmTab.IsEnabled = false;
            andTab.IsEnabled = true;
            
            tabPreview.SelectedIndex = 3;
            resPreview.SelectedIndex = 1; 
            workArea.AddElement(new Explore.Environment.Controls.Elements.UG_Elements.UG_CityElement { Caption = "City" });
        }

        public void InitMuseumGuide()
        {
            MuseumGuideElement e = new MuseumGuideElement
            {
                Caption = "Museum Guide"
            };
            exportToXml.IsEnabled = exportToHtml.IsEnabled = true;
            wmTab.IsEnabled = webTab.IsEnabled = true;
            mtTab.IsEnabled = false;
            tabPreview.SelectedIndex = 0;
            workArea.AddElement(e);
        }

        public void TestExplore()
        {
            ExcursionGameElement e = new ExcursionGameElement
            {
                Caption = "Excursion-Game"
            };
            exportToXml.IsEnabled = true;
            exportToHtml.IsEnabled = false;
            wmTab.IsEnabled = true;
            webTab.IsEnabled = mtTab.IsEnabled = false;
            tabPreview.SelectedIndex = 1;
            workArea.AddElement(e);

        }

        protected override void OnActivated(System.EventArgs e)
        {
            base.OnActivated(e);
            Global.MainWindow = this;
        }


        private void PublishToXml_Click(object sender, RoutedEventArgs e)
        {
            PublishTemplate publishWindow = new PublishTemplate();

            // genera template in una forder tmp
            string folderTmp = string.Empty;

            publishWindow.tmpDir = folderTmp;
            publishWindow.ShowDialog();
        }
            
         private void ExportToXml_Click(object sender, RoutedEventArgs e)
        {
            Exporter exporter = new Exporter(workArea);

            string path = null;

            if (Global.Template == Data.Resources.TemplateType.UrbanGame)
            {
                 var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
                 folderDialog.Description = "Select the destination folder of the template.";
                 var dialogres = folderDialog.ShowDialog();
                 if (dialogres != System.Windows.Forms.DialogResult.OK)
                 { MessageBox.Show("Operation aborted."); return; }
                 path = folderDialog.SelectedPath;
            }
            else
            {
                var fileDialog = new Microsoft.Win32.SaveFileDialog();
                fileDialog.Filter = "CHe Xml File|*.xml";
                var result = fileDialog.ShowDialog();
                if (result.HasValue && result.Value)
                    path = fileDialog.SafeFileName;
            }



            if (path!=null)
            {
                bool success = true;
                switch (Global.Template)
                {
                    case Data.Resources.TemplateType.MuseumGuide:
                        exporter.ExportMuseumGuideXML(path);
                        break;
                    case Data.Resources.TemplateType.ExcursionGame:
                        success = exporter.ExportExcursionGameXML(path);
                        break;
                    case Data.Resources.TemplateType.UrbanGame:
                        success = exporter.ExportUrbanGameXML(path);
                        break;
                    default:
                        break;
                }
                if (success)
                    MessageBox.Show("Export operation successfully completed !", "Export", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void exportToHtml_Click(object sender, RoutedEventArgs e)
        {
            Exporter exporter = new Exporter(workArea);
            Microsoft.Win32.SaveFileDialog fileDialog = new Microsoft.Win32.SaveFileDialog();

            fileDialog.Filter = "CHe HTML File|*.html";
            bool? result = fileDialog.ShowDialog();

            if (result.HasValue)
            {
                switch (Global.Template)
                {
                    case Data.Resources.TemplateType.MuseumGuide:
                        exporter.ExportMuseumGuideHTML(
                            Path.GetFileNameWithoutExtension(fileDialog.SafeFileName),
                            Path.GetDirectoryName(fileDialog.FileName));
                        break;
                    default:
                        break;
                }

                MessageBox.Show("Export operation successfully completed !", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }



        private void ReCreateDataFile_Click(object sender, RoutedEventArgs e)
        {
            AvengersUtd.Explore.Data.DataManager.ReCreateResources();
            searchPanel.ResetInterface();

            MessageBox.Show(string.Format("Updated resources index with {0} files", AvengersUtd.Explore.Data.DataManager.Resources.Length));

        }
        private void ViewHelp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "Help.html";
                process.Start();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Help file not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Directory.Delete(Global.ExecutionPath + "\\temp", true);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to quit?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.No)
                e.Cancel = true;
        }
    }
}
