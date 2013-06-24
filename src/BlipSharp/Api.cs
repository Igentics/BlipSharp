using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using BlipSharp.EventArgs;
using BlipSharp.Interfaces;
using BlipSharp.Json;
using BlipSharp.Resources;
using BlipSharp.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlipSharp
{
    public class Api
    {
        private const string ApiEndpoint = "api.blipfoto.com";
        
        private static string _serviceEndpoint;
        
        private bool _isAuthenticated;
        private AuthToken _auth;

        public Api() : this(ApiEndpoint, null)
        {

        }

        public string Authenticate(string temp_token, string nonce, string timestamp, string signiture, string device_id = "")
        {
            //var tokenRequest = Get<JToken>() Token.Get(this, temp_token, nonce, timestamp, signiture, device_id);
            var tokenUri = new UriBuilder("https", ServiceEndpoint, 443, String.Format("/{0}/{1}.{2}", "v3", "token", "json")) {
                Query = Query() + "&temp_token=" + temp_token + "&device_id=" + device_id + AuthToken.GenerateQueryString(ApiTimestamp, AuthToken.RandomString(32), "", Context.ApiSecret)
            };
            var tokenRequest = JsonConvert.DeserializeObject<dynamic>(MakeAsyncRequest<JToken>(tokenUri.Uri, "GET").Result.ToString());
            if (tokenRequest != null)
            {
                var args = AuthenticationArgs.FromJson(tokenRequest["data"].ToString());
                Context.AuthenticationToken = args.Token;
                if (Authenticated != null)
                    Authenticated(this, args);

                return Context.AuthenticationToken;
            }

            return String.Empty;
        }

        public Api(IApiContext context) : this(ApiEndpoint, context){ }

        protected static string ServiceEndpoint
        {
            get { return _serviceEndpoint ?? (_serviceEndpoint = ApiEndpoint); }
            set { _serviceEndpoint = value; }
        }

        public Api(string endpoint, IApiContext context)
        {
            Context = context;
            _serviceEndpoint = endpoint;
        }

        public IApiContext Context { get; set; }

        public async Task<T> Get<T>() where T : class
        {
            return await Get<T>("");
        }

        public async Task<T> Put<T>() where T : class
        {
            return await Put<T>("", null);
        }

        public async Task<T> Post<T>() where T : class
        {
            return await Post<T>("", null);
        }

        public async Task<T> Delete<T>() where T : class
        {
            return await Delete<T>("");
        }

        public JsonSerializerSettings JsonSetting
        {
            get
            {
                return new JsonSerializerSettings
                {
                    ContractResolver = new SnakeCasePropertyNamesContractResolver()
                };
            }
        }

        public bool IsAuthenticated
        {
            get { return Context != null && !String.IsNullOrEmpty(Context.AuthenticationToken); }
        }

        public T ConvertData<T>(object oData) where T : class
        {
            var obj = Newtonsoft.Json.Linq.JObject.Parse(oData.ToString());

            return obj["data"].ToObject<T>(new JsonSerializer
            {
                ContractResolver = new SnakeCasePropertyNamesContractResolver(),
            });
        }

        public async Task<T> Put<T>(string options, T content) where T : class
        {
            var url = new UriBuilder("http", ServiceEndpoint, 80, String.Format("/{0}/{1}.{2}", "v3", typeof(T).Name.ToLowerInvariant(), "json")) {
                Query = Query(options)
            };

            var response = await MakeAsyncRequest(url.Uri, "PUT", content) as HttpResponseMessage;
            if (response != null)
            {
                var result = ConvertData<T>(await response.Content.ReadAsStringAsync());
                return result;
            }
            return null;
        }

        public async Task<T> Post<T>(string options, T content) where T : class
        {
            var url = new UriBuilder("http", ServiceEndpoint, 80, String.Format("/{0}/{1}.{2}", "v3", typeof(T).Name.ToLowerInvariant(), "json")) {
                Query = Query(options)
            };

            var response = await MakeAsyncRequest(url.Uri, "POST", content) as HttpResponseMessage;
            var result = ConvertData<T>(await response.Content.ReadAsStringAsync());
            return result;
        }

        public async Task<T> Delete<T>(string options) where T : class
        {
            var url = new UriBuilder("http", ServiceEndpoint, 80, String.Format("/{0}/{1}.{2}", "v3", typeof(T).Name.ToLowerInvariant(), "json")) {
                Query = Query(options)
            };

            var response = await MakeAsyncRequest<T>(url.Uri, "DELETE");
            var result = ConvertData<T>(response);
            return result;
        }

        public async Task<T> Get<T>(string options) where T : class
        {
            var isSecure = typeof(T) == typeof(Token);
            var url = new UriBuilder(isSecure ? "https" : "http", ServiceEndpoint, isSecure ? 443:80, String.Format("/{0}/{1}.{2}", "v3", typeof(T).Name.ToLowerInvariant().Replace("[]", ""), "json")) {
                Query = typeof(T) == typeof(Time) ? Query() : Query(options)
            };
            LogManager.Log("Calling {0} with Params {1}", ServiceEndpoint, url.Query);
            LogManager.Log(url.ToString());
            var response = await MakeAsyncRequest<T>(url.Uri, "GET");
            LogManager.Log("Response: {0}", response);
            var result = ConvertData<T>(response);
            return result;
        }

        private string _time = String.Empty;
        private object _lock = new object();
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime UnixTimeToDateTime(string text)
        {
            double seconds = double.Parse(text, CultureInfo.InvariantCulture);
            return _epoch.AddSeconds(seconds);
        }

        private string ApiTimestamp
        {
            get
            {
                var _time2 = Math.Floor((DateTime.UtcNow.AddMinutes(1).AddSeconds(-6) - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
                if (_time == String.Empty)
                {
                    lock (_lock)
                    {
                        var timeResponse = MakeAsyncRequest<JToken>(new UriBuilder("http", ServiceEndpoint, 80, String.Format("/{0}/{1}.{2}", "v3", "time", "json"))
                        {
                            Query = Query()
                        }.Uri, "GET").Result;
                        _time = ConvertData<JToken>(timeResponse)["timestamp"].Value<String>();
                    }
                    Debug.WriteLine("Time2: {0} || Api: {1}", _time2, _time);
                }
                return _time2;

            }
        }

        private object _authLock = new object();
        private AuthToken auth
        {
            get
            {
                if (_auth == null && Context != null)
                {
                    lock (_authLock)
                    {
                        _auth = new AuthToken(Context.ApiSecret, new Time {
                            Timestamp = ApiTimestamp
                        }) {
                            Token = Context.AuthenticationToken
                        };
                    }
                }
                return _auth;
            }
        }

        public virtual string Query(string append)
        {
            if (Context == null)
                throw new Exception("Api Context is not available");

            append = append.TrimStart(new[] { '?', '&' }).TrimEnd(new[] { '?', '&' });
            if (IsAuthenticated)
            {
                var authQuery = AuthToken.GenerateQueryString(ApiTimestamp, AuthToken.RandomString(32), Context.AuthenticationToken, Context.UserSecret);
                return String.Format("api_key={0}&{1}{2}{3}", Context.ApiKey, append, authQuery, Debugger.IsAttached ? "debug=1" : "");
            }
            return String.Format("api_key={0}&{1}&{2}", Context.ApiKey, append, Debugger.IsAttached ? "debug=1" : "");
        }

        public virtual string Query()
        {
            return String.Format("api_key={0}{1}", Context.ApiKey, Debugger.IsAttached ? "&debug=1":"");

        }

        public static async Task<object> MakeAsyncRequest<T>(Uri url, string method, T content = null) where T : class
        {
            var client = new HttpClient();
            object response = null;

            switch (method.ToUpperInvariant())
            {
                case "GET":
                    response = await client.GetStringAsync(url);
                    break;
                case "PUT":
                    if (content != null)
                    {
                        var json = JsonConvert.SerializeObject(content);
                        var stringContent = new StringContent(json);
                        response = await client.PutAsync(url, stringContent);
                    }
                    break;
                case "POST":
                    if (content != null)
                    {
                        var json = JsonConvert.SerializeObject(content);
                        var stringContent = new StringContent(json);
                        response = await client.PostAsync(url, stringContent);
                    }
                    break;
                case "DELETE":
                    response = await client.DeleteAsync(url);
                    break;
            }

            return response;
        }

        public event Action<object, AuthenticationArgs> Authenticated;
    }
}