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
using AvengersUtd.Explore.Data.Resources;

namespace AvengersUtd.Explore.Environment.Windows
{
    /// <summary>
    /// Interaction logic for Wizard.xaml
    /// </summary>
    public partial class Wizard : Window
    {
        public Wizard()
        {



            InitializeComponent();
        }

        private void MuseumButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow
                                        {
                                            Title = "Cultural Heritage Expert Workshop - New Museum Guide"
                                        };
            this.Close();
            Global.Template = TemplateType.MuseumGuide;
            mainWindow.InitMuseumGuide();
            mainWindow.Show();

        }

        private void ExcursionGameButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow
                                        {
                                            Title = "Cultural Heritage Expert Workshop - New Excursion-game"
                                        };
            this.Close();
            Global.Template = TemplateType.ExcursionGame;
            mainWindow.TestExplore();
            mainWindow.Show();
        }

        private void PuzzleButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow
                                        {
                                           Title = "Cultural Heritage Expert Workshop - New History-Puzzle"
                                        };
            Global.Template = TemplateType.HistoryPuzzle;
           
            this.Close();
            mainWindow.InitHistoryPuzzle();
            mainWindow.Show();
        }

        private void UrbanGameClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow
            {
                Title = "Cultural Heritage Expert Workshop - New Urban-game"
            };
            Global.Template = TemplateType.UrbanGame;

            this.Close();
            mainWindow.InitUrbanGame();
            mainWindow.Show();

        }
    }
}
