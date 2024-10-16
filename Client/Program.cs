using ConsoleUI;
using Logger;
using System.Data;
using System.Net;

namespace Client
{
    public class Program
    {
        ILogger logger = new ConsoleLogger();
        
        public static void Main(string[] args)
        {

            int r = 5;
            int c = 10;

            int s = 5;

            Table t = new Table(r, c);


            for (int i = 0; i < r; i++)
            {
                for(int j = 0; j < c; j++)
                {
                    Element el;
                    if((i + j) % 2 == 0) el = new Button(s * 2 * j + 2 * j, s * i + i, s * 2, s);
                    else el = new TextBox(s * 2 * j + 2 * j, s * i + i, s * 2, s);
                    el.Render();
                    t.AddElement(i, j, el);
                }
            }

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
