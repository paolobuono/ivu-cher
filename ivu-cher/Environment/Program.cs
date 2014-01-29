using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AvengersUtd.Explore.Environment.Controls;
using AvengersUtd.Explore.Environment.Windows;
using System.Runtime.InteropServices;
using System.IO;
using AvengersUtd.Explore.Data;
using System.Windows.Forms;

namespace AvengersUtd.Explore.Environment
{
    class Program
    {
        #region Chiavi configurazione
        public const string Section_TemplateConfig = "TEMPLATE CONFIG";

        // Template config.
        public const string TemplateConfig_Directory = "Directory";
        public const string TemplateConfig_AskOnStartup = "AskOnStartup";
        #endregion

        public const string FileName = "\\Explore.ini";
        public static bool AskOnStartup = false;
        [STAThread]
        public static void Main(string[] args)
        {
            App app = new App();
            System.Uri resourceLocater = new System.Uri("/Environment;component/app.xaml", System.UriKind.Relative);
            System.Windows.Application.LoadComponent(app, resourceLocater);

            using (ISplashScreen splashScreen = SplashScreenManager.CreateSplashScreen())
            {
                splashScreen.SetContentObject(typeof(CustomSplashScreen));

                // Perform loading
                Thread.Sleep(1000);
            }
            
            string iniPath = GetIniPath(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Dictionary<string, string> settingsDictionary;

            if (File.Exists(iniPath))
            {
                settingsDictionary = ReadFromIni(iniPath);
                
                DataManager.ResourceFolder = settingsDictionary[TemplateConfig_Directory];
                DataManager.ReCreateResources(false);
                try
                {

                    AskOnStartup = Convert.ToBoolean(settingsDictionary[TemplateConfig_AskOnStartup]);
                    if (AskOnStartup)
                    {
                        Config config = new Config(DataManager.ResourceFolder, AskOnStartup, settingsDictionary, iniPath, app);
                        config.ShowDialog();
                    }
                    else
                    {
                        //MainWindow mainWindow = new MainWindow();
                        Wizard wizard = new Wizard();

                        app.Run(wizard);
                        //app.Run(mainWindow);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(string.Format("Explore.ini in \n {0} \n contains incorrect value",iniPath),"Explore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("File .ini not found.Please contact administrator", "Explore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     
                Application.Exit();
                return;
            }
            
        }
        #region Ini
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// Return .ini file path.
        /// </summary>
        /// <param name="path">Application Exe current path.</param>
        /// <returns>Path .ini file.</returns>
        public static string GetIniPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return FileName;
            return new FileInfo(path).DirectoryName + FileName;
        }

        /// <summary>
        /// Create .ini file empty
        /// </summary>
        /// <param name="path">Path del file di configurazione.</param>
        public static void CreateEmptyFile(string path)
        {
            // Template config.
            WritePrivateProfileString(Section_TemplateConfig, TemplateConfig_Directory, string.Empty, path);  
        }

        /// <summary>
        /// Create dictionary for store .ini values
        /// </summary>
        /// <returns>Dictionary with .ini file values.</returns>
        public static Dictionary<string, string> CreateEmptySettingsDictionary()
        {
            Dictionary<string, string> dicToReturn = new Dictionary<string, string>();

            // Template config.
            dicToReturn.Add(TemplateConfig_Directory, string.Empty);

            return dicToReturn;
        }

        /// <summary>
        /// Read from .ini file.
        /// </summary>
        /// <param name="path">Path .ini file.</param>
        /// <returns>Dictionary with file .ini values.</returns>
        public static Dictionary<string, string> ReadFromIni(string path)
        {
            Dictionary<string, string> dicToReturn = new Dictionary<string, string>();
            StringBuilder value;

            if (File.Exists(path))
            {
                // Template config.
                value = new StringBuilder(255);
                GetPrivateProfileString(Section_TemplateConfig, TemplateConfig_Directory, string.Empty, value, 255, path);
                dicToReturn.Add(TemplateConfig_Directory, value.ToString());
                value = new StringBuilder(255);
                GetPrivateProfileString(Section_TemplateConfig, TemplateConfig_AskOnStartup, string.Empty, value, 255, path);
                dicToReturn.Add(TemplateConfig_AskOnStartup, value.ToString());
            }
            return dicToReturn;
        }

        /// <summary>
        /// Write .ini file from dictionary content.
        /// </summary>
        /// <param name="path">Path .ini file.</param>
        /// <param name="dictionary">Dictionary content for .ini file.</param>
        public static void WriteToIni(string path, Dictionary<string, string> dictionary)
        {
            // Template config.
            WritePrivateProfileString(Section_TemplateConfig, TemplateConfig_Directory, dictionary[TemplateConfig_Directory], path);
            WritePrivateProfileString(Section_TemplateConfig, TemplateConfig_AskOnStartup, dictionary[TemplateConfig_AskOnStartup], path);
             
        }
        #endregion

    }
}
