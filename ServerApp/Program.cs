using System.Net;
using Logger;
using Server;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine($"IP: {Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString()}");
        ReadServerData(out int port, out int maxClients, out string password);
        
        ChatServer server = new ChatServer(new ConsoleLogger(), port, maxClients, password);

        server.Start();

        Console.WriteLine("Press ESC to exit");
        while (true)
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Escape)
            {
                server.Stop();
                break;
            }
        }
    }

    static void ReadServerData(out int port, out int maxClients, out string password)
    {
        while (true)
        {
            Console.Write("Port: ");
            if (int.TryParse(Console.ReadLine(), out var res))
            {
                port = res;
                break;
            }
        }

        while (true)
        {
            Console.Write("Max clients: ");
            if (int.TryParse(Console.ReadLine(), out var res))
            {
                maxClients = res;
                break;
            }
        }

        while (true)
        {
            Console.Write("Password: ");
            string? res = Console.ReadLine();
            if (res != null)
            {
                password = res;
                break;
            }
        }
    }
}

