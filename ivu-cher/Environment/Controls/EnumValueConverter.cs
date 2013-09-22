using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace AvengersUtd.Explore.Environment.Controls
{
    /*
 * Released under Microsoft Public License (Ms-PL) 
 * Sponsored by Development Platform Evangelism unit of Microsoft Israel
 * 
 * Copyright © 2008 by Tamir Khason
 * http://blogs.microsoft.co.il/blogs/tamir/
 * http://sharpsoft.net/
 * 
 * More information including licensing and term of use
 * can be found on http://www.codeplex.com/SilverlightRTL/
 * 
 */




    public class EnumValueConverter<T> : TypeConverter where T:struct
    {
        static EnumValueConverter()
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException("Cannot use this type for conversion");
            }
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return Type.GetTypeCode(sourceType) == TypeCode.String;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string enumValue = value as string;
            if (enumValue != null)
            {
                return Enum.Parse(typeof(T), enumValue, true);
            }
            return (T)value;
        }

    }
}

        

