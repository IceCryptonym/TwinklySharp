using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TwinklySharp
{
    public static class Discover
    {
        private const int PORT = 5555;
        private static readonly byte[] DISCOVER_MSG = { 0x01, 0x64, 0x69, 0x73, 0x63, 0x6f, 0x76, 0x65, 0x72 };

        public static async Task<Twinkly[]> FindTwinklyDevices(int timeout)
        {
            Dictionary<string, Twinkly> devices = new Dictionary<string, Twinkly>();
            
            using (UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Any, PORT)))
            {
                CancellationTokenSource cts = new CancellationTokenSource(timeout);
                client.Send(DISCOVER_MSG, DISCOVER_MSG.Length, "255.255.255.255", PORT);

                while (!cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        UdpReceiveResult result = await client.ReceiveAsync(cts.Token);

                        if (devices.ContainsKey(result.RemoteEndPoint.ToString()) ||
                            result.Buffer.Length != 21 || result.Buffer[4] != 'O' || result.Buffer[5] != 'K')
                        {
                            continue;
                        }

                        string deviceId = Encoding.UTF8.GetString(result.Buffer[6..^1]);
                        devices.Add(result.RemoteEndPoint.ToString(), new Twinkly(result.RemoteEndPoint.Address, deviceId));
                    } catch (System.Exception) { }
                }
            }

            return devices.Values.ToArray();
        }
    }
}
