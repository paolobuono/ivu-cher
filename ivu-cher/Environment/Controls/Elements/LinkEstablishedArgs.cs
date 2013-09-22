using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Explore.Environment.Controls.Elements
{
    public class LinkArgs:EventArgs
    {
        public Element Source { get; set; }
        public Element Target { get; set; }

        public LinkArgs(Element source, Element target)
        {
            Source = source;
            Target = target;
        }
    }
}
