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


            if (File.Exists(DataManager.iniPath))
            {
                DataManager.LoadFromIniFile();
                
                try
                {

                    if (DataManager.AskOnStartup)
                    {
                        Config config = new Config(DataManager.ResourceFolder, DataManager.AskOnStartup, DataManager.settingsDictionary, DataManager.iniPath, app);
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

                    MessageBox.Show(string.Format("Explore.ini in \n {0} \n contains incorrect value", DataManager.iniPath), "Explore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                DataManager.CreateDefaultIni();
                
                MessageBox.Show("File .ini created. The application must be restarted", "Explore", MessageBoxButtons.OK, MessageBoxIcon.Question);
                     
                Application.Exit();
                return;
            }
            
        }

    }
}
