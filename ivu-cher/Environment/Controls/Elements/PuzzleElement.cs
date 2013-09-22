using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Explore.Environment.Templates;
using AvengersUtd.Explore.Data.Resources;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class PuzzleElement : ResourceElement
    {
        PuzzleProperties puzzleProperties;

        public PuzzleElement()
        {
            ElementHeight = 240;
            BuildingBlockLabel = "Pz";
        }

        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ConnectionType acceptedRight = new ConnectionType
                (ThumbPosition.Right,
                 typeof(PuzzleElement),
                 ThumbPosition.Top,
                 new Type[] { typeof(QuestionElement) });

            Connections.Add(ThumbPosition.Right, acceptedRight);

            base.ElementContentLoaded(sender, e);
            puzzleProperties = new PuzzleProperties();
            ContentPanel.Children.Insert(0,puzzleProperties);
            CircleAdorner.SetVisibility(ThumbPosition.Right, false);
            InfoText.Text = "Drag a puzzle image inside:";

            ResourceRules.Add(ResourceType.Image, new Data.Elements.ResourceRule(ResourceType.Image, 1, 1));
            ResourceBox.ItemsSource = ResourceRules.Values;
        }

        protected override void OnGotFocus(System.Windows.RoutedEventArgs e)
        {

            base.OnGotFocus(e);
            if (!ResourceBoxMode || ResourceBox == null || ResourceBox.Items.Count == 0)
                return;

            switch (Global.MainWindow.ActivePreview)
            {
                case "MT":
                    MTPreview();
                    break;
            }

        }

        void MTPreview()
        {
           
            ImageResource iResource = ResourceBox.Items.OfType<ImageResource>().First();

            if (iResource == null)
                return;

            IEnumerable<QuestionElement> qElements = OwnerCanvas.GetElementsConnectedTo(this).OfType<QuestionElement>();
            if (qElements.Count() == 0)
                return;

            QuestionElement qElement = qElements.First();

            Global.MainWindow.MtPreview.Content = new HPTemplate(iResource, Caption, qElement.AllAnswers, puzzleProperties.Rows, puzzleProperties.Columns);
        }

    }
}
