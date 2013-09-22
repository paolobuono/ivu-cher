using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvengersUtd.Explore.Data.Templates
{
    public class Puzzle
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public int TileCount { get; set; }

        [XmlArray(Order = 0)]
        [XmlArrayItem("Tile")]
        public List<TileElement> Tiles { get; set; }

        [XmlArray(ElementName="FalseAnswers", Order = 1)]
        [XmlArrayItem("FalseAnswer")]
        public string[] FalseAnswers { get; set; }
    }
}
