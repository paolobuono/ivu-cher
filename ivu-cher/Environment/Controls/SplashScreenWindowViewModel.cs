using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Threading;
using AvengersUtd.Explore.Environment.Controls;

namespace SplashScreen.Wpf
{
    internal class SplashScreenWindowViewModel : ISplashScreen, INotifyPropertyChanged
    {
        private string message;
        private object content;

        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                if (Message == value) return;
                message = value;
                OnPropertyChanged("Message");
            }
        }

        public object Content
        {
            get
            {
                return content;
            }
            set
            {
                if (Content == value) return;
                content = value;
                OnPropertyChanged("Content");
            }
        }

        public void SetContentObject(Type contentType)
        {
            Dispatcher.BeginInvoke((Action<Type>)delegate(Type input)
            {
                object result = Activator.CreateInstance(input);
                Content = result;
            }, contentType);
        }

        public Dispatcher Dispatcher
        {
            get;
            set;
        }

        public void Dispose()
        {
            if (Dispatcher != null)
            {
                Dispatcher.InvokeShutdown();
                Dispatcher = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler ev = PropertyChanged;
            if (ev != null)
            {
                ev(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
