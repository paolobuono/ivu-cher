using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Explore.Data.Resources;

namespace AvengersUtd.Explore.Environment.Controls.Elements.UG_Elements
{
    public class UG_GoalElement: UG_BaseElement
    {


        public UG_GoalElement() 
        {
            ElementType = Data.Elements.ElementType.UG_GoalElement;
        }


        protected override void ElementContentLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            base.ElementContentLoaded(sender, e);
            CircleAdorner.SetVisibility(ThumbPosition.Top, true);
        }

    }
}
