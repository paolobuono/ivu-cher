using System;
using System.Threading;
using System.Windows.Threading;
using SplashScreen.Wpf;

namespace AvengersUtd.Explore.Environment.Controls
{
    public class SplashScreenManager
    {
        private static readonly object mutex = new object();

        public static ISplashScreen CreateSplashScreen()
        {
            lock (mutex)
            {
                SplashScreenWindowViewModel vm = new SplashScreenWindowViewModel();

                AutoResetEvent ev = new AutoResetEvent(false);

                Thread uiThread = new Thread(() =>
                {
                    vm.Dispatcher = Dispatcher.CurrentDispatcher;
                    ev.Set();

                    Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate()
                    {
                        SplashScreenWindow splashScreenWindow = new SplashScreenWindow();
                        splashScreenWindow.DataContext = vm;
                        splashScreenWindow.Show();
                    });

                    Dispatcher.Run();
                });

                uiThread.SetApartmentState(ApartmentState.STA);
                uiThread.IsBackground = true;
                uiThread.Start();
                ev.WaitOne();

                return vm;
            }
        }

    }
}
