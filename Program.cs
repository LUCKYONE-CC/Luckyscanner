using System.Net;
using System.Net.Sockets;

namespace Luckyscanner
{
    public class Program
    {
        public static string target = "";
        public static string targetIP = "";
        public static TcpClient socket = new TcpClient();
        static void Main(string[] args)
        {
            target = "[website]"; //Example: scanme.nmap.org
            //Removes unnecessary characters of the URL, otherwise an error occurs
            switch (target)
            {
                case string s when s.StartsWith("http://www"):
                    target = target.Replace("http://www.", "");
                    break;
                case string s when s.StartsWith("https://www"):
                    target = target.Replace("https://www.", "");
                    break;
                case string s when s.StartsWith("https://"):
                    target = target.Replace("https://", "");
                    break;
                case string s when s.StartsWith("http://"):
                    target = target.Replace("http://", "");
                    break;
                case string s when s.StartsWith("www."):
                    target = target.Replace("www.", "");
                    break;
            }
            IPAddress[] IPs = GetIPsByName(target, true, false);
            if (GetIPsByName(target, true, false).Count() == 1)
            {
                targetIP = GetIPsByName(target, true, false)[0].ToString();
                Console.WriteLine(targetIP);
            }
            else
            {
                Console.WriteLine("Found more than one ip:");
                targetIP = GetIPsByName(target, true, false)[0].ToString();
                foreach (var ipAdress in GetIPsByName(target, true, false))
                {
                    Console.WriteLine(ipAdress);
                }
            }
            Console.WriteLine("Press enter to scan all ports");
            Console.ReadKey();
            //Checks all Ports
            for (int port = 1; port <= 65535; port++)
            {
                if(targetIP != null)
                {
                    Scan(targetIP, port);
                }
            }
        }

        // A host can have multiple IP addresses!
        private static IPAddress[] GetIPsByName(string hostName, bool ip4Wanted, bool ip6Wanted)
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

        private static void Scan(string ipaddress, int port)
        {
            IPAddress ipo = IPAddress.Parse(ipaddress);
            IPEndPoint ipEo = new IPEndPoint(ipo, port);
            try
            {
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  // Prepare connection
                sock.Connect(ipEo);  // Establish connection
                Console.WriteLine($"Port {port} is open");  // Positiv Text ausgeben
                sock.Close();  //Close connection is no longer needed
            }
            catch (Exception)
            {
                //No open port, because an error occurred above.
                Console.WriteLine($"Port {port} is closed");
            }
        }
    }
}