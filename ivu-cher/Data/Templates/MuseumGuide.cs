using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Explore.Data.Resources;
using System.Xml.Serialization;

namespace AvengersUtd.Explore.Data.Templates
{
    public class MuseumGuide
    {
        [XmlAttribute]
        public string Id { get; set; }
        
        [XmlArray("AppResources")]
        [XmlArrayItem(ElementName = "ResourceId")]
        public string[] ResourceIDs { get; set; }


        [XmlArray("Themes")]
        [XmlArrayItem("Theme")]
        public Theme[] Themes {get;set;}

    }

    public class Theme {

        [XmlAttribute]
        public string Id {get;set;}

        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public ResourceElementXml[] Items {get;set;}
    }
}
