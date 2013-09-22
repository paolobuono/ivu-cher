using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AvengersUtd.Explore.Environment.Controls;
using AvengersUtd.Explore.Environment.Windows;

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

            //MainWindow mainWindow = new MainWindow();
            Wizard wizard = new Wizard();
            app.Run(wizard);
            //app.Run(mainWindow);
        }

    }
}
