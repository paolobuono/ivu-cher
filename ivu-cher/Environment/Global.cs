using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AvengersUtd.Explore.Data.Resources;
using AvengersUtd.Explore.Environment.Controls;
using AvengersUtd.Explore.Environment.Windows;

namespace AvengersUtd.Explore.Environment
{
    public static class Global
    {
        public static TemplateType Template { get; set; }
        public static GridCanvas WorkArea { get; set; }
        public static MainWindow MainWindow { get; set; }
        public static string ExecutionPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string ResourcePath = "Resources\\";
        public static string PicturePath = ExecutionPath + ResourcePath + "Pictures\\";
        public static string TextPath = ExecutionPath + ResourcePath + "Text\\";
        public static string AudioPath = ExecutionPath + ResourcePath + "Audio\\";


        static Global()
        {
            ImageResource.IconUri = new Uri("pack://application:,,,/Environment;component/Icons/pictures.png");
            AudioResource.IconUri = new Uri("pack://application:,,,/Environment;component/Icons/sound.png");
            TextResource.IconUri = new Uri("pack://application:,,,/Environment;component/Icons/text.png");
            VideoResource.IconUri = new Uri("pack://application:,,,/Environment;component/Icons/video.png");
        }
    }

 
}
