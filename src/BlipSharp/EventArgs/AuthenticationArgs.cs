using Newtonsoft.Json;

namespace BlipSharp.EventArgs
{
    public class AuthenticationArgs
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }
        [JsonProperty(PropertyName = "secret")]
        public string Secret { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings {
                ContractResolver = new Json.SnakeCasePropertyNamesContractResolver()
            });
        }

        public static AuthenticationArgs FromJson(string json)
        {
            return JsonConvert.DeserializeObject<AuthenticationArgs>(json, new JsonSerializerSettings {
                ContractResolver = new Json.SnakeCasePropertyNamesContractResolver()
            });
        }
    }
}