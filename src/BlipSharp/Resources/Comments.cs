using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlipSharp.Interfaces;
using BlipSharp.Utilities;

namespace BlipSharp.Resources
{
    public class Comments : ResourceBase, IGetResource<Comment>
    {
        public Comments() { }

        public static Comments[] Get(Api blipApi, int max=90,long since=0)
        {
            var options = new Dictionary<string, object>{
                {"max", max}
            };
            if (since > 0)
                options.Add("since", since);

            var t = blipApi.Get<Comments[]>(QueryStringUtilities.BuildQueryString(options));
            return t.Result;
        }

        public int CommentId { get; set; }
        public int ParentId { get; set; }
        public int EntryId { get; set; }
        public long Timestamp { get; set; }
        public string DisplayName { get; set; }
        public string[] Icons { get; set; }
        public string Content{ get; set; }
        public string Thumbnail { get; set; }
        public bool Reply { get; set; }
        public bool Unread { get; set; }
    }
}
