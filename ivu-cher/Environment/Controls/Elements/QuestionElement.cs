using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Explore.Data;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class QuestionElement : Element
    {

        public string QuestionId
        {
            get;
            set;
        }

        private QuestionControl qControl;

        public QuestionElement()
        {
            this.QuestionId = this.elementIndex.ToString();
            ElementHeight = 400;
            BuildingBlockLabel = "QA";

        }

        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ElementContentLoaded(sender, e);
            qControl = new QuestionControl();
            if (Global.Template == Data.Resources.TemplateType.UrbanGame)
            {qControl.ShowIndizio = true;
            this.ElementHeight = this.ElementHeight + 60;
            }
            CircleAdorner.SetVisibility(false);
            CircleAdorner.SetVisibility(ThumbPosition.Top, true);
            WatermarkLabel.Content = "Maximize to edit questions...";
            ContentPanel.Children.Add(qControl);
        }

        public string Testo
        {
            get { return qControl.Testo; }
        }
        public string Indizio
        { get { return qControl.Indizio; } }

        public string[] AllAnswers
        {
            get { return qControl.AllAnswers; }
        }

        public override IEnumerable<ThumbPosition> AvailableIncomingConnectors
        {
            get
            {
                yield return ThumbPosition.Top;
            }
        }

        /// <summary>
        /// convert to UGTemplateWrapper
        /// </summary>
        /// <returns></returns>
        internal Data.Templates.Domanda ConvertToWrapper()
        {
            Data.Templates.Domanda t_ret = new Data.Templates.Domanda();
            if (AllAnswers.Count() > 1)
            {
                t_ret.risposta = AllAnswers.First().toWrapper();

                if (AllAnswers.Length>1)
                { t_ret.rispostaerrata1 = AllAnswers[1].toWrapper() ;}
               
                if (AllAnswers.Length>2)
                { t_ret.rispostaerrata2 = AllAnswers[2].toWrapper();}

            }
            t_ret.id = this.QuestionId;
            t_ret.testo = this.Testo;
            t_ret.indizio = this.Indizio.toWrapper();
            return t_ret;
        }

    }
}
