using System;
using System.Windows.Media;
using AvengersUtd.Explore.Data.Resources;
using System.Windows.Media.Imaging;
using AvengersUtd.Explore.Environment.Templates;
using AvengersUtd.Explore.Data;
using System.Linq;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class PageElement : ResourceElement
    {
        public PageElement()
        {
            BuildingBlockLabel = "It";
        }

        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ElementContentLoaded(sender, e);
            CircleAdorner.SetVisibility(ThumbPosition.Bottom, false);
            ResourceRules.Add(ResourceType.Image, new Data.Elements.ResourceRule(ResourceType.Image, 1, 5));
            ResourceRules.Add(ResourceType.Text, new Data.Elements.ResourceRule(ResourceType.Text, 1, 5));
            ResourceRules.Add(ResourceType.Video, new Data.Elements.ResourceRule(ResourceType.Video, 1, 5));
            ResourceRules.Add(ResourceType.Audio, new Data.Elements.ResourceRule(ResourceType.Audio, 1, 5));
            ResourceBox.ItemsSource = ResourceRules.Values;
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

                case "Web":
                    WebPreview();
                    break;

                case "And":
                    AndPreview();
                    break;
            
            
            }

        }

        private void AndPreview()
        {
            // TODO: da definire

            // swich on template

            // check miminal conditions

            // render template...

            //ImageResource ir = ResourceBox.Items.OfType<ImageResource>().FirstOrDefault();
            //if (ir == null)
            //    return;

            //BitmapImage bmpImage = new BitmapImage(ir.Uri);

            //TextResource tr = ResourceBox.Items.OfType<TextResource>().FirstOrDefault();
            //string text = tr == null ? "No text resource found." : DataManager.LoadText(tr.Uri);
            ////TODO: Assegnare titolo descr.. e image...
            UGTempleteObiettivoRagg previewObject = new UGTempleteObiettivoRagg();
            CircleAdorner.SetVisibility(ThumbPosition.Top, false);
            Global.MainWindow.ANDPreview.Child = previewObject;
        }

        protected virtual void WMPreview()
        {
            ImageResource ir = ResourceBox.Items.OfType<ImageResource>().FirstOrDefault();
            if (ir == null)
                return;

            BitmapImage bmpImage = new BitmapImage(ir.Uri);

            TextResource tr = ResourceBox.Items.OfType<TextResource>().FirstOrDefault();
            string text = tr == null ? "No text resource found." : DataManager.LoadText(tr.Uri);

            MuseumGuideMain previewObject = new MuseumGuideMain
                (Caption, text, bmpImage);

            CircleAdorner.SetVisibility(ThumbPosition.Top, false);

            Global.MainWindow.WMPreview.Child = previewObject;

        }

        protected virtual void WebPreview()
        {
            Exporter exporter = new Exporter(OwnerCanvas);

            exporter.ExportMuseumGuideHTML("temp", Global.ExecutionPath);

            Global.MainWindow.WebPreview.Navigate(new Uri(Global.ExecutionPath + "temp\\index.htm").AbsoluteUri);

        }
   
    }
}
