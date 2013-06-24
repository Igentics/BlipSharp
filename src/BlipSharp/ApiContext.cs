using BlipSharp.Interfaces;

namespace BlipSharp
{
    public class ApiContext : IApiContext
    {
        private readonly Api _api;
        private AuthToken _token;
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string PermissionsURL { get; set; }
        public bool IsAuthenticated { get; set; }
        public string AuthenticationToken { get; set; }
        public string UserSecret { get; set; }

        public ApiContext()
        {

        }

        public virtual bool GetPermission()
        {
            return false;
        }
    }
}
