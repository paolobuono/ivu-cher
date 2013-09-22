using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AvengersUtd.Explore.Data.Resources
{
    [XmlType("TextResource")]
    public class TextResource:AbstractResource
    {
        public static Uri IconUri { get; set; }
        public TextResource(string id, string title, string uri, IList<string> tags)
            : base(id, title, uri)
        {
            Type = ResourceType.Text;
            TagList.AddRange(tags);
        }

        public override Uri  Icon
        {
            get { return IconUri; }
        }

        public TextResource()
        { }
    }
}
