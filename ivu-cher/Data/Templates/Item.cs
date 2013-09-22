using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvengersUtd.Explore.Data.Templates
{
    public class ResourceElementXml
    {
        [XmlAttribute]
        public string ID { get; set; }

        [XmlArray("Resources")]
        [XmlArrayItem("ResourceID")]
        public string[] ResourceIDs { get; set; }

    }

    public class MissionElementXml : ResourceElementXml
    {
        [XmlAttribute]
        public int Index { get; set; }

        [XmlAttribute]
        public string PlaceId { get; set; }
        
        [XmlElement("Goal")]
        public ResourceElementXml Goal { get; set; }

        [XmlElement("OracleHint")]
        public ResourceElementXml OracleHint { get; set; }
    }

    public class ContextualSoundXml : ResourceElementXml
    {
        [XmlAttribute]
        public string GPSCoordNS { get; set; }

        [XmlAttribute]
        public string Radius { get; set; }
    }
}
