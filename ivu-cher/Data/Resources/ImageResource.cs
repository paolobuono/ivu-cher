using System.Collections.Generic;
using System.Xml.Serialization;
using System;

namespace AvengersUtd.Explore.Data.Resources
{
    [XmlType("ImageResource")]
    public class ImageResource : AbstractResource
    {
        //[XmlElement(Order = 0)]
        //public ImageDescription Description { get; set; }
        public static Uri IconUri { get; set; }

        public ImageResource(string id, string title, string uri, IList<string> tags)
            :base(id,title,uri)//, ImageDescription description) : base(id, uri)
        {
            //Description = description;
            Type = ResourceType.Image;
            TagList.AddRange(tags);
        }

        public Uri Icon
        {
            get { return IconUri; }
        }

        [XmlIgnore]
        public override Uri PreviewUri
        {
            get { return Uri; }
        }

        public ImageResource()
        {}

        
    }
}
