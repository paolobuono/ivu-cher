using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Explore.Data.Resources;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{

    public class ContextSoundElement : ResourceElement
    {
        static int count;
        ContextSoundProperties csProperties;
        

        public ContextSoundElement()
        {
            ElementHeight = 190;
            Caption = "Cont. Sounds" + ++count;
            BuildingBlockLabel = "CS";
        }

        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ElementContentLoaded(sender, e);
            //csProperties = new ContextSoundProperties();
            CircleAdorner.SetVisibility(false);
            CircleAdorner.SetVisibility(ThumbPosition.Top, true);
            WatermarkLabel.Content = "Maximize to edit properties...";
            InfoText.Text = "Audio resources:";
            //ContentPanel.Children.Insert(0,csProperties);

            ResourceRules.Clear();
            ResourceRules.Add(ResourceType.Audio, new Data.Elements.ResourceRule(ResourceType.Audio, 1, 99));
            ResourceBox.ItemsSource = ResourceRules.Values;
            ResourceBox.Height = 120;
            
        }

        public override IEnumerable<ThumbPosition> AvailableIncomingConnectors
        {
            get
            {
                yield return ThumbPosition.Top;
            }
        }
    }
}
