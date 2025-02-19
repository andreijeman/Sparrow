using Logger;
using Client;
using Server.Postman;
using System.Net;

ChatClient client = new ChatClient(new ConsoleLogger());

client.PacketReceivedEvent += (packet) => Console.WriteLine($"{packet.Sender}: {packet.Data}");

ReadServerData(out var ip, out var port, out var password, out var username);

await client.ConnectToServerAsync(ip, port, password, username);

_ = client.StartReceivePacketsAsync();

while(true)
{
    Console.Write("> ");
    string? s = Console.ReadLine();
    _ = client.SendPacketAsync(new Packet { Label = Label.UserMessage, Data = s });
}

static void ReadServerData(out IPAddress ip, out int port, out string password, out string username)
{
    while (true)
    {
        Console.Write("IP: ");
        if (IPAddress.TryParse(Console.ReadLine(), out var res))
        {
            ip = res;
            break;
        }
    }

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
        Console.Write("Password: ");
        string? res = Console.ReadLine();
        if (res != null)
        {
            password = res;
            break;
        }
    }

    while (true)
    {
        Console.Write("Username: ");
        string? res = Console.ReadLine();
        if (res != null)
        {
            username = res;
            break;
        }
    }
}