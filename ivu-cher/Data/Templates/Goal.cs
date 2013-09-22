using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvengersUtd.Explore.Data.Templates
{
    public class Goal
    {
        [XmlAttribute]
        public string ID { get; set; }

        [XmlArray("Resources")]
        [XmlArrayItem("ResourceID")]
        public string[] ResourceIDs { get; set; }
    }
}
