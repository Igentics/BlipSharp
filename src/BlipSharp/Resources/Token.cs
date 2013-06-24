using System;
using System.Collections.Generic;
using BlipSharp.Interfaces;
using BlipSharp.Utilities;

namespace BlipSharp.Resources
{
    public class Token : ResourceBase, IGetResource<Token>, ISecure
    {
        public const string NOTAUTHENTICATED = "Error";
        public string TempToken { get; set; }
        public string DeviceId { get; set; }
        public string DisplayName { get; set; }
        public string token { get; set; }
        public string Secret { get; set; }

        public static Token Get(Api blip, string temp_token, string nonce, string timestamp, string signiture, string device_id = "")
        {
            var options = new Dictionary<string, object> {
                {"temp_token", temp_token},
                {"nonce", nonce},
                {"timestamp", timestamp},
                {"signature", signiture},
                {"token", String.Empty},
            };
            if(!String.IsNullOrEmpty(device_id))
                options.Add("device_id", device_id);
            
            var t = blip.Get<Token>(QueryStringUtilities.BuildQueryString(options));
            return t.Result;
        }
    }
}