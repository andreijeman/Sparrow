using Logger;
using System.Net;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var conf = GetServerConf();
            Server server = new Server(conf.Ip, conf.Port, conf.ServerMaxConn, conf.ServerPassword, conf.Logger);
            server.Start();

            conf.Logger.LogInfo("Press Esc to close server.");
            while(Console.ReadKey().Key != ConsoleKey.Escape);
        }

        public static 
        (IPAddress Ip, int Port, int ServerMaxConn, string ServerPassword, ILogger Logger) 
        GetServerConf()
        {
            IPAddress ip; int port, serverMaxConn; string? serverPassword; ILogger logger; 

            logger = new ConsoleLogger();
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
                serverPassword = Console.ReadLine();
                if (serverPassword != null && serverPassword.Length < 20) break;
                else logger.LogWarning("Password max length must be 20.");
            }

            return (ip, port, serverMaxConn, serverPassword, logger);
        }
    }
}
