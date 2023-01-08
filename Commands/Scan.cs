

using Luckyscanner.Functions;
using Luckyscanner.Helper;
using System.Net;

namespace Luckyscanner.Commands
{
    public class Scan : Command
    {
        public static Portscan portscan = new Portscan();
        public Scan(string name) : base(name)
        {

        }

        public override string execute(Dictionary<string, string> args)
        {
            try
            {
                int startPort = 1;
                int endPort = 65535;
                if (!args.ContainsKey("t"))
                {
                    throw new KeyNotFoundException("Parameter -t was not given.");
                }

                if (args.ContainsKey("p"))
                {
                    try
                    {
                        var ports = args["p"].Split("-").Select(x => Int32.Parse(x)).ToArray();
                        startPort = ports[0];
                        endPort = ports[1];
                    }
                    catch
                    {
                        startPort = Int32.Parse(args["p"]);
                        endPort = Int32.Parse(args["p"]);
                    }
                }

                portscan.target = args["t"];

                //Removes unnecessary characters of the URL, otherwise an error occurs
                switch (portscan.target)
                {
                    case string s when s.StartsWith("http://www"):
                        portscan.target = portscan.target.Replace("http://www.", "");
                        break;
                    case string s when s.StartsWith("https://www"):
                        portscan.target = portscan.target.Replace("https://www.", "");
                        break;
                    case string s when s.StartsWith("https://"):
                        portscan.target = portscan.target.Replace("https://", "");
                        break;
                    case string s when s.StartsWith("http://"):
                        portscan.target = portscan.target.Replace("http://", "");
                        break;
                    case string s when s.StartsWith("www."):
                        portscan.target = portscan.target.Replace("www.", "");
                        break;
                }

                IPAddress[] IPs = portscan.GetIPsByName(portscan.target, true, false);
                if (portscan.GetIPsByName(portscan.target, true, false).Count() == 1)
                {
                    portscan.targetIP = portscan.GetIPsByName(portscan.target, true, false)[0].ToString();
                    ConsoleFormatter.ColoredOutput(portscan.targetIP, ConsoleColor.Magenta);
                }
                else
                {
                    Console.WriteLine("Found more than one ip:");
                    portscan.targetIP = portscan.GetIPsByName(portscan.target, true, false)[0].ToString();
                    foreach (var ipAdress in portscan.GetIPsByName(portscan.target, true, false))
                    {
                        ConsoleFormatter.ColoredOutput(ipAdress.ToString(), ConsoleColor.Yellow);
                    }
                }
                Console.WriteLine("\n");
                //Checks all Ports
                List<int> openPorts = new List<int>();
                if (portscan.targetIP != null)
                {
                    foreach (var port in portscan.Scan(portscan.targetIP, startPort, endPort))
                    {
                        openPorts.Add(port);
                        ConsoleFormatter.ColoredOutput($"Port {port} is open", ConsoleColor.Green);
                    }
                }
                if (openPorts.Count == 0)
                {
                    ConsoleFormatter.ColoredOutput("No open Ports!", ConsoleColor.Red);
                }
                Console.WriteLine("\n");
                ConsoleFormatter.ColoredOutput("----Finished----", ConsoleColor.Blue);

            }catch(Exception ex)
            {
                ConsoleFormatter.ColoredOutput(ex.ToString(), ConsoleColor.Red);
            }
            return "";
        }
    }
}
