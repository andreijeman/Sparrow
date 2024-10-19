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
            //Console.Clear();
            //ConnectPage page = new ConnectPage();
            //page.Show();
            Test1();

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
            Table table = new Table(2, 1, 4, 2, null, null, null, ConsoleKey.Tab);
            Scroll scroll = new Scroll(20, 20, ConsoleKey.UpArrow, ConsoleKey.DownArrow) { Left = 0, Top = 0 }; 
            TextBox textBox = new TextBox(20, 4) { Left = 0, Top = 20, Action = scroll.AddText }; 
            
            table.AddElement(0, 0, scroll);
            table.AddElement(1, 0, textBox);
            table.Draw();
            table.Active = true;
        }

       
    }
}
