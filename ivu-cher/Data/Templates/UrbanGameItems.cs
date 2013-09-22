
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvengersUtd.Explore.Data.Templates
{
    public class MonumentXml
    {
        public MonumentXml()
        {
            // initializing lists
            FotoResId = new List<string>();
            AudioResId = new List<string>();
            Ricostruzioni3dId = new List<string>();
            //  DomandaId = new List<string>();
            CommentoID = new List<string>();
        }

        [XmlAttribute]
        public int id { get; set; }
        [XmlAttribute]
        public string nome { get; set; }
        [XmlAttribute]
        public string latitudine;
        [XmlAttribute]
        public string longitudine;
        [XmlElement]
        public string storia { get; set; }
        [XmlElement]
        public string informazione { get; set; }

        [XmlElement("idfotografia")]
        public List<string> FotoResId;

        [XmlElement("idaudio")]
        public List<string> AudioResId;

        [XmlElement("idricostruzione3d")]
        public List<string> Ricostruzioni3dId;
        [XmlElement("idcommento")]
        public List<string> CommentoID;
        //[XmlElement("idDomanda")]
        //public List<string> DomandaId;
    }

    public class RisorsaAudio
    {
        [XmlElement]
        public string id { get; set; }
        [XmlElement("audio")]
        public string fileAudio { get; set; }
        [XmlElement]
        public string lat { get; set; }
        [XmlElement]
        public string lon { get; set; }
        [XmlElement]
        public string dMin { get; set; }
        [XmlElement]
        public string dMax { get; set; }
        [XmlElement]
        public string roll { get; set; }
    }

    public class Commento
    {
        [XmlElement]
        public string id { get; set; }
        [XmlElement]
        public string testo { get; set; }
        [XmlElement]
        public string utente { get; set; }
    }
    public class Domanda
    {
        public Domanda()
        { }


        [XmlAttribute]
        public string id { get; set; }
        [XmlAttribute]
        public string testo { get; set; }
        [XmlElement]
        public string indizio { get; set; }
        [XmlElement]
        public string risposta { get; set; }
        [XmlElement]
        public string rispostaerrata1 { get; set; }
        [XmlElement]
        public string rispostaerrata2 { get; set; }
    }
    public class Fotografia
    {
        [XmlAttribute]
        public string id { get; set; }
        [XmlText]
        public string file { get; set; }
    }

    public class ricostruzione3d
    {
        [XmlAttribute]
        public string id { get; set; }
        [XmlText]
        public string info { get; set; }
    }
    public class stile
    {
        [XmlAttribute]
        public string id { get; set; }
        [XmlElement]
        public string sfondo { get; set; }
    }

    public class Tappa
    {
        public Tappa()
        {
            iddomanda = new List<string>();
        }

        [XmlAttribute]
        public string tipo { get; set; }
        public string id { get; set; }
        public string descrizione { get; set; }
        [XmlElement("iddomanda")]
        public List<string> iddomanda { get; set; }





    }


}
