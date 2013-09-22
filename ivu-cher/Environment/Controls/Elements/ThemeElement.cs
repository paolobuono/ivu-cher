using System;
using System.Windows.Media;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class ThemeElement : Element
    {
        public ThemeElement () : base()
        {
            Background = Brushes.PapayaWhip;
            ConnectionType acceptedBottom = new ConnectionType(ThumbPosition.Bottom,
                                                            typeof (ThemeElement),
                                                            ThumbPosition.Top,
                                                            new Type[] {typeof (PageElement), typeof(Connector)},true);

            ConnectionType acceptedRight = new ConnectionType(ThumbPosition.Right,
                                                            typeof(ThemeElement),
                                                            ThumbPosition.Left,
                                                            new Type[] { typeof(ThemeElement) });

            Connections.Add(ThumbPosition.Bottom, acceptedBottom);
            Connections.Add(ThumbPosition.Right, acceptedRight);
            //MinHeight = 92;
            BuildingBlockLabel = "Th";
        }

        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ElementContentLoaded(sender, e);
            CircleAdorner.SetVisibility(ThumbPosition.Left, false);
            CircleAdorner.SetVisibility(ThumbPosition.Right, false);
            CircleAdorner.SetBackground(ThumbPosition.Right, (SolidColorBrush)FindResource("ConnectorUnconnected"));
            WatermarkLabel.Content = "Connect to Item blocks";
        }

        public override System.Collections.Generic.IEnumerable<ThumbPosition> AvailableIncomingConnectors
        {
            get
            {
                yield return ThumbPosition.Top;
                yield return ThumbPosition.Left;
            }
        }
    }
}
