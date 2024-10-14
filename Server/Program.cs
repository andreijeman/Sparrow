using Logger;
using System.Net;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ReadServerConf(out IPAddress ip, out int port, out int serverMaxConn, out string serverPassword);
            Server server = new Server(ip, port, serverMaxConn, serverPassword, new ConsoleLogger());
            server.Start();

            while(Console.ReadKey().Key != ConsoleKey.Escape);
        }

        public static void ReadServerConf(out IPAddress ip, out int port, out int serverMaxConn, out string serverPassword)
        {
            var logger = new ConsoleLogger();

            ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1];

            logger.LogInfo("Server Ip is " + ip.ToString());

            while (true)
            {
                Console.Write("Enter server port: ");
                if (int.TryParse(Console.ReadLine(), out port) && port < 65536) break;
                else logger.LogWarning("Invalid port format.");
            }

            while (true)
            {
                Console.Write("Enter server max number of connections: ");
                if (int.TryParse(Console.ReadLine(), out serverMaxConn)) break;
                else logger.LogWarning("Invalid number format.");
            }

            while (true)
            {
                Console.Write("Enter server password: ");
                string? l = Console.ReadLine();
                if (l != null && l.Length < 20) { serverPassword = l; break; }
                else logger.LogWarning("Password max length must be 20.");
            }

            logger.LogInfo("Press Esc to close server.");
        }
    }
}
