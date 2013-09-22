using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvengersUtd.Explore.Data.Templates
{
    public class HistoryPuzzleTemplate
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public int MaximumPlayers { get; set; }

        [XmlArray("Puzzles")]
        [XmlArrayItem("PuzzleID")]
        public string[] PuzzleIDs { get; set; }

        [XmlIgnore]
        public SortedDictionary<string, Puzzle> Puzzles { get; private set; }

    }
}
