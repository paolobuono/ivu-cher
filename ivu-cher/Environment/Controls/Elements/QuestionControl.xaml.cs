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

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    /// <summary>
    /// Interaction logic for QuestionControl.xaml
    /// </summary>
    public partial class QuestionControl : UserControl
    {
        private readonly BitmapImage questionIcon;
        private readonly BitmapImage answerIcon;
        private BitmapImage falseAnswerIcon;
        List<String> answers;
        List<String> falseAnswers;
        private bool _showIndizio;


        public bool ShowIndizio
        {
            get { return _showIndizio; }
            set
            {
                _showIndizio = value;
                SetIndizio();
            }
        }

        private void SetIndizio()
        {
            lIndizio.Visibility = tbIndizio.Visibility = _showIndizio ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }


        public QuestionControl()
        {
            InitializeComponent();
            questionIcon = (BitmapImage)FindResource("Question");
            answerIcon = (BitmapImage)FindResource("Mark");
            falseAnswerIcon = (BitmapImage)FindResource("Cancel");

            answers = new List<string>();
            falseAnswers = new List<string>();

        }

        public String[] AllAnswers
        {
            get
            {
                return answers.Concat(falseAnswers).ToArray();
            }
        }
        public string Testo { get { return tbQuestion.Text; } }
        public string Indizio { get { return tbIndizio.Text; } }

        private void QAClick(object sender, RoutedEventArgs e)
        {
            if (Global.Template == Data.Resources.TemplateType.UrbanGame && answers.Count == 1)
            {
                MessageBox.Show("You can enter only one correct answer for each question in the template Urbanoid");
                return;
            }
            answers.Add(tbAnswer.Text);

            var item = new
                       {
                           Index = lbQA.Items.Count + 1,
                           QuestionIcon = questionIcon,
                           AnswerIcon = answerIcon,
                           Question = tbQuestion.Text,
                           Answer = tbAnswer.Text
                       };
            lbQA.Items.Add(item);
        }

        private void FalseAnswerClick(object sender, RoutedEventArgs e)
        {
            if (Global.Template == Data.Resources.TemplateType.UrbanGame && falseAnswers.Count == 2)
            {
                MessageBox.Show("You can enter up to 2 wrong answers for each question in the template Urbanoid");
                return;
            }
            falseAnswers.Add(tbFalseAnswer.Text);
            var item = new
                       {
                           Index = lbFalseAnswers.Items.Count + 1,
                           FalseAnswerIcon = falseAnswerIcon,
                           FalseAnswer = tbFalseAnswer.Text
                       };
            lbFalseAnswers.Items.Add(item);
        }


    }
}
