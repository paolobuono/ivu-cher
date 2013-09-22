using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class MissionListElement : Element
    {
        static int count;

        private MissionListControl mlControl;

        public MissionListElement()
        {
            ElementHeight = 160;
            Caption = "Mission List" + ++count;
            BuildingBlockLabel = "ML";
        }

        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ElementContentLoaded(sender, e);
            mlControl = new MissionListControl();
            CircleAdorner.SetVisibility(false);
            CircleAdorner.SetVisibility(ThumbPosition.Top, true);
            WatermarkLabel.Content = "Maximize to edit mission order...";
            ContentPanel.Children.Insert(0,mlControl);
            OwnerCanvas.LinkEstablished += new EventHandler<LinkArgs>(OwnerCanvas_LinkEstablished);
            OwnerCanvas.LinkRemoved += new EventHandler<LinkArgs>(OwnerCanvas_LinkRemoved);
            OwnerCanvas.ElementCaptionChanged += new EventHandler<ElementCaptionArgs>(OwnerCanvas_ElementCaptionChanged);
            OwnerCanvas.ElementRemoved += new EventHandler<ElementArgs>(OwnerCanvas_ElementRemoved);

            foreach (PrologueElement pE in OwnerCanvas.Elements.OfType<PrologueElement>())
            {
                foreach (MissionElement mE in OwnerCanvas.GetElementsConnectedTo(pE).OfType<MissionElement>())
                    mlControl.AddMission(mE);
            }
        }

        public SortedList<int,MissionElement> MissionList
        {
            get
            {
                SortedList<int, MissionElement> missions = new SortedList<int, MissionElement>();
                var MissionData = OwnerCanvas.Elements.OfType<MissionElement>();
                foreach (string missionCaption in mlControl.Missions)
                {
                    int index = mlControl.FindMissionIndex(missionCaption);
                    missions.Add(index, MissionData.Where(p => p.Caption == missionCaption).First());
                }
                return missions;
            }
        }

        void OwnerCanvas_LinkRemoved(object sender, LinkArgs e)
        {
            MissionElement mElement = e.Target as MissionElement;
            if (mElement == null)
                return;

            if (mlControl.HasMission(e.Target.Caption))
                mlControl.RemoveMission(e.Target.Caption);

        }

        void OwnerCanvas_ElementRemoved(object sender, ElementArgs e)
        {
            MissionElement mElement = e.Element as MissionElement;
            if (mElement == null)
                return;

            if (mlControl.HasMission(e.Element.Caption))
                mlControl.RemoveMission(e.Element.Caption);
        }

        void OwnerCanvas_ElementCaptionChanged(object sender, ElementCaptionArgs e)
        {
            MissionElement mElement = e.Element as MissionElement;
            if (mElement == null)
                return;

            if (mlControl.HasMission(e.OldCaption))
                mlControl.EditMission(e.OldCaption, e.NewCaption);

        }

        void OwnerCanvas_LinkEstablished(object sender, LinkArgs e)
        {
            MissionElement mElement = e.Target as MissionElement;
            if (mElement == null)
                return;

            mlControl.AddMission(mElement);
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
