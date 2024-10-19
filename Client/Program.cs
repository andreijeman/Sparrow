using Client.Pages;
using ConsoleUI.Elements;
using Logger;
using System.Net;

namespace Client
{
    public class Program
    {
        ILogger logger = new ConsoleLogger();
        
        public static void Main(string[] args)
        {
            //Test1(); 
            Console.Clear();
            ConnectPage page = new ConnectPage();
            page.Show();
            Thread.Sleep(100000);
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

        public static void HelloButtonAction()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Hello World!");
        }

       
    }
}
