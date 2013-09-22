using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AvengersUtd.Explore.Environment.Templates;
using AvengersUtd.Explore.Data.Resources;
using AvengersUtd.Explore.Data;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class MuseumGuideElement : PageElement
    {
        public MuseumGuideElement()
        {
            ElementHeight = 172;
            BuildingBlockLabel = "Mg";
        }

        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ElementContentLoaded(sender, e);
            CircleAdorner.SetVisibility(ThumbPosition.Top, false);
            CircleAdorner.SetVisibility(ThumbPosition.Bottom, true);
            CircleAdorner.SetVisibility(ThumbPosition.Right, false);
            WatermarkLabel.Content = "Maximize to edit options...";
            ConnectionType acceptedBottom = new ConnectionType(ThumbPosition.Bottom,
                                                typeof(MuseumGuideElement),
                                                ThumbPosition.Top,
                                                new Type[] { typeof(ThemeElement), typeof(Connector) }, true);
            ConnectionType acceptedRight = new ConnectionType(ThumbPosition.Right,
                                                typeof(MuseumGuideElement),
                                                ThumbPosition.Top,
                                                new Type[] { typeof(MapElement), typeof(Connector) }, true);

            Connections.Add(ThumbPosition.Bottom, acceptedBottom);
            Connections.Add(ThumbPosition.Right, acceptedRight);

            ResourceRules.Clear();
            ResourceRules.Add(ResourceType.Image, new Data.Elements.ResourceRule(ResourceType.Image, 1, 1));
            ResourceRules.Add(ResourceType.Text, new Data.Elements.ResourceRule(ResourceType.Text, 1, 1));

            
        }

       

    }
}
