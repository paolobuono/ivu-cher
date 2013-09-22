using System.Drawing.Imaging;
using System.Xml.Serialization;

namespace AvengersUtd.Explore.Data.Resources
{
    public struct ImageDescription
    {
        [XmlAttribute]
        public int Width { get; set; }
        [XmlAttribute]
        public int Height { get; set; }

        [XmlAttribute]
        public ImageFormat ImageFormat { get; set; }
        [XmlAttribute]
        public PixelFormat PixelFormat { get; set; }

    }
}
