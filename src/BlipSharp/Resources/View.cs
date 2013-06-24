using System.Collections.Generic;
using BlipSharp.Interfaces;
using BlipSharp.Utilities;

namespace BlipSharp.Resources
{
    public class View : ResourceBase, IGetResource<View[]>
    {
        #region Private Properties

        private static string _view;
        private static string _groupID;
        private static int _max;
        private static string _size;
        private static string _color;

        #endregion

        #region Public Properties

        public string DisplayName { get; set; }
        public string JournalTitle { get; set; }
        public string EntryId { get; set; }
        public string Thumbnail { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        #endregion

        #region Api Methods

        public static View[] Get(Api blip, string view = "", string group_id = "", int max = 12, string size = "big", string color = "color" )
        {
            _view = view;
            _groupID = group_id;
            _max = max;
            _size = size;
            _color = color;

            var options = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(_view))
                options.Add("view", _view);
            if (!string.IsNullOrEmpty(_groupID))
                options.Add("group_id", _groupID);

            options.Add("max", _max);
            options.Add("size", _size);
            options.Add("color", _color);

            var t = blip.Get<View[]>(QueryStringUtilities.BuildQueryString(options));
            return t.Result;
        }

        #endregion
    }
}