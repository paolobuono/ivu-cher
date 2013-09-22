using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvengersUtd.Explore.Data.Resources
{
    [XmlType("VideoResource")]
    public class VideoResource : AbstractResource
    {
        public static Uri IconUri { get; set; }

        public VideoResource(string id, string title, string uri, IList<string> tags)
            : base(id, title, uri)
        {
            Type = ResourceType.Video;
            TagList.AddRange(tags);
        }

        [XmlIgnore]
        public Uri Icon
        {
            get { return IconUri; }
        }

        public VideoResource()
        { }

    }
}
