using BlipSharp.Interfaces;

namespace BlipSharp.Resources
{
    public abstract class ResourceBase
    {
        public string Version { get; set; }
        public string RequestId { get; set; }
        public Error Error { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}