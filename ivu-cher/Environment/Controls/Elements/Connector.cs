using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class Connector : Element
    {
        public Connector()
        {
            Type[] types = new Type[] { typeof(Connector), typeof(PuzzleElement),typeof(PageElement), 
                typeof(GoalElement), typeof(MissionElement), typeof(ContextSoundElement), typeof(UG_Elements.UG_ContentItem),
                typeof(QuestionElement)};

            ConnectionType acceptedBottom = new ConnectionType(ThumbPosition.Bottom,
                                                            typeof(Connector),
                                                            ThumbPosition.Top,
                                                            types);

            ConnectionType acceptedRight = new ConnectionType(ThumbPosition.Right,
                                                            typeof(Connector),
                                                            ThumbPosition.Top,
                                                            types);

            ConnectionType acceptedLeft= new ConnectionType(ThumbPosition.Left,
                                                            typeof(Connector),
                                                            ThumbPosition.Top,
                                                            types);

            Connections.Add(ThumbPosition.Bottom,acceptedBottom);
            Connections.Add(ThumbPosition.Right, acceptedRight);
            Connections.Add(ThumbPosition.Left, acceptedLeft);


        }

        protected override void ElementContentLoaded(object sender, RoutedEventArgs e)
        {
            base.ElementContentLoaded(sender, e);
            infoButton.Visibility = Visibility.Collapsed;
            CaptionBorder.CornerRadius = new CornerRadius(16);
            CaptionBorder.Height = 60;
            CollapseButton.Visibility = Visibility.Hidden;
            WatermarkLabel.Visibility = Visibility.Hidden;
            //ContentPanel.Visibility = Visibility.Collapsed;
            CaptionTextBlock.Visibility = Visibility.Hidden;
            CloseButton.Margin = new Thickness(6, 20, 6, 20);

            DockPanel.SetDock(((StackPanel)CloseButton.Parent), Dock.Top);

            Width = 64;
            Height = 64;

            CircleAdorner.SetVisibility(true);
            Caption = "Connector" + Element.Index;
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
