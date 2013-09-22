using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AvengersUtd.Explore.Data.Resources;

namespace AvengersUtd.Explore.Data.Templates
{
    public struct TileElement
    {
        [XmlAttribute]
        public int Position { get; set; }

        [XmlAttribute]
        public string Question { get; set; }

        [XmlAttribute]
        public string Answer { get; set; }

        [XmlAttribute]
        public string ImageResourceId { get; set; }

        [XmlIgnore]
        public ImageResource ImageResource { get; set; }
    }
}
