using System.Net;

namespace TwinklySharp
{
    public class Twinkly
    {
        public IPAddress IP { get; init; }
        public string DeviceId { get; init; }

        public Twinkly(IPAddress ip, string deviceId)
        {
            IP = ip;
            DeviceId = deviceId;
        }
    }
}