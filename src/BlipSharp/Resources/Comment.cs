using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlipSharp.Interfaces;
using BlipSharp.Utilities;

namespace BlipSharp.Resources
{
    public abstract class Comment : IPostResource
    {
        protected Comment()
        {

        }

        protected Comment(string comment, int entryId, int replyToComment)
        {
            CommentContent = comment;
            EntryId = entryId;
            ReplyToCommendId = replyToComment;
        }

        public int EntryId { get; set; }
        public string CommentContent { get; set; }
        public string[] Icons { get; set; }
        protected int ReplyToCommendId { get; set; }

        public virtual bool Save(Api blip)
        {
            return blip.Post(QueryStringUtilities.BuildQueryString(new Dictionary<string, object> {
                {"entry_id", EntryId},
                {"reply_to_comment_id", ReplyToCommendId},
                {"comment", CommentContent}
            }), this).Result != null;

        }

        
    }
}