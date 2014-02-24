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
using System.Windows.Shapes;
using System.Windows.Forms;
using System.ComponentModel;
using AvengersUtd.Explore.Environment.Windows;
using AvengersUtd.Explore.Data;

namespace AvengersUtd.Explore.Environment.Controls
{
    /// <summary>
    /// Interaction logic for Config.xaml
    /// </summary>
    public partial class Config : Window
    {
        private string ResourceFolder;
        private bool? AskOnStartup;
        private Dictionary<string, string> SettingsDictionary;
        private string IniPath; 
        private App App;
        
        public Config(string resourceFolder, bool askOnStartup, Dictionary<string, string> settingsDictionary, string iniPath, App app)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.ResourceFolder = this.tbDirectory.Text = resourceFolder;
            this.AskOnStartup = askOnStartup;
            this.SettingsDictionary = settingsDictionary;
            this.IniPath = iniPath;
            this.chkAskOnStartup.IsChecked = AskOnStartup;
            this.App = app;
        } 
    
        private void btnFolderBrowser_Click(object sender, RoutedEventArgs e)
        {
            DataManager.ReCreateResources();
            if(!string.IsNullOrEmpty(DataManager.ResourceFolder))
                tbDirectory.Text = DataManager.ResourceFolder;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            SettingsDictionary[DataManager.TemplateConfig_Directory] = tbDirectory.Text;
            SettingsDictionary[DataManager.TemplateConfig_AskOnStartup] = chkAskOnStartup.IsChecked.Value.ToString();

            DataManager.WriteToIni(IniPath, SettingsDictionary); 

            //MainWindow mainWindow = new MainWindow();
            Wizard wizard = new Wizard();

            this.Close();
            App.Run(wizard);
            //app.Run(mainWindow);
        }

    }
}
