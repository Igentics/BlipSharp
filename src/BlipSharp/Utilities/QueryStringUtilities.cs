using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BlipSharp.Utilities
{
    public class QueryStringUtilities
    {
        public static string Encode(string toEncode)
        {
            return Uri.EscapeDataString(toEncode);
        }

        public static string AsBooleanValue(bool input)
        {
            return input ? "1" : "0";
        }

        public static string BuildQueryString(Dictionary<string, object> data)
        {
            if (data == null)
                return String.Empty;

            var sb = new StringBuilder();
            foreach (var d in data)
            {
                sb.AppendFormat("{0}={1}&", d.Key, d.Value);
            }

            return sb.ToString().TrimEnd('&');
        }
    }

    
}