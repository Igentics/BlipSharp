using System;
using System.Linq;
using System.Text;
using BlipSharp.Resources;
using xBrainLab.Security.Cryptography;

namespace BlipSharp
{
    public class AuthToken
    {
        public AuthToken(string apiSecret, Time timeStamp, string temp_token = "")
        {
            _apiSecret = apiSecret;
            Timestamp = timeStamp;
            Nonce = RandomString(32);

        }
        private Time _time;
        private readonly string _apiSecret;

        public Time Timestamp
        {
            get { return _time; }
            private set { _time = value; }
        }

        public string Nonce { get; private set; }

        public static string RandomString(int size)
        {
            var chars = (Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N").ToLower());
            var random = new Random();
            var retVal = "";
            while (retVal.Length < size)
            {
                retVal += chars[random.Next(chars.Length)];
            }
            return retVal;
        }

        private void UpdateNonce()
        {
            Nonce = RandomString(32);
        }

        public string Token { get; set; }

        public static string GenerateQueryString(string timestamp, string nonce, string token, string secret, bool append=true)
        {
            var sb = new StringBuilder();
            if (append)
                sb.Append("&");
            
            var signature = MD5.GetHashString(timestamp + nonce.ToLowerInvariant() + token + secret);
            sb.AppendFormat("timestamp={0}&", timestamp);
            sb.AppendFormat("nonce={0}&", nonce.ToLowerInvariant());
            sb.AppendFormat("token={0}&", token);
            sb.AppendFormat("signature={0}&", signature.ToLowerInvariant());

            return sb.ToString();
        }

        public string Signature
        {
            get
            {
                var md5 = MD5.GetHashString(String.Concat(Timestamp.Timestamp.ToLower(), Nonce.ToLower(), (Token ?? (Token = String.Empty)).ToLower(), _apiSecret.ToLower()));
                return md5.ToLower();
            }
        }

        public void Authenticated(string token)
        {
            Token = token;
        }
    }
}