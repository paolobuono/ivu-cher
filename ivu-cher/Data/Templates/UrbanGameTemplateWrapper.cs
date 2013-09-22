using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvengersUtd.Explore.Data.Templates
{
    [XmlRoot("template")]
    public class UrbanGameTemplateWrapper
    {
        public UrbanGameTemplateWrapper()
        { Tappe = new List<Tappa>(); }

        [XmlAttribute]
        private string name = "Game Guide";
        [XmlArray("tappe")]
        [XmlArrayItem("idtappa")]
        public List<Tappa> Tappe;
        public string idstile = "1";

    }
}
