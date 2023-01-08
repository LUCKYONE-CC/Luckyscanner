using System.Net.Sockets;
using System.Net;

namespace Luckyscanner.Functions
{
    public class Portscan
    {
        public string target { get; set; } = string.Empty;
        public string targetIP { get; set; } = string.Empty;
        public TcpClient socket { get; set; } = null!;

        // A host can have multiple IP addresses!
        public IPAddress[] GetIPsByName(string hostName, bool ip4Wanted, bool ip6Wanted)
        {
            try
            {
                // Check if the hostname is already an IPAddress
                IPAddress outIpAddress;
                if (IPAddress.TryParse(hostName, out outIpAddress) == true)
                    return new IPAddress[] { outIpAddress };
                //<----------

                IPAddress[] addresslist = Dns.GetHostAddresses(hostName);

                if (addresslist == null || addresslist.Length == 0)
                    return new IPAddress[0];
                //<----------

                if (ip4Wanted && ip6Wanted)
                    return addresslist;
                //<----------

                if (ip4Wanted)
                    return addresslist.Where(o => o.AddressFamily == AddressFamily.InterNetwork).ToArray();
                //<----------

                if (ip6Wanted)
                    return addresslist.Where(o => o.AddressFamily == AddressFamily.InterNetworkV6).ToArray();
                //<----------

                return new IPAddress[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return new IPAddress[0];
            }
        }
        public IEnumerable<int> Scan(string ipaddress, int startPort, int endPort)
        {
            IPAddress ipo = IPAddress.Parse(ipaddress);
            IPEndPoint ipEo = new IPEndPoint(ipo, 1);
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  // Prepare connection
            for (int port = startPort; port <= endPort; port++)
            {
                ipEo.Port = port;
                bool isOpen = true;
                try
                {
                    sock.Connect(ipEo);  // Establish connection
                }catch
                {
                    isOpen = false;
                }
                finally
                {
                    sock.Close();
                }
                if(isOpen)
                {
                    yield return port;
                }
            }
        }
    }
}
