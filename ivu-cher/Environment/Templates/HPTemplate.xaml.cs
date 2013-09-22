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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AvengersUtd.Explore.Data.Resources;

namespace AvengersUtd.Explore.Environment.Templates
{
    /// <summary>
    /// Interaction logic for HistoryPuzzle.xaml
    /// </summary>
    public partial class HPTemplate : UserControl
    {
        int puzzleSide = 400;
        int xOffset = 120;
        int yOffset = 56;

        public HPTemplate()
        {
            InitializeComponent();
            DrawTitle("Select a Puzzle element connected to a QA element first.");
        }

        public HPTemplate(ImageResource image, string title, string[] answers, int horizontalLines, int verticalLines)
        {
            InitializeComponent();

            puzzleGrid.Children.Clear();
            DrawBackground(image);
            DrawGrid(horizontalLines, verticalLines);
            DrawAnswers(answers);
            DrawTitle(title);
        }

        void DrawTitle(string text)
        {
            TextBlock tb = new TextBlock();
            tb.FontSize = 24;
            tb.Text = text;
            tb.Width = 400;
            tb.Padding = new Thickness(8);
            tb.Background = Brushes.Black;
            tb.Foreground = Brushes.White;
            tb.TextWrapping = TextWrapping.Wrap;
            Canvas.SetLeft(tb, 112);
            puzzleGrid.Children.Add(tb);
        }

        void DrawBackground(ImageResource image)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Width = rectangle.Height = puzzleSide;
            Canvas.SetLeft(rectangle, xOffset);
            Canvas.SetTop(rectangle, yOffset);
            rectangle.Fill = new ImageBrush(new BitmapImage(image.PreviewUri));
            puzzleGrid.Children.Add(rectangle);
        }

        void DrawAnswers(string[] answers)
        {
            int blockSize = 96;
            int x=8;
            int y=8;
            for (int i = 0; i < answers.Length; i++)
            {
                TextBlock tb = new TextBlock();
                tb.Width = blockSize;
                tb.Height = blockSize;
                tb.FontSize = 12;
                tb.Text = answers[i];
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Padding = new Thickness(8);
                tb.Background = Brushes.LightGray;
                tb.Foreground = Brushes.Black;

                if (i == 4)
                    y = 8;

                if (i < 4)
                    Canvas.SetLeft(tb, x);
                else
                    Canvas.SetLeft(tb, puzzleGrid.Width - blockSize - 8);
                
                Canvas.SetTop(tb, y);
                y += 8 + blockSize;
                puzzleGrid.Children.Add(tb);
            }
        }

        void DrawGrid(int horizontalLines, int verticalLines)
        {       
            float horizontalFrequency = puzzleSide / horizontalLines;
            float verticalFrequency = puzzleSide / verticalLines;

            for (int i = 0; i <= horizontalLines; i++)
            {
                Line line = new Line
                {
                    X1 = xOffset,
                    Y1 = yOffset +i * verticalFrequency,
                    X2 = xOffset + puzzleSide,
                    Y2 = yOffset + i * verticalFrequency,
                    Stroke = Brushes.White,
                    StrokeThickness = 4,
                    IsHitTestVisible = false
                };
                puzzleGrid.Children.Add(line);  
            }

            for (int i = 0; i <= verticalLines; i++)
            {
                Line line = new Line
                {
                    X1 = xOffset+i * horizontalFrequency,
                    Y1 = yOffset,
                    X2 = xOffset + i * horizontalFrequency,
                    Y2 = yOffset + puzzleSide,
                    Stroke = Brushes.White,
                    StrokeThickness = 4,
                    IsHitTestVisible = false
                };
                puzzleGrid.Children.Add(line);
            }

        }

    }
}
