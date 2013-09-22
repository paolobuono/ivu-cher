using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class ElementArgs : EventArgs
    {
        public Element Element { get; set; }

        public ElementArgs(Element element)
        {
            Element = element;
        }
    }

    public class ElementCaptionArgs : ElementArgs
    {
        public string OldCaption { get; set; }
        public string NewCaption { get; set; }

        public ElementCaptionArgs(Element element, string oldCaption, string newCaption) : base(element)
        {
            OldCaption = oldCaption;
            NewCaption = newCaption;
        }
    }


}
