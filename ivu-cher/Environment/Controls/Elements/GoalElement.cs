using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Explore.Environment.Templates;
using AvengersUtd.Explore.Data.Resources;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class GoalElement : ResourceElement
    {
        static int count;
        public GoalElement()
        {
            ElementHeight = 160;
            Caption = "Goal" + ++count;
            BuildingBlockLabel = "Go";
        }

        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {

            base.ElementContentLoaded(sender, e);
            CircleAdorner.SetVisibility(ThumbPosition.Right, false);
            CircleAdorner.SetVisibility(ThumbPosition.Bottom, false);
            WatermarkLabel.Content = "Maximize to add resources...";
            InfoText.Text = "Resources for the goal screen:";
            ResourceRules.Clear();
            ResourceRules.Add(ResourceType.Image, new Data.Elements.ResourceRule(ResourceType.Image, 1, 16));
            //ResourceRules.Add(ResourceType.Text, new Data.Elements.ResourceRule(ResourceType.Text, 1, 1));
            //ResourceRules.Add(ResourceType.Audio, new Data.Elements.ResourceRule(ResourceType.Audio, 1, 1));
            ResourceBox.ItemsSource = ResourceRules.Values;
        }

    }
}
