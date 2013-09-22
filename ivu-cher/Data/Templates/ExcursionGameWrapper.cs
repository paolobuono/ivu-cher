using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvengersUtd.Explore.Data.Templates
{
    [XmlRoot("ExcursionGame")]
    public class ExcursionGameWrapper
    {
        [XmlAttribute]
        public string GameTitle { get; set; }

        [XmlAttribute]
        public string PrologueTitle { get; set; }

        [XmlArray("Missions")]
        [XmlArrayItem("Mission")]
        public MissionElementXml[] Missions { get; set; }

        [XmlArray("ContextualSounds")]
        [XmlArrayItem("ContextualSound")]
        public ContextualSoundXml[] ContextualSounds { get; set; }
    }
}
