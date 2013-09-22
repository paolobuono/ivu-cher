using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Explore.Environment.Templates;
using AvengersUtd.Explore.Data.Resources;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class MissionElement : ResourceElement
    {
        static int count;
        QuestProperties questProperties;

        public MissionElement()
        {
            ElementHeight = 190;
            ConnectionType acceptedRight = new ConnectionType(ThumbPosition.Right,
                                                typeof(MissionElement),
                                                ThumbPosition.Top,
                                                new Type[] { typeof(Connector), typeof(OracleElement)});
            ConnectionType acceptedBottom = new ConnectionType(ThumbPosition.Bottom,
                                                typeof(MissionElement),
                                                ThumbPosition.Top,
                                                new Type[] { typeof(GoalElement) });

            Connections.Add(ThumbPosition.Right, acceptedRight);
            Connections.Add(ThumbPosition.Bottom, acceptedBottom);
            Caption = "Mission" + ++count;
            BuildingBlockLabel = "Mi";
        }

        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {

            base.ElementContentLoaded(sender, e);
            questProperties = new QuestProperties();
            ContentPanel.Children.Insert(0, questProperties);
            CircleAdorner.SetVisibility(ThumbPosition.Right, true);
            CircleAdorner.SetVisibility(ThumbPosition.Bottom, true);
            WatermarkLabel.Content = "Maximize to edit properties...";
            InfoText.Text = "Resources for the mission screen:";
            ResourceRules.Clear();
            //ResourceRules.Add(ResourceType.Image, new Data.Elements.ResourceRule(ResourceType.Image, 1, 1));
            ResourceRules.Add(ResourceType.Text, new Data.Elements.ResourceRule(ResourceType.Text, 1, 1));
            //ResourceRules.Add(ResourceType.Audio, new Data.Elements.ResourceRule(ResourceType.Audio, 1, 1));
            ResourceBox.ItemsSource = ResourceRules.Values;
        }

        public string PlaceId
        {
            get { return questProperties.PlaceId; }
        }

    }
}
