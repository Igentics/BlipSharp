using System.Collections.Generic;
using BlipSharp.Utilities;

namespace BlipSharp.Resources
{
    public class Search : View
    {
        public static Search[] Get(Api blip, string query, int max = 12, string size = "big", string order = "")
        {
            var options = new Dictionary<string, object>() {
                {"query", query},
                {"max", max},
                {"size", size}
            };
            var t = blip.Get<Search[]>(QueryStringUtilities.BuildQueryString(options));
            return t.Result;
        }
    }
}