using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Explore.Data.Resources;

namespace AvengersUtd.Explore.Data.Elements
{
    public class ResourceRule
    {
        public ResourceType Type { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }

        public ResourceRule(ResourceType type, int min, int max)
        {
            Type = type;
            Min = min;
            Max = max;
        }

        public string Bounds
        {
            get
            {
                if (Min == Max)
                    return string.Format("{0}", Min);
                else
                    return string.Format("{0}-{1}", Min, Max);
            }
        }
    }
}
