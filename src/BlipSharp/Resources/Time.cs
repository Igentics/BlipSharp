using BlipSharp.Interfaces;

namespace BlipSharp.Resources
{
    public class Time : IGetResource<Time>
    {
        public string Timestamp { get; set; }
        public static Time Get(Api blip)
        {
            var t = blip.Get<Time>().Result;
            return t;
        }
    }
}