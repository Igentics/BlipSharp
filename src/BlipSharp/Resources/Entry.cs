using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using BlipSharp.Interfaces;
using BlipSharp.Utilities;
using Newtonsoft.Json;

namespace BlipSharp.Resources
{
    public sealed class Entry : ResourceBase, IGetResource<Entry>
    {
        private static string _entryID;
        private static string _displayName;
        private static bool _returnExtended;
        private static bool _returnActions;
        private static bool _returnComments;
        private static bool _returnCommentReplies;
        private static bool _returnIds;
        private static bool _returnDimensions;
        private static bool _returnExif;

        public string EntryId { get; set; }
        
        public string DisplayName { get; set; }
        
        public string JournalTitle { get; set; }
        
        public int Member { get; set; }
        
        public string Date { get; set; }
        
        public string Description { get; set; }
        
        public string Image { get; set; }
        
        public string LargeImage { get; set; }
        
        public string Thumbnail { get; set; }

        public string Title { get; set; }
        
        public string Url { get; set; }
        
        public int? RatingTotal { get; set; }
        
        public int? RatingCount { get; set; }
        
        public string[] tags { get; set; }
        
        public int? Views { get; set; }
        
        public string RawDescription { get; set; }
        
        public Actions Actions { get; set; }
        
        public Comments[] Comments { get; set; }
        
        public IDs Ids { get; set; }
        
        public Dimensions Dimensions { get; set; }
        
        public Exif Exif { get; set; }
        
        public Location Location { get; set; }

        public static Entry Get(Api blip, string entry_id = "", string display_name = "", bool returnExtended = false, bool returnActions = false, bool return_comments = false, bool return_comment_replies = false, bool return_ids = false, bool return_dimensions = false, bool return_exif = false)
        {
            _entryID = entry_id;
            _displayName = display_name;
            _returnExtended = returnExtended;
            _returnActions = returnActions;
            _returnComments = return_comments;
            _returnCommentReplies = return_comment_replies;
            _returnIds = return_ids;
            _returnDimensions = return_dimensions;
            _returnExif = return_exif;

            var options = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(_entryID))
                options.Add("entry_id", _entryID);

            if (!String.IsNullOrEmpty(_displayName))
                options.Add("display_name", _displayName);

            if (_returnExtended)
                options.Add("return_extended", QueryStringUtilities.AsBooleanValue(_returnExtended));
            if (_returnActions)
                options.Add("return_Actions", QueryStringUtilities.AsBooleanValue(_returnActions));
            if (_returnComments)
                options.Add("return_comments", _returnCommentReplies ? 2 : 1);
            if (_returnIds)
                options.Add("return_ids", QueryStringUtilities.AsBooleanValue(_returnIds));
            if (_returnDimensions)
                options.Add("return_dimensions", QueryStringUtilities.AsBooleanValue(_returnDimensions));
            if (_returnExif)
                options.Add("return_exif", QueryStringUtilities.AsBooleanValue(_returnExif));

            var t = blip.Get<Entry>(QueryStringUtilities.BuildQueryString(options));
            return t.Result;
        }
    }
    
    public class Actions
    {
        public int Comment { get; set; }
        public int CommentLinks { get; set; }
        public int Subscribe { get; set; }
        public int Unsubscribe { get; set; }
        public int Favourite { get; set; }
        public int Rate { get; set; }
        public int Modify { get; set; }
        public int Delete { get; set; }
    }

    //[Obsolete("Use Resources.Comments")]
    //public class Comments
    //{
    //    public int CommentId { get; set; }
    //    public string DisplayName { get; set; }
    //    public string Content { get; set; }
    //    public string[] Icons { get; set; }
    //    public int Reply { get; set; }
    //    public Comments[] Replies { get; set; }
    //}

    // ReSharper disable InconsistentNaming
    public class IDs
    {
        public string Next { get; set; }
        public string Previous { get; set; }
    }
    // ReSharper restore InconsistentNaming

    public class Dimensions
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? LargeWidth { get; set; }
        public int? LargeHeight { get; set; }
    }

    public class Exif
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string FNumber { get; set; }
        public string ExposureTime { get; set; }
        public string FocalLength { get; set; }
        public string ISO { get; set; }
        public override string ToString()
        {
            var exif = String.Empty;
            var content = new List<string>() {
                Model,
                FNumber,
                ExposureTime,
                FocalLength,
                ISO
            };
            exif = content.Where(s => !String.IsNullOrEmpty(s)).Aggregate(exif, (current, s) => current + (s + " : "));
            exif = exif.TrimEnd(new[] {' ', ':'});
            return exif;
        }
    }

    public class Location
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int Accuracy { get; set; }
    }
}