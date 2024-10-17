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

        public static void Test1()
        {
            int r = 3, c = 2, s = 4;
            Table childTable = new Table(0, 0, r, c, 2, 1, ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.UpArrow, ConsoleKey.DownArrow);

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    Button el;
                    el = new Button(s * 2 * j + 2 * j, s * i + i, s * 2, s);
                    el.Action = HelloButtonAction;
                    childTable.AddElement(i, j, el);
                }
            }


            Table parentTable = new Table(0, 0, 1, 2, 2, 1, ConsoleKey.Tab, null, null, null);
            TextBox textBox = new TextBox(24, 0, 20, 16);


            parentTable.AddElement(0, 0, childTable);
            parentTable.AddElement(0, 1, textBox);

            parentTable.Active = true;
            parentTable.OriginLeft = 20;
            parentTable.OriginTop = 5;
            parentTable.Draw();
        }
    }
}
