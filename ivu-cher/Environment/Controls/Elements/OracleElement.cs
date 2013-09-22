using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Explore.Data.Resources;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class OracleElement : ResourceElement
    {
        static int count;

        public OracleElement()
        {
            Caption = "OracleHint" + ++count;
            BuildingBlockLabel = "OH";
            ElementHeight = 160;
        }

        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ElementContentLoaded(sender, e);
            CircleAdorner.SetVisibility(false);
            CircleAdorner.SetVisibility(ThumbPosition.Top, true);
            WatermarkLabel.Content = "Maximize to add mission hints...";
            InfoText.Text = "Mission hints:";
            ResourceRules.Clear();
            ResourceRules.Add(ResourceType.Text, new Data.Elements.ResourceRule(ResourceType.Text, 1, 6));
            ResourceBox.ItemsSource = ResourceRules.Values;
        }
    }
}
