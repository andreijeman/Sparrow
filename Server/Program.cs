using System.Net;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server Ip: " + Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString());
            int port;
            do 
            {
                Console.Write("Enter port: ");
            }while(!int.TryParse(Console.ReadLine(), out port));

            Server server = new Server(port);
            server.Start();

            Console.WriteLine("Server is running.");
            Console.WriteLine("Press Esc to close.");
            while (Console.ReadKey().Key != ConsoleKey.Escape); 
        }
    }
}
