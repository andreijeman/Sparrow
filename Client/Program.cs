using ConsoleUI;
using Logger;
using System.Net;

namespace Client
{
    public class Program
    {
        ILogger logger = new ConsoleLogger();
        
        public static void Main(string[] args)
        {

            TextBox a = new TextBox(10, 10, 20, 5);
            a.BorderTemplate = Config.Chat.BorderTemplate;
            a.BorderColor = ConsoleColor.Magenta;
            a.TextColor = ConsoleColor.White;
            a.Render();

            TextBox b = new TextBox(31, 10, 20, 5);
            b.BorderTemplate = Config.Chat.BorderTemplate;
            b.BorderColor = ConsoleColor.Magenta;
            b.TextColor = ConsoleColor.White;
            b.Render();

            TextBox c = new TextBox(10, 15, 20, 5);
            c.BorderTemplate = Config.Chat.BorderTemplate;
            c.BorderColor = ConsoleColor.Magenta;
            c.TextColor = ConsoleColor.White;
            c.Render();

            Table t = new Table(2, 2);
            t.AddElement(0, 0, a);
            t.AddElement(0, 1, b);
            t.AddElement(1, 0, c);

            t.Active = true;


            while (true) ;
           

        }

        public static void MyAction()
        {
            Console.Beep();
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
