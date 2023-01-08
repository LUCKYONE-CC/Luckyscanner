using Luckyscanner.Commands;
using System.Net;
using System.Net.Sockets;

namespace Luckyscanner
{
    public class Program
    {
        public static CommandHandler ch = new CommandHandler();
        static void Main(string[] args)
        {
            //target = "[website]"; //Example: scanme.nmap.org

            ch.runCommand(Console.ReadLine());
        }
    }
}