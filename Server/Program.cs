using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
          
            //string ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            //Console.WriteLine("Server Ip: " + ip);

            //ReadServerConf(out int port, out int maxClients);

            //Server server = new Server(
            //    new ServerData(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp),
            //                    ip,
            //                    port,
            //                    maxClients));
            
            //server.Start();

            //Console.WriteLine("Server is running.");
            //Console.WriteLine("Press Esc to close.");
            //while (Console.ReadKey().Key != ConsoleKey.Escape);

            //server.Stop();
        }

        public static void ReadServerConf(out int port, out int maxClients)
        {
            do 
            {
                Console.Write("Enter port: ");
            }while(!int.TryParse(Console.ReadLine(), out port));

            do
            {
                Console.Write("Enter max number of clients: ");
            } while (!int.TryParse(Console.ReadLine(), out maxClients));
        }
    }
}
