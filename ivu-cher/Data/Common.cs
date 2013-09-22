using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;

namespace AvengersUtd.Explore.Data
{
    public static class Common
    {

        public static string toWrapper(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return " ";

            }
            else return text;
        }

        public static string HtmlEncode(string text)
        {
            char[] chars = WebUtility.HtmlEncode(text).ToCharArray();
            StringBuilder result = new StringBuilder(text.Length + (int)(text.Length * 0.1));

            foreach (char c in chars)
            {
                int value = Convert.ToInt32(c);
                if (value > 127)
                    result.AppendFormat("&#{0};", value);
                else
                    result.Append(c);
            }

            return result.ToString();
        }
    }
}
