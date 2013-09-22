using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System;

namespace AvengersUtd.Explore.Data.Resources
{
    [XmlInclude(typeof(ImageResource)), XmlInclude(typeof(AudioResource)), XmlInclude(typeof(TextResource))]
    [XmlType("Resource")]
    public class AbstractResource
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Title { get; set; }

        [XmlIgnore]
        public Uri Uri { get; set; }

        [XmlIgnore]
        public virtual Uri Icon
        {
            get { return Uri; }
        }

        [XmlIgnore]
        public virtual Uri PreviewUri
        {
            get { return Icon; }
        }

        [XmlElement(ElementName="Uri", Order = 0)]
        public string UriString {
            get
            {
                return Uri.AbsolutePath;
            }
            set { Uri = new Uri(value); }
        }

        [XmlArray(ElementName = "TagList",Order = 1)]
        [XmlArrayItem("Tag")]
        public List<string> TagList { get; set; }

        public string Tags
        {
            get
            {
                if (TagList.Count == 0)
                    return string.Empty;

                StringBuilder sb = new StringBuilder();
                foreach (string tag in TagList)
                {
                    sb.AppendFormat("{0},", tag);
                }

                return sb.ToString(0, sb.Length - 1);
            }
        }


        [XmlAttribute]
        public ResourceType Type { get; set; }

        protected AbstractResource(string id, string title, string uri)
        {
            Id = id;
            Title = title;
            Uri = string.IsNullOrEmpty(uri) ? new Uri("http://www.avengersutd.com") : new Uri(uri);
            TagList = new List<string>();
        }
        protected AbstractResource() : this (string.Empty, string.Empty, string.Empty)
        {}
    }
}
