using System;
using System.Windows.Media;
using AvengersUtd.Explore.Data.Resources;
using System.Linq;
using System.Windows.Media.Imaging;
using AvengersUtd.Explore.Data;
using AvengersUtd.Explore.Environment.Templates;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class PrologueElement : ResourceElement
    {
        static int count;
        public PrologueElement() : base()
        {
            ElementHeight = 160;
            Background = Brushes.PapayaWhip;
            ConnectionType acceptedBottom = new ConnectionType(ThumbPosition.Bottom,
                                                            typeof(PrologueElement),
                                                            ThumbPosition.Top,
                                                            new Type[] { typeof(MissionElement), typeof(Connector) }, true);

            ConnectionType acceptedRight = new ConnectionType(ThumbPosition.Right,
                                              typeof(PrologueElement),
                                              ThumbPosition.Top,
                                              new Type[] { typeof(MissionListElement) });

            Caption = "Prologue" + ++count;
            Connections.Add(ThumbPosition.Bottom, acceptedBottom);
            Connections.Add(ThumbPosition.Right, acceptedRight);
            BuildingBlockLabel = "Pr";
        }

        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ElementContentLoaded(sender, e);
            CircleAdorner.SetVisibility(ThumbPosition.Left, false);
            CircleAdorner.SetVisibility(ThumbPosition.Right, true);
            CircleAdorner.SetBackground(ThumbPosition.Right, (SolidColorBrush)FindResource("ConnectorUnconnected"));
           
            
            ResourceRules.Clear();
            ResourceRules.Add(ResourceType.Image, new Data.Elements.ResourceRule(ResourceType.Image, 1, 1));
            ResourceRules.Add(ResourceType.Text, new Data.Elements.ResourceRule(ResourceType.Text, 1, 1));
            ResourceBox.ItemsSource = ResourceRules.Values;
            WatermarkLabel.Content = "Maximize to design a prologue.";
            InfoText.Text = "Resources for the intro screen:";
        }

        public override System.Collections.Generic.IEnumerable<ThumbPosition> AvailableIncomingConnectors
        {
            get
            {
                yield return ThumbPosition.Top;
            }
        }

        protected override void OnGotFocus(System.Windows.RoutedEventArgs e)
        {

            base.OnGotFocus(e);
            if (ResourceBox == null)
                return;

            switch (Global.MainWindow.ActivePreview)
            {
                case "WM":
                    WMPreview();
                    break;
            }

        }

        protected virtual void WMPreview()
        {
            ImageResource ir = ResourceBox.Items.OfType<ImageResource>().FirstOrDefault();
            if (ir == null)
                return;

            BitmapImage bmpImage = new BitmapImage(ir.Uri);

            TextResource tr = ResourceBox.Items.OfType<TextResource>().FirstOrDefault();
            string text = tr == null ? "No text resource found." : DataManager.LoadText(tr.Uri);

            EGPrologueMain previewObject = new EGPrologueMain(Caption, text, bmpImage);

            Global.MainWindow.WMPreview.Child = previewObject;
        }
    }
}
