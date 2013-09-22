using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Controls;
using AvengersUtd.Explore.Data.Resources;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class ExcursionGameElement : ResourceElement
    {
        public ExcursionGameElement()
        {
            ConnectionType acceptedBottom = new ConnectionType(ThumbPosition.Bottom,
                                                            typeof(ExcursionGameElement),
                                                            ThumbPosition.Top,
                                                            new Type[] { typeof(PrologueElement)});
            ConnectionType acceptedRight = new ConnectionType(ThumbPosition.Right,
                                               typeof(ExcursionGameElement),
                                               ThumbPosition.Top,
                                               new Type[] { typeof(Connector), typeof(ContextSoundElement)});

            Connections.Add(ThumbPosition.Bottom, acceptedBottom);
            Connections.Add(ThumbPosition.Right, acceptedRight);

            BuildingBlockLabel = "EG";

            ElementHeight = 160;
        }

        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            
            base.ElementContentLoaded(sender, e);
            CircleAdorner.SetVisibility(ThumbPosition.Top, false);
            CircleAdorner.SetVisibility(ThumbPosition.Right, true);
            ResourceRules.Add(ResourceType.Image, new Data.Elements.ResourceRule(ResourceType.Image, 1, 1));
            ResourceRules.Add(ResourceType.Text, new Data.Elements.ResourceRule(ResourceType.Text, 1, 1));
            ResourceBox.ItemsSource=ResourceRules.Values;
            WatermarkLabel.Content = "Maximize to edit options...";
        }
    }
}
