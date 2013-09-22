using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvengersUtd.Explore.Data.Resources
{
    [XmlType("AudioResource")]
    public class AudioResource:AbstractResource
    {
        public static Uri IconUri { get; set; }

        public AudioResource(string id, string title, string uri, IList<string> tags)
                : base(id, title, uri)
            {
                Type = ResourceType.Audio;
                TagList.AddRange(tags);
            }

        public override Uri Icon
        {
            get { return IconUri; }
        }

        [XmlIgnore]
        public override Uri PreviewUri
        {
            get { return Icon; }
        }

        public AudioResource()
        { }
        
    }
}
