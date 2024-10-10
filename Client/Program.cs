using System;
using System.Net;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string ip = Console.ReadLine();
            int port = int.Parse(Console.ReadLine());
            string username = Console.ReadLine();
            ClientService client = new ClientService();

            client.ConnectToServer(IPAddress.Parse(ip), port, username);  
            client.Start();
        }
    }
}
