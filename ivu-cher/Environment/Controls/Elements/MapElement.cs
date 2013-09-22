using System;
using System.Windows.Media;
using AvengersUtd.Explore.Data.Resources;
using System.Windows.Media.Imaging;
using AvengersUtd.Explore.Environment.Templates;
using AvengersUtd.Explore.Data;
using System.Linq;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class MapElement : ResourceElement
    {
        public MapElement()
        {
        }

        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ElementContentLoaded(sender, e);
            CircleAdorner.SetVisibility(ThumbPosition.Bottom, false);
            ResourceRules.Add(ResourceType.Image, new Data.Elements.ResourceRule(ResourceType.Image, 1, 1));
            ResourceRules.Add(ResourceType.Text, new Data.Elements.ResourceRule(ResourceType.Text, 1, 1));
            ResourceBox.ItemsSource = ResourceRules.Values;
        }

    }
}
