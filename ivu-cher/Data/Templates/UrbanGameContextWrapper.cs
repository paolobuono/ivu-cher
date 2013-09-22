
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvengersUtd.Explore.Data.Templates
{
    [XmlRoot("context")]
    public class UrbanGameContextWrapper
    {
        public UrbanGameContextWrapper()
        {
            Luoghi = new List<MonumentXml>();
            ListAudioResources = new List<RisorsaAudio>();
            ListComments = new List<Commento>();
            ListDomande = new List<Domanda>();
            ListFoto = new List<Fotografia>();
            ListRicostruzione3d = new List<ricostruzione3d>();
        }


        [XmlAttribute]
        public string tipo = "urbangame";

        [XmlAttribute]
        public string nome { get; set; }

        [XmlElement]
        public string descrizione { get; set; }

        [XmlArray("luoghi")]
        [XmlArrayItem("monumento")]
        public List<MonumentXml> Luoghi { get; set; }

        [XmlArray("audioContext")]
        [XmlArrayItem("risorsa")]
        public List<RisorsaAudio> ListAudioResources { get; set; }
        [XmlArray("commenti")]
        [XmlArrayItem("commento")]
        public List<Commento> ListComments { get; set; }
        [XmlArray("domande")]
        [XmlArrayItem("domanda")]
        public List<Domanda> ListDomande { get; set; }
        [XmlArray("fotografie")]
        [XmlArrayItem("foto")]
        public List<Fotografia> ListFoto { get; set; }
        [XmlArray("ricostruzioni3d")]
        [XmlArrayItem("ricostruzione3d")]
        public List<ricostruzione3d> ListRicostruzione3d { get; set; }
        [XmlElement]
        public stile stile { get; set; }

    
    }
}
