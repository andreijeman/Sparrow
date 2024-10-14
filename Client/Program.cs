using ConsoleUI;
using Logger;
using System;
using System.Net;

namespace Client
{
    public class Program
    {
        ILogger logger = new ConsoleLogger();
        
        public static void Main(string[] args)
        {

            Client client = new Client(new ConsoleLogger());

            LoginPage page = new LoginPage();

            page.Render();
            //Console.BackgroundColor = ConsoleColor.Green;
            //Image img = ImageUtils.GetBorderedTextImage(1, 10, 1, 10, Config.Chat.BorderTemplate, ConsoleColor.Magenta, "", ConsoleColor.White);
            //ImageUtils.ConsolePrint(0, 0, img);

            while (true) page.DrawCursor();

        }

        public static void ReadConnConf(out IPAddress ip, out int port, out string serverPassword, out int serverMaxConn)
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
